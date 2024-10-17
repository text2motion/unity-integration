
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Text2Motion.Settings;
using Text2Motion.Utils;
using Text2MotionClientAPI.Api;
using Text2MotionClientAPI.Client;
using Text2MotionClientAPI.Model;
using UnityEngine;

namespace Text2Motion.Server
{
    public class T2MServerRequestWrapper
    {
        private const string ServerBasePath = "https://api.text2motion.ai";
        private readonly string _gameObjectName;
        private readonly T2MSettings _settingsCache;
        private Skeleton _tPose;

        public T2MServerRequestWrapper(string gameObjectName, T2MSettings settingCache)
        {
            _gameObjectName = gameObjectName;
            _settingsCache = settingCache;
        }

        public Task<T2MFrames> GenerateRequestAsync(
            string prompt,
            string requestName,
            CancellationToken cancellationToken = default)
        {
            Configuration config = new()
            {
                BasePath = ServerBasePath
            };
            // Configure API key authorization: APIKeyHeader
            config.AddApiKey("x-apikey", _settingsCache.APIKey);

            var apiInstance = new GenerateApi(config);
            var generateRequestBody = new GenerateRequestBody(
                prompt: prompt,
                targetSkeleton: _tPose
            );

            // Generate
            var task = apiInstance.GenerateApiGeneratePostAsync(generateRequestBody, cancellationToken).ContinueWith(
                task =>
                {
                    var t2mFrames = T2MFrames.FromJson(task.Result.Result);
                    t2mFrames.Prompt = prompt;
                    SaveT2MFrames(
                        t2mFrames,
                        requestName);

                    return t2mFrames;
                }, cancellationToken
            );
            return task;
        }


        public void SaveTPose(Transform rootBone, Dictionary<string, List<decimal>> tPoseMapping = null)
        {
            _tPose = GetTargetSkeleton(rootBone, tPoseMapping);
            string tPoseFilePath = GetTPoseFilePath();
            Directory.CreateDirectory(Path.GetDirectoryName(tPoseFilePath));
            File.WriteAllText(tPoseFilePath, JsonConvert.SerializeObject(_tPose));
            Debug.Log("T-Pose data saved to: " + tPoseFilePath);
        }

        public void LoadTPoseFromFile()
        {
            var tPoseFilePath = GetTPoseFilePath();
            if (!File.Exists(tPoseFilePath))
            {
                throw new InvalidOperationException("T-Pose data is not saved, please initialize the T-Pose first.");
            }
            else
            {
                var tPoseStr = File.ReadAllText(tPoseFilePath);
                _tPose = JsonConvert.DeserializeObject<Skeleton>(tPoseStr);
                // Debug.Log("Loaded T-Pose data from: " + tPoseFilePath);
            }
        }

        public bool IsTPoseLoaded()
        {
            return _tPose != null;
        }

        public string GetTPoseFilePath()
        {
            string tPoseFileDir = Path.Join(_settingsCache.TPoseDataDir, _gameObjectName);
            return Path.Join(tPoseFileDir, _settingsCache.TPoseDataName);
        }

        public bool IsTPoseFileExists()
        {
            return File.Exists(GetTPoseFilePath());
        }

        public void ClearTPose()
        {
            _tPose = null;
        }

        private static Skeleton GetTargetSkeleton(Transform rootBone, Dictionary<string, List<decimal>> boneMatrixOverrides = null)
        {
            if (rootBone == null)
            {
                throw new InvalidOperationException("Root bone is required to target skeleton.");
            }
            // No transformation is needed to convert to the world coordinate in our case, so use identity matrix here.
            var worldMatrix = Matrix4x4.identity.ToDecimalList();
            // Debug.Log(string.Join(", ", worldMatrix));

            Stack<(Transform, Bone)> stack = new();
            Bone t2MBone = null;
            stack.Push((rootBone, null));

            while (stack.Any())
            {
                var (current, parent) = stack.Pop();

                List<decimal> matrix;
                if (boneMatrixOverrides != null && boneMatrixOverrides.TryGetValue(current.name, out List<decimal> overrides))
                {
                    matrix = overrides;
                }
                else
                {
                    matrix = current.GetLocalMatrix(isLeftHanded: false, isZup: false, scaleFactor: 1.0f).ToDecimalList();
                }

                // Remove colon in the bone name for the mapping, this is
                // because model is hardcoded to only support without colon for now.
                var remappedName = current.name.Replace(":", "");

                var bone = new Bone(
                    children: new List<Bone>(),
                    matrix: matrix,
                    name: remappedName
                );
                if (parent == null)
                {
                    t2MBone = bone;
                }
                else
                {
                    parent.Children.Add(bone);
                }

                for (int i = 0; i < current.childCount; i++)
                {
                    stack.Push((current.GetChild(i), bone));
                }
            }

            if (t2MBone == null)
            {
                throw new InvalidOperationException("Failed to generate target skeleton.");
            }

            var result = new Skeleton(
                root: t2MBone,
                worldMatrix: worldMatrix
            );
            // Debug.Log("target_skeleton:" + JsonConvert.SerializeObject(result));
            return result;
        }


        public string GetGeneratedFramesOutputDir()
        {
            return Path.Join(
                _settingsCache.FramesOutputDir,
                _gameObjectName);
        }

        public void SaveT2MFrames(T2MFrames t2MFrames, string clipName)
        {
            string generatedFramesOutputDir = GetGeneratedFramesOutputDir();
            string fileName = DateTime.Now.ToString("yyyyMMddHHmm") + "_" + clipName + ".json";
            Directory.CreateDirectory(generatedFramesOutputDir);
            var frames = T2MFrames.ToJson(t2MFrames);
            T2MFrameSaveFile saveFile = new()
            {
                Version = T2MFrames.Version,
                Content = frames,
            };
            var json = JsonConvert.SerializeObject(saveFile);
            File.WriteAllText(Path.Join(generatedFramesOutputDir, fileName), json);
        }

        public static T2MFrames LoadT2MFrames(string json)
        {
            var saveFile = JsonConvert.DeserializeObject<T2MFrameSaveFile>(json);
            if (saveFile != null &&
                saveFile.Version == T2MFrames.Version)
            {
                Debug.Log(saveFile.Content);
                return T2MFrames.FromJson(saveFile.Content);
            }
            throw new InvalidOperationException("Unsupported T2MFrameSaveFile version: " + saveFile.Version);
        }

        public static void WriteT2MToAnimationClip(
            T2MFrames t2mFrames,
            AnimationClip writeToClip,
            Dictionary<string, string> pathToBone,
            bool applyRootMotion,
            Vector3 positionOffset,
            bool isLeftHanded = false)
        {
            foreach (var track in t2mFrames.Tracks)
            {
                if (!pathToBone.ContainsKey(track.Key))
                {
                    Debug.LogError($"Bone {track.Key} not found in the model, skipping animation track.");
                    continue;
                }
                var boneRelativePath = pathToBone[track.Key];
                if (track.Value.Rotation != null)
                {
                    var xRotationKeyFrames = new List<Keyframe>();
                    var yRotationKeyFrames = new List<Keyframe>();
                    var zRotationKeyFrames = new List<Keyframe>();
                    var wRotationKeyFrames = new List<Keyframe>();
                    foreach (var rotation in track.Value.Rotation)
                    {
                        var time = float.Parse(rotation.Key, CultureInfo.InvariantCulture.NumberFormat);
                        xRotationKeyFrames.Add(
                            new Keyframe(
                                time: time,
                                value: isLeftHanded ? (float)rotation.Value.X : -(float)rotation.Value.X)
                                );
                        yRotationKeyFrames.Add(
                            new Keyframe(
                                time: time,
                                value: (float)rotation.Value.Y)
                                );
                        zRotationKeyFrames.Add(
                            new Keyframe(
                                time: time,
                                value: (float)rotation.Value.Z)
                                );
                        wRotationKeyFrames.Add(
                            new Keyframe(
                                time: time,
                                value: isLeftHanded ? (float)rotation.Value.W : -(float)rotation.Value.W)
                                );
                        // Debug.Log($"{boneRelativePath}: [{time}] x: {rotation.Value.X} y: {rotation.Value.Y} z: {rotation.Value.Z} w: {rotation.Value.W}");
                    }

                    writeToClip.SetCurve(
                        relativePath: boneRelativePath,
                        type: typeof(Transform),
                        propertyName: "localRotation.x",
                        curve: new AnimationCurve(xRotationKeyFrames.ToArray())
                    );
                    writeToClip.SetCurve(
                        relativePath: boneRelativePath,
                        type: typeof(Transform),
                        propertyName: "localRotation.y",
                        curve: new AnimationCurve(yRotationKeyFrames.ToArray())
                    );
                    writeToClip.SetCurve(
                        relativePath: boneRelativePath,
                        type: typeof(Transform),
                        propertyName: "localRotation.z",
                        curve: new AnimationCurve(zRotationKeyFrames.ToArray())
                    );
                    writeToClip.SetCurve(
                        relativePath: boneRelativePath,
                        type: typeof(Transform),
                        propertyName: "localRotation.w",
                        curve: new AnimationCurve(wRotationKeyFrames.ToArray())
                    );
                }

                if (applyRootMotion && track.Value.Position != null)
                {
                    var xPositionKeyFrames = new List<Keyframe>();
                    var yPositionKeyFrames = new List<Keyframe>();
                    var zPositionKeyFrames = new List<Keyframe>();
                    foreach (var position in track.Value.Position)
                    {
                        var time = float.Parse(position.Key, CultureInfo.InvariantCulture.NumberFormat);
                        xPositionKeyFrames.Add(
                            new Keyframe(
                                time: time,
                                value: (isLeftHanded ? (float)position.Value.X : -(float)position.Value.X) + positionOffset.x)
                                );
                        yPositionKeyFrames.Add(
                            new Keyframe(
                                time: time,
                                value: (float)position.Value.Y + positionOffset.y)
                                );
                        zPositionKeyFrames.Add(
                            new Keyframe(
                                time: time,
                                value: (float)position.Value.Z + positionOffset.z)
                                );
                        // Debug.Log($"{boneRelativePath}: [{time}] x: {position.Value.X} y: {position.Value.Y} z: {position.Value.Z}");
                    }

                    writeToClip.SetCurve(
                        relativePath: boneRelativePath,
                        type: typeof(Transform),
                        propertyName: "localPosition.x",
                        curve: new AnimationCurve(xPositionKeyFrames.ToArray())
                    );
                    writeToClip.SetCurve(
                        relativePath: boneRelativePath,
                        type: typeof(Transform),
                        propertyName: "localPosition.y",
                        curve: new AnimationCurve(yPositionKeyFrames.ToArray())
                    );
                    writeToClip.SetCurve(
                        relativePath: boneRelativePath,
                        type: typeof(Transform),
                        propertyName: "localPosition.z",
                        curve: new AnimationCurve(zPositionKeyFrames.ToArray())
                    );
                }
            }
        }
    }
}
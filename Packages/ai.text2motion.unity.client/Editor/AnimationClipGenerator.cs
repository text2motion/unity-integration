using System.Collections.Generic;
using System.IO;
using System.Linq;
using Text2Motion.Server;
using Text2Motion.Settings;
using Text2Motion.Utils;
using UnityEditor;
using UnityEngine;

namespace Text2Motion
{
    public class AnimationClipGenerator : MonoBehaviour
    {
        [Header("Animation input settings")]
        [Tooltip("Root bone of the skeleton to animate.")]
        public Transform RootBone;

        [Header("Animation output settings")]
        [Tooltip("Whether or not to move the object using the root motion from the animations.")]
        public bool applyRootMotion = true;
        [Tooltip("Position offset for the final generated animation. E.g. move the generated animation up in the Y axis to the ground level.")]
        public Vector3 positionOffset = Vector3.zero;

        [Header("Animation")]
        [Tooltip("This is the Animation component to which the clip will be added. If left empty, a new Animation component will be added to the GameObject.")]
        public Animation addToAnimation;

        [HideInInspector]
        public T2MServerRequestWrapper ServerRequestWrapper
        {
            get
            {
                if (_serverRequestWrapper == null)
                {
                    LoadServerWrapper();
                }
                return _serverRequestWrapper;
            }
        }

        private T2MServerRequestWrapper _serverRequestWrapper;

        [HideInInspector]
        public T2MSettings SettingsCache
        {
            get
            {
                if (_settingsCache == null)
                {
                    LoadSettings();
                }
                return _settingsCache;
            }
        }

        private T2MSettings _settingsCache;
        private Dictionary<string, string> pathToBone;



        void LoadServerWrapper()
        {
            _serverRequestWrapper = new T2MServerRequestWrapper(gameObject.name, SettingsCache);
            if (!ServerRequestWrapper.IsTPoseLoaded() &&
                ServerRequestWrapper.IsTPoseFileExists())
            {
                ServerRequestWrapper.LoadTPoseFromFile();
            }
        }

        public void FetchAndSaveTPose()
        {
            Debug.Log("Fetching T-Pose for: " + gameObject.name);
            var tPoseMapping = TPoseHelper.GetTPoseFromFbxModel(RootBone, gameObject);
            ServerRequestWrapper.SaveTPose(RootBone, tPoseMapping);
        }

        public void SaveCurrentPoseAsTPose()
        {
            Debug.Log("Saving current pose as T-Pose for: " + gameObject.name);
            ServerRequestWrapper.SaveTPose(RootBone);
        }

        public string GetClipPath(string clipName)
        {
            return Path.Join(_settingsCache.GeneratedAnimationDir, gameObject.name, clipName + ".anim");
        }

        public void LoadSettings()
        {
            _settingsCache = T2MSettings.GetOrCreateSettings();
        }

        public void SaveSettings()
        {
            EditorUtility.SetDirty(SettingsCache);
            AssetDatabase.SaveAssets();
        }


        private void PopulateBoneToPathMap(Transform root)
        {
            pathToBone = new Dictionary<string, string>();
            var stack = new Stack<(Transform, string)>();
            stack.Push((root, ""));

            while (stack.Any())
            {
                var (current, path) = stack.Pop();
                string currentPath = string.IsNullOrEmpty(path) ? current.name : $"{path}/{current.name}";

                // Remove colon in the bone name for the mapping, this is
                // because model is hardcoded to only support without colon for now.
                var remappedName = current.name.Replace(":", "");
                pathToBone[remappedName] = currentPath;

                for (int i = 0; i < current.childCount; i++)
                {
                    stack.Push((current.GetChild(i), currentPath));
                }
            }
        }

        public AnimationClip LoadAnimation(
            T2MFrames t2mFrames,
            string clipName = null,
            AnimationClip writeToClip = null)
        {
            bool isNewClip = writeToClip == null;
            if (isNewClip)
            {
                writeToClip = new AnimationClip
                {
                    name = clipName,
                    // Animation only support adding legacy clips
                    legacy = true
                };
            }
            else
            {
                writeToClip.ClearCurves();
            }
            PopulateBoneToPathMap(RootBone);
            T2MServerRequestWrapper.WriteT2MToAnimationClip(
                t2mFrames,
                writeToClip,
                pathToBone,
                applyRootMotion,
                positionOffset
            );

            if (addToAnimation == null)
            {
                addToAnimation = gameObject.GetComponent<Animation>();
                if (addToAnimation == null)
                {
                    addToAnimation = gameObject.AddComponent<Animation>();
                }
            }
            addToAnimation.AddClip(writeToClip, writeToClip.name);
            // After the clip was added, need to set legacy to false so it can play in Animation panel
            writeToClip.legacy = false;

            addToAnimation.clip = writeToClip;

            if (isNewClip)
            {
                var clipPath = Path.Join(
                    SettingsCache.GeneratedAnimationDir,
                    gameObject.name,
                    writeToClip.name + ".anim");
                Directory.CreateDirectory(Path.GetDirectoryName(clipPath));
                AssetDatabase.CreateAsset(writeToClip, clipPath);
                AssetDatabase.SaveAssets();
            }

            return writeToClip;
        }
    }
}
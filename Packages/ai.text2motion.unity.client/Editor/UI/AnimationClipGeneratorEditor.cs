using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Text2Motion.Server;
using Text2Motion.Settings;
using Text2MotionClientAPI.Client;
using UnityEditor;
using UnityEngine;

namespace Text2Motion.UI
{
    [CustomEditor(typeof(AnimationClipGenerator))]
    public class AnimationClipGeneratorEditor : Editor
    {
        string _apiKeyToEdit = "";
        string _prompt = "Walk around the room";
        string _clipName = "";
        UnityEngine.Object _clipToReplace;
        bool _showAdditionalParameters = false;
        int _generateAnimationTabSelector;
        string _generatedFramesFilePath = "";
        string t2mGeneratedFramesJson = "";
        AnimationWindow _animationWindow;

        static private int clipCount = 0;

        enum GenerateAnimationTab
        {
            New,
            Replace,
            Load,
            Options,
        }

        public override void OnInspectorGUI()
        {
            AnimationClipGenerator animationClipGenerator = (AnimationClipGenerator)target;
            serializedObject.Update();

            if (string.IsNullOrWhiteSpace(animationClipGenerator.SettingsCache.APIKey))
            {
                DrawAPIKeyConfigurator(animationClipGenerator);
            }
            else if (!animationClipGenerator.ServerRequestWrapper.IsTPoseLoaded())
            {
                DrawTPoseInitializer(animationClipGenerator);
            }
            else
            {
                DrawAnimationClipGenerator(animationClipGenerator);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAPIKeyConfigurator(AnimationClipGenerator animationClipGenerator)
        {
            EditorGUILayout.LabelField("Login", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("API Key is not set. Please obtain an API key from the developer portal by creating an App.", MessageType.Warning);
            if (EditorGUILayout.LinkButton("Open Developer Portal (open browser)"))
            {
                Application.OpenURL("https://developer.text2motion.ai/");
            }
            _apiKeyToEdit = EditorGUILayout.PasswordField("ApiKey:", _apiKeyToEdit);

            if (!string.IsNullOrWhiteSpace(_apiKeyToEdit))
            {
                if (GUILayout.Button("Save API Key to Settings"))
                {
                    animationClipGenerator.SettingsCache.APIKey = _apiKeyToEdit;
                    animationClipGenerator.SaveSettings();
                }
            }
        }

        private void DrawTPoseInitializer(AnimationClipGenerator animationClipGenerator)
        {
            EditorGUILayout.LabelField("Initialize T-Pose", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("T-Pose must be loaded first before animation can be generated.", MessageType.Warning);

            SerializedProperty sizeProp = serializedObject.FindProperty("RootBone");
            EditorGUILayout.PropertyField(sizeProp, true);

            if (animationClipGenerator.RootBone == null)
            {
                EditorGUILayout.HelpBox("Please set the Root Bone of the model. E.g. Drag \"Hips\" or whatever the root bone is from th Hierarchy window to None (Transform) above.", MessageType.Warning);
            }

            using (new EditorGUI.DisabledScope(animationClipGenerator.RootBone == null))
            {

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Initialize T-Pose from FBX asset"))
                {
                    if (EditorUtility.DisplayDialog("Load T-Pose?",
                                                    "This will attempt to load T-Pose for the current model. " +
                                                    "This assumes the current model is a Prefab for an imported FBX Model. " +
                                                    "If the Animation Type isn't Humanoid, it will attempts to convert it " +
                                                    "to Humanoid so it can fetch the t-pose, then it'll grab the t-pose " +
                                                    "and convert it back.",
                                                    "Load",
                                                    "Do Not Load"))
                    {
                        animationClipGenerator.FetchAndSaveTPose();
                    }
                }
                using (new EditorGUI.DisabledScope(!animationClipGenerator.ServerRequestWrapper.IsTPoseFileExists()))
                {
                    if (GUILayout.Button("Load Cached T-Pose"))
                    {
                        animationClipGenerator.ServerRequestWrapper.LoadTPoseFromFile();
                    }
                }
                GUILayout.EndHorizontal();
                // This is potentially useful for GLB format, but getting GLB to work is more complicated than 
                // just this function alone. We'll leave it out for now.
                // if (GUILayout.Button("Save current pose as T-Pose"))
                // {
                //     if (EditorUtility.DisplayDialog("Save Pose?",
                //                                     "This will use the model's current pose as the t-pose. " +
                //                                     "This assumes the model is already in t-pose. " +
                //                                     "If the model isn't in t-pose, the animation result will be broken. " +
                //                                     "Use this option if your model isn't in FBX format and initialize T-Pose " +
                //                                     "doesn't work.",
                //                                     "Save",
                //                                     "Do Not Save"))
                //     {
                //         animationClipGenerator.SaveCurrentPoseAsTPose();
                //     }
                // }
            }
        }

        private void DrawAnimationClipGenerator(AnimationClipGenerator animationClipGenerator)
        {
            _generateAnimationTabSelector = GUILayout.Toolbar(_generateAnimationTabSelector, Enum.GetNames(typeof(GenerateAnimationTab)));
            switch (_generateAnimationTabSelector)
            {
                case (int)GenerateAnimationTab.New:
                    ShowLicenseLink();
                    EditorGUILayout.LabelField("Create New Animation Clip", EditorStyles.boldLabel);
                    DrawGenerateAnimationTab(animationClipGenerator, true);
                    break;
                case (int)GenerateAnimationTab.Replace:
                    ShowLicenseLink();
                    EditorGUILayout.LabelField("Override Existing Animation Clip", EditorStyles.boldLabel);
                    DrawGenerateAnimationTab(animationClipGenerator, false);
                    break;
                case (int)GenerateAnimationTab.Load:
                    ShowLicenseLink();
                    EditorGUILayout.LabelField("Load Previously Generated Animation Clip", EditorStyles.boldLabel);
                    DrawLoadAnimationTab(animationClipGenerator);
                    break;
                case (int)GenerateAnimationTab.Options:
                    DrawAdvancedOptionsTab(animationClipGenerator);
                    break;
            }
        }

        private bool CheckGenerateAnimationPrecondition(
            AnimationClipGenerator animationClipGenerator,
            bool shouldCreateNewClip,
            bool shouldLoadExistingFrames)
        {
            bool isGenerateConditionMet = true;
            if (animationClipGenerator.RootBone == null)
            {
                EditorGUILayout.HelpBox("Please set the Root Bone of the model. E.g. Drag \"Hips\" or whatever the root bone is from th Hierarchy window to None (Transform) above.", MessageType.Warning);
                isGenerateConditionMet = false;
            }
            if (!shouldLoadExistingFrames)
            {
                _prompt = EditorGUILayout.TextField(
                    new GUIContent("Prompt", "Text prompt for the animation to generate."),
                    _prompt);

                if (string.IsNullOrWhiteSpace(_prompt))
                {
                    EditorGUILayout.HelpBox("Please enter a prompt for the animation to generate.", MessageType.Warning);
                    isGenerateConditionMet = false;
                }

            }
            if (shouldCreateNewClip)
            {
                _clipName = EditorGUILayout.TextField(
                    new GUIContent("Clip Name", "Name of the animation clip to create."),
                    _clipName);

                if (!string.IsNullOrWhiteSpace(_clipName) &&
                    File.Exists(animationClipGenerator.GetClipPath(_clipName)))
                {
                    EditorGUILayout.HelpBox("Clip with the same name already exists. Please choose a different name.", MessageType.Warning);
                    isGenerateConditionMet = false;
                }
            }
            else
            {
                _clipToReplace = EditorGUILayout.ObjectField(
                    new GUIContent("Clip to replace", "Existing clip to clear and load generated animation on it."),
                    _clipToReplace,
                    typeof(AnimationClip),
                    true);
                if (_clipToReplace == null)
                {
                    EditorGUILayout.HelpBox("Please select an existing clip to replace.", MessageType.Warning);
                    isGenerateConditionMet = false;
                }
            }
            return isGenerateConditionMet;

        }

        private async Task ShowServerRequestProgressBarAsync(CancellationToken cancellationToken)
        {
            var stepSeconds = 1;
            var maxWaitTimeSeconds = 30;
            var averageWaitTimeSeconds = 10;
            bool isCancelled;
            for (int t = 0; t < maxWaitTimeSeconds; t += stepSeconds)
            {
                var statusText = "Prompt: " + _prompt;
                var progress = 1.0f * t / averageWaitTimeSeconds;
                if (t > averageWaitTimeSeconds)
                {
                    statusText += "\n\nServer request is taking longer than usual. Please be patient...";
                    progress = 1.0f * t / maxWaitTimeSeconds;
                }
                else
                {
                    statusText += "\n\nWaiting for server response...";
                }
                isCancelled = EditorUtility.DisplayCancelableProgressBar("Text2Motion Animation generation", statusText, progress);

                if (isCancelled ||
                    cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                await Task.Delay((int)(stepSeconds * 1000.0f));
            }

            EditorUtility.ClearProgressBar();
        }

        private void SetDefaultClipNameIfEmpty(AnimationClipGenerator animationClipGenerator)
        {
            if (string.IsNullOrWhiteSpace(_clipName))
            {
                var tmpClipName = "T2MGeneratedClip(" + clipCount++ + ")";
                while (File.Exists(animationClipGenerator.GetClipPath(tmpClipName)))
                {
                    tmpClipName = "T2MGeneratedClip(" + clipCount++ + ")";
                }
                _clipName = tmpClipName;
            }
        }

        private void HandleHTTPResponseError(
            ApiException e,
            string prompt)
        {
            Debug.LogError("Failed to generate request for prompt: " + prompt +
            "\nMessage: " + e.Message + " ErrorContent: " + e.ErrorContent);

            switch (e.ErrorCode)
            {
                case (int)HttpStatusCode.BadRequest:
                case (int)HttpStatusCode.NotFound:
                case (int)HttpStatusCode.MethodNotAllowed:
                case (int)HttpStatusCode.Conflict:
                case (int)HttpStatusCode.UnprocessableEntity:
                case (int)HttpStatusCode.UnsupportedMediaType:
                    Debug.LogError("Text2Motion request failed due to issue on the client.");
                    EditorUtility.DisplayDialog("Text2Motion Client Request Error",
                    "Failed to generate request due to client error." +
                    "\nprompt: " + prompt +
                    "\n\n" + e.Message +
                    "\n\n Please contact support@text2motion.ai for help.",
                    "OK");
                    break;
                case (int)HttpStatusCode.UpgradeRequired:
                    Debug.LogError("Text2Motion request failed due to outdated package.");
                    EditorUtility.DisplayDialog("Text2Motion Client Request Error",
                    "Package update required. Please go to \"Window > Package Manager\" and update the Text2Motion package.",
                    "OK");
                    break;
                case (int)HttpStatusCode.Unauthorized:
                case (int)HttpStatusCode.Forbidden:
                    Debug.LogError("Text2Motion request failed due to auth issue.");
                    EditorUtility.DisplayDialog("Text2Motion Client Request Error",
                    "Invalid API Key. Please got to \"Options > Clear API Key\" to reconfigure it.",
                    "OK");
                    break;
                case (int)HttpStatusCode.TooManyRequests:
                    Debug.LogWarning("Text2Motion request failed due to throttling.");
                    EditorUtility.DisplayDialog("Text2Motion Client Request Error",
                    "Request Throttled. Please wait for some time and try again later.",
                    "OK");
                    break;
                case (int)HttpStatusCode.InternalServerError:
                case (int)HttpStatusCode.BadGateway:
                case (int)HttpStatusCode.GatewayTimeout:
                    Debug.LogError("Text2Motion request failed due to server issue.");
                    EditorUtility.DisplayDialog("Text2Motion Server Request Error",
                    "Server has encountered an error. Please try again later.",
                    "OK");
                    break;
                case (int)HttpStatusCode.ServiceUnavailable:
                    Debug.LogError("Text2Motion server is doing an maintenance now. Message: " + e.Message);
                    EditorUtility.DisplayDialog("Text2Motion Server Request Error",
                    "Server has encountered an error. Please try again later." +
                    "\n" + e.ErrorContent.ToString(),
                    "OK");
                    break;
                default:
                    Debug.LogError("Text2Motion request failed due to unknown.");
                    EditorUtility.DisplayDialog("Text2Motion Request Error",
                    "Request failed. Please try again.",
                    "OK");
                    break;
            }
        }

        private void ShowLicenseLink()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("All generated animations are licensed under", new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleRight,
            });
            if (EditorGUILayout.LinkButton("CC-by-4.0"))
            {
                Application.OpenURL("https://creativecommons.org/licenses/by/4.0/");
            }
            GUILayout.EndHorizontal();
        }

        private async void DrawGenerateAnimationTab(AnimationClipGenerator animationClipGenerator, bool shouldCreateNewClip, string injectT2MFrames = "")
        {
            bool shouldLoadExistingFrames = !string.IsNullOrWhiteSpace(injectT2MFrames);
            bool isGenerateConditionMet = CheckGenerateAnimationPrecondition(
                animationClipGenerator,
                shouldCreateNewClip,
                shouldLoadExistingFrames);

            _showAdditionalParameters = EditorGUILayout.Foldout(_showAdditionalParameters, "Advanced Parameters");
            if (_showAdditionalParameters)
            {
                DrawPropertiesExcluding(serializedObject, "m_Script");
            }

            using (new EditorGUI.DisabledScope(!isGenerateConditionMet))
            {
                var buttonName = shouldLoadExistingFrames ? "Load Animation" : "Generate Animation";
                if (GUILayout.Button(buttonName))
                {
                    SetDefaultClipNameIfEmpty(animationClipGenerator);

                    T2MFrames t2mFrames;
                    if (!shouldLoadExistingFrames)
                    {
                        var tokenSource = new CancellationTokenSource();
                        CancellationToken cancellationToken = tokenSource.Token;
                        var serverRequestTask = animationClipGenerator.ServerRequestWrapper.GenerateRequestAsync(
                            prompt: _prompt,
                            requestName: _clipName,
                            cancellationToken: cancellationToken).ContinueWith(
                            (task) =>
                            {
                                tokenSource.Cancel();
                                return task.Result;
                            }, cancellationToken
                        );

                        await ShowServerRequestProgressBarAsync(cancellationToken);
                        tokenSource.Cancel();

                        try
                        {
                            t2mFrames = await serverRequestTask;
                        }
                        catch (AggregateException e)
                        {
                            foreach (var ex in e.Flatten().InnerExceptions)
                            {
                                if (ex is ApiException)
                                {
                                    HandleHTTPResponseError(ex as ApiException, _prompt);
                                    return;
                                }
                            }
                            Debug.LogError("Failed to generate request for prompt: " + _prompt + "\n" + e.Message);
                            EditorUtility.DisplayDialog("Error",
                                "Failed to generate request for prompt: " + _prompt + "\n" + e.Message,
                                "OK");
                            return;
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Failed to generate request for prompt: " + _prompt + "\n" + e.Message);
                            EditorUtility.DisplayDialog("Error",
                                "Failed to generate request for prompt: " + _prompt + "\n" + e.Message,
                                "OK");
                            return;
                        }
                    }
                    else
                    {
                        t2mFrames = T2MServerRequestWrapper.LoadT2MFrames(injectT2MFrames);
                    }

                    AnimationClip result;
                    if (shouldCreateNewClip)
                    {
                        result = animationClipGenerator.LoadAnimation(t2mFrames, _clipName);
                    }
                    else
                    {
                        result = animationClipGenerator.LoadAnimation(t2mFrames, writeToClip: _clipToReplace as AnimationClip);
                    }

                    Debug.Log("Animation generated for prompt: \"" + _prompt + "\" and saved to: " + AssetDatabase.GetAssetPath(result));
                    FocusAnimationWindowsOnClip(result, animationClipGenerator);
                }
            }
        }

        private void FocusAnimationWindowsOnClip(
            AnimationClip cliToFocus,
            AnimationClipGenerator animationClipGenerator
            )
        {
            if (animationClipGenerator.SettingsCache.AutoFocusAnimationPanelOnResult)
            {
                if (_animationWindow == null)
                {
                    _animationWindow = EditorWindow.GetWindow<AnimationWindow>();
                }

                _animationWindow.animationClip = cliToFocus;
                if (animationClipGenerator.SettingsCache.AutoPlayAnimationOnResult)
                {
                    _animationWindow.playing = true;
                }
            }
        }

        private void DrawLoadAnimationTab(AnimationClipGenerator animationClipGenerator)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                _generatedFramesFilePath = EditorGUILayout.TextField(
                _generatedFramesFilePath, new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleRight,
                });
            }

            if (GUILayout.Button("Select Generated Frames"))
            {
                string path = EditorUtility.OpenFilePanel(
                    "Generated Frames to load",
                    animationClipGenerator.ServerRequestWrapper.GetGeneratedFramesOutputDir(),
                    "json");
                if (path.Length != 0)
                {
                    _generatedFramesFilePath = path;
                }
            }
            using (new EditorGUI.DisabledScope(string.IsNullOrWhiteSpace(_generatedFramesFilePath)))
            {
                if (GUILayout.Button("Load Frames"))
                {
                    t2mGeneratedFramesJson = File.ReadAllText(_generatedFramesFilePath);
                }
            }

            if (!string.IsNullOrWhiteSpace(t2mGeneratedFramesJson))
            {
                DrawGenerateAnimationTab(animationClipGenerator, false, t2mGeneratedFramesJson);
            }
        }

        private void DrawAdvancedOptionsTab(AnimationClipGenerator animationClipGenerator)
        {
            if (GUILayout.Button("Open Settings"))
            {
                SettingsService.OpenProjectSettings(T2MSettingsProvider.SettingItemPath);
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reload Settings"))
            {
                animationClipGenerator.LoadSettings();
                Debug.Log("Settings reloaded.");
            }
            if (GUILayout.Button("Clear T-Pose"))
            {
                if (EditorUtility.DisplayDialog("Clear T-Pose?",
                                                "Are you sure you want to clear the T-Pose used for animation generation? " +
                                                "This will need to be loaded again to generate animation. \n\n" +
                                                "Use this option for resetting the t-pose of animation when necessary.",
                                                "Clear",
                                                "Do Not Clear"))
                {
                    animationClipGenerator.ServerRequestWrapper.ClearTPose();
                }
            }

            if (GUILayout.Button("Clear API Key"))
            {
                if (EditorUtility.DisplayDialog("Clear API Key?",
                                                "Are you sure you want to clear the API Key? You will have to fetch it " +
                                                "from the developer portal again to generate animation. \n\n" +
                                                "Use this option for resetting the API Key for server request when necessary.",
                                                "Clear",
                                                "Do Not Clear"))
                {
                    _apiKeyToEdit = "";
                    animationClipGenerator.SettingsCache.APIKey = "";
                    animationClipGenerator.SaveSettings();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }
    }
}
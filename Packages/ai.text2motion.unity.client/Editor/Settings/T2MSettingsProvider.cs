
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Text2Motion.Settings
{
    class T2MSettingsProvider : SettingsProvider
    {
        public const string SettingItemPath = "Project/Text2Motion";
        private SerializedObject _settings;
        bool _showAdvancedSettings = true;

        public T2MSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public static bool IsSettingsAvailable()
        {
            return File.Exists(T2MSettings.SettingsPath);
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the T2M element in the Settings window.
            _settings = T2MSettings.GetSerializedSettings();
        }

        public override void OnGUI(string searchContext)
        {
            var apiKeyLabel = new GUIContent("API Key", "API key used for making request to Text2Motion API server.");
            // Use IMGUI to display UI:
            if (_settings.FindProperty("HideAPIKey").boolValue)
            {
                _settings.FindProperty("APIKey").stringValue = EditorGUILayout.PasswordField(
                    apiKeyLabel,
                    _settings.FindProperty("APIKey").stringValue);
            }
            else
            {
                EditorGUILayout.PropertyField(
                    _settings.FindProperty("APIKey"),
                    apiKeyLabel
                    );
            }
            EditorGUILayout.PropertyField(_settings.FindProperty("HideAPIKey"), new GUIContent("Hide API Key", "Whether or not API Key field should be display as plain text."));


            _showAdvancedSettings = EditorGUILayout.Foldout(_showAdvancedSettings, "Advanced Settings");
            if (_showAdvancedSettings)
            {
                EditorGUILayout.PropertyField(
                    _settings.FindProperty("AutoFocusAnimationPanelOnResult"),
                    new GUIContent("Auto Focus Animation", "Whether or not to automatically focus on Animation Panel when generation result comes back."));


                using (new EditorGUI.DisabledScope(!_settings.FindProperty("AutoFocusAnimationPanelOnResult").boolValue))
                {
                    EditorGUILayout.PropertyField(
                        _settings.FindProperty("AutoPlayAnimationOnResult"),
                        new GUIContent(
                            "Auto Play Animation",
                            "If focused on the Animation Panel on result, whether or not to auto-play the animation"));
                }

                EditorGUILayout.PropertyField(
                    _settings.FindProperty("TPoseDataDir"),
                    new GUIContent("T-Pose Data File Path", "Directory to store the T-Pose data."));
                if (string.IsNullOrWhiteSpace(_settings.FindProperty("TPoseDataDir").stringValue))
                {
                    _settings.FindProperty("TPoseDataDir").stringValue = T2MSettings.Default.TPoseDataDir;
                }
                EditorGUILayout.PropertyField(
                    _settings.FindProperty("TPoseDataName"),
                    new GUIContent("T-Pose Data File Name", "Name of the file to save for the T-Pose data."));
                if (string.IsNullOrWhiteSpace(_settings.FindProperty("TPoseDataName").stringValue))
                {
                    _settings.FindProperty("TPoseDataName").stringValue = T2MSettings.Default.TPoseDataName;
                }
                EditorGUILayout.PropertyField(
                    _settings.FindProperty("FramesOutputDir"),
                    new GUIContent(
                        "Generated Raw Animation Frames",
                        "Directory to store the generated raw animation frame from Text2Motion."));
                if (string.IsNullOrWhiteSpace(_settings.FindProperty("FramesOutputDir").stringValue))
                {
                    _settings.FindProperty("FramesOutputDir").stringValue = T2MSettings.Default.FramesOutputDir;
                }
                EditorGUILayout.PropertyField(
                    _settings.FindProperty("GeneratedAnimationDir"),
                    new GUIContent("Generated Animation Path", "Directory to store the generated unity animation clips."));
                if (string.IsNullOrWhiteSpace(_settings.FindProperty("GeneratedAnimationDir").stringValue))
                {
                    _settings.FindProperty("GeneratedAnimationDir").stringValue = T2MSettings.Default.GeneratedAnimationDir;
                }
                EditorGUILayout.LabelField("Clear the content to reset to default values.", EditorStyles.miniLabel);
            }

            _settings.ApplyModifiedPropertiesWithoutUndo();
        }

        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider CreateT2MSettingsProvider()
        {
            var provider = new T2MSettingsProvider(SettingItemPath, SettingsScope.Project);
            return provider;
        }
    }
}
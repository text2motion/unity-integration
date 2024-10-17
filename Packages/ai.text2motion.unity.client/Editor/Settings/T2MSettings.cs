using UnityEditor;
using UnityEngine;
using System.IO;

namespace Text2Motion.Settings
{
    public class T2MSettings : ScriptableObject
    {
        public const string SettingsPath = "Assets/Resources/T2MSettings.asset";

        public static class Default
        {
            public static readonly string GeneratedDir = Path.Join("Assets", "T2MGenerated");
            public static readonly string TempDir = Path.Join(GeneratedDir, ".tmp");
            public static readonly string TPoseDataDir = Path.Join(TempDir, "Data");
            public const string TPoseDataName = "t-pose.json";
            public static readonly string FramesOutputDir = Path.Join(TempDir, "Frames");
            public static readonly string GeneratedAnimationDir = Path.Join(GeneratedDir, "Animations");
        }

        public string APIKey = "";

        public bool HideAPIKey = true;    // so one can hide their key if they wanted to stream or create a video of their work

        public string TPoseDataDir = Default.TPoseDataDir;

        public string TPoseDataName = Default.TPoseDataName;

        public string FramesOutputDir = Default.FramesOutputDir;

        public string GeneratedAnimationDir = Default.GeneratedAnimationDir;

        public bool AutoFocusAnimationPanelOnResult = true;
        public bool AutoPlayAnimationOnResult = true;

        internal static T2MSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<T2MSettings>(SettingsPath);
            if (settings == null)
            {
                settings = CreateInstance<T2MSettings>();
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
                AssetDatabase.CreateAsset(settings, SettingsPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }


}
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveSystemModuleSelector : EditorWindow
{
    private static string manifestPath = Path.Combine(Directory.GetCurrentDirectory(), "Packages/manifest.json");
    
    private static string jsonModuleURL = "https://github.com/JoshuaLetessier/UnitySaveSystem.git?path=/Packages/com.faolline.savesystem.json";
    private static string prefsModuleURL = "https://github.com/JoshuaLetessier/UnitySaveSystem.git?path=/Packages/com.faolline.savesystem.playerprefs";

    private bool useJson;
    private bool usePlayerPrefs;

    [MenuItem("Window/Save System Modules")]
    public static void ShowWindow()
    {
        GetWindow<SaveSystemModuleSelector>("Save System Modules");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Save System Modules", EditorStyles.boldLabel);
        useJson = EditorGUILayout.Toggle("JSON Save System", useJson);
        usePlayerPrefs = EditorGUILayout.Toggle("PlayerPrefs Save System", usePlayerPrefs);

        if (GUILayout.Button("Apply Changes"))
        {
            UpdateManifest();
        }
    }

    private void UpdateManifest()
    {
        if (!File.Exists(manifestPath))
        {
            Debug.LogError("manifest.json not found! Make sure this is a Unity project.");
            return;
        }

        string manifestContent = File.ReadAllText(manifestPath);

        if (useJson && !manifestContent.Contains(jsonModuleURL))
        {
            manifestContent = manifestContent.Replace("\"dependencies\": {", $"\"dependencies\": {{\n    \"com.faolline.savesystem.json\": \"{jsonModuleURL}\",");
        }
        else if (!useJson && manifestContent.Contains(jsonModuleURL))
        {
            manifestContent = manifestContent.Replace($"\"com.faolline.savesystem.json\": \"{jsonModuleURL}\",", "");
        }

        if (usePlayerPrefs && !manifestContent.Contains(prefsModuleURL))
        {
            manifestContent = manifestContent.Replace("\"dependencies\": {", $"\"dependencies\": {{\n    \"com.faolline.savesystem.playerprefs\": \"{prefsModuleURL}\",");
        }
        else if (!usePlayerPrefs && manifestContent.Contains(prefsModuleURL))
        {
            manifestContent = manifestContent.Replace($"\"com.faolline.savesystem.playerprefs\": \"{prefsModuleURL}\",", "");
        }

        File.WriteAllText(manifestPath, manifestContent);
        Debug.Log("✅ Package manifest updated. Restart Unity to apply changes.");
    }
}

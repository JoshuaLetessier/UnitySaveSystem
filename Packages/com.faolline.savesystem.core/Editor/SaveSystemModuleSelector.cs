using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using Newtonsoft.Json;

public class SaveSystemModuleSelector : EditorWindow
{
    private static string manifestPath = Path.Combine(Directory.GetCurrentDirectory(), "Packages/manifest.json");
    private static string moduleConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "Packages/com.faolline.savesystem/Editor/SaveSystemModules.json");

    private Dictionary<string, string> moduleUrls = new Dictionary<string, string>();
    private Dictionary<string, bool> moduleToggles = new Dictionary<string, bool>();

    private ListRequest listRequest;
    private bool isCheckingPackages = true;

    [MenuItem("Window/Save System Modules")]
    public static void ShowWindow()
    {
        GetWindow<SaveSystemModuleSelector>("Save System Modules");
    }

    private void OnEnable()
    {
        LoadModulesFromJson();
        listRequest = Client.List();
        isCheckingPackages = true;
    }

    private void LoadModulesFromJson()
    {
        if (!File.Exists(moduleConfigPath))
        {
            Debug.LogError($"Module config file not found at {moduleConfigPath}");
            return;
        }

        string jsonContent = File.ReadAllText(moduleConfigPath);
        ModuleList moduleList = JsonConvert.DeserializeObject<ModuleList>(jsonContent);

        foreach (var module in moduleList.modules)
        {
            moduleUrls[module.package] = module.url;
            moduleToggles[module.package] = false;
        }
    }

    private void OnGUI()
    {
        GUILayout.Space(5);
        GUILayout.Label("🔧 Save System Modules", EditorStyles.boldLabel);
        GUILayout.Space(5);
        
        if (isCheckingPackages)
        {
            EditorGUILayout.HelpBox("Checking installed packages...", MessageType.Info);
            return;
        }

        EditorGUILayout.BeginVertical("box"); // Ajoute une boîte autour des modules pour améliorer la visibilité
        
        var keys = new List<string>(moduleToggles.Keys); // 🔥 Stocke les clés avant l'itération

        for (int i = 0; i < keys.Count; i++)
        {
            string module = keys[i];
            moduleToggles[module] = EditorGUILayout.ToggleLeft(ObjectNames.NicifyVariableName(module), moduleToggles[module]);
        }

        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        GUI.enabled = true; // Désactiver le bouton si aucun changement
        if (GUILayout.Button("✅ Apply Changes", GUILayout.Height(30)))
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

        foreach (var module in moduleToggles.Keys)
        {
            string packageUrl = moduleUrls[module];

            if (moduleToggles[module] && !manifestContent.Contains(packageUrl))
            {
                manifestContent = manifestContent.Replace("\"dependencies\": {", $"\"dependencies\": {{\n    \"{module}\": \"{packageUrl}\",");
            }
            else if (!moduleToggles[module] && manifestContent.Contains(packageUrl))
            {
                manifestContent = manifestContent.Replace($"\"{module}\": \"{packageUrl}\",", "");
            }
        }

        File.WriteAllText(manifestPath, manifestContent);
        Debug.Log("✅ Package manifest updated. Refreshing UPM...");
        Client.Resolve();
    }

    private void Update()
    {
        if (listRequest != null && listRequest.IsCompleted && isCheckingPackages)
        {
            isCheckingPackages = false;
            foreach (var package in listRequest.Result)
            {
                if (moduleToggles.ContainsKey(package.name))
                {
                    moduleToggles[package.name] = true;
                }
            }
            Repaint();
        }
    }

    [System.Serializable]
    private class ModuleList
    {
        public List<Module> modules;
    }

    [System.Serializable]
    private class Module
    {
        public string name;
        public string package;
        public string url;
    }
}

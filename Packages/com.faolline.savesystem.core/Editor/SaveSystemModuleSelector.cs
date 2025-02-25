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

    private Dictionary<string, string> moduleUrls = new Dictionary<string, string>(); // Stocke les URLs des modules
    private Dictionary<string, bool> moduleToggles = new Dictionary<string, bool>(); // Stocke les états des cases à cocher

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
            moduleToggles[module.package] = false; // Initialisation (sera mis à jour après la vérification des packages installés)
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Save System Modules", EditorStyles.boldLabel);

        if (isCheckingPackages)
        {
            GUILayout.Label("Checking installed packages...");
            return;
        }

        // Génère dynamiquement l'UI des modules
        foreach (var module in moduleToggles.Keys)
        {
            moduleToggles[module] = EditorGUILayout.Toggle(module, moduleToggles[module]);
        }

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

    // Classe pour stocker les modules à partir du JSON
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

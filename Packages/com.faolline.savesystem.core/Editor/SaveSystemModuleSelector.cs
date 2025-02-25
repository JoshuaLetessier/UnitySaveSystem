using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Collections.Generic;

public class SaveSystemModuleSelector : EditorWindow
{
    private static string manifestPath = Path.Combine(Directory.GetCurrentDirectory(), "Packages/manifest.json");

    private static string jsonModuleURL = "https://github.com/JoshuaLetessier/UnitySaveSystem.git?path=/Packages/com.faolline.savesystem.json";
    private static string prefsModuleURL = "https://github.com/JoshuaLetessier/UnitySaveSystem.git?path=/Packages/com.faolline.savesystem.playerprefs";

    private bool useJson;
    private bool usePlayerPrefs;

    private ListRequest listRequest;
    private bool isCheckingPackages = true;

    [MenuItem("Window/Save System Modules")]
    public static void ShowWindow()
    {
        GetWindow<SaveSystemModuleSelector>("Save System Modules");
    }

    private void OnEnable()
    {
        // 🔄 Démarre une requête pour lister les packages installés
        listRequest = Client.List();
        isCheckingPackages = true;
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Save System Modules", EditorStyles.boldLabel);

        if (isCheckingPackages)
        {
            GUILayout.Label("Checking installed packages...");
            return;
        }

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

        // 📥 Ajoute ou supprime le module JSON
        if (useJson && !manifestContent.Contains(jsonModuleURL))
        {
            manifestContent = manifestContent.Replace("\"dependencies\": {", $"\"dependencies\": {{\n    \"com.faolline.savesystem.json\": \"{jsonModuleURL}\",");
        }
        else if (!useJson && manifestContent.Contains(jsonModuleURL))
        {
            manifestContent = manifestContent.Replace($"\"com.faolline.savesystem.json\": \"{jsonModuleURL}\",", "");
        }

        // 📥 Ajoute ou supprime le module PlayerPrefs
        if (usePlayerPrefs && !manifestContent.Contains(prefsModuleURL))
        {
            manifestContent = manifestContent.Replace("\"dependencies\": {", $"\"dependencies\": {{\n    \"com.faolline.savesystem.playerprefs\": \"{prefsModuleURL}\",");
        }
        else if (!usePlayerPrefs && manifestContent.Contains(prefsModuleURL))
        {
            manifestContent = manifestContent.Replace($"\"com.faolline.savesystem.playerprefs\": \"{prefsModuleURL}\",", "");
        }

        File.WriteAllText(manifestPath, manifestContent);
        Debug.Log("✅ Package manifest updated. Refreshing UPM...");

        // 🔄 Rafraîchit Unity Package Manager immédiatement
        Client.Resolve();
    }

    private void Update()
    {
        // 📋 Vérifie la liste des packages installés et met à jour les cases à cocher
        if (listRequest != null && listRequest.IsCompleted && isCheckingPackages)
        {
            isCheckingPackages = false;
            foreach (var package in listRequest.Result)
            {
                if (package.packageId.Contains("com.faolline.savesystem.json"))
                {
                    useJson = true;
                }
                if (package.packageId.Contains("com.faolline.savesystem.playerprefs"))
                {
                    usePlayerPrefs = true;
                }
            }
            Repaint(); // 🔄 Met à jour l'affichage
        }
    }
}

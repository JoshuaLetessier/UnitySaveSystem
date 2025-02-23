using UnityEngine;
using SaveSystem;

/* 
 * Classe de test pour la sauvegarde
 */
public class SaveTest : MonoBehaviour
{
    private SaveManager<GameSettings> settingsManager;
    private SaveManager<PlayerProgress> progressManager;

    void Start()
    {
        // 📌 Création de managers distincts
        settingsManager = new SaveManager<GameSettings>();
        progressManager = new SaveManager<PlayerProgress>();

        // 📌 Enregistrement des méthodes de sauvegarde
        settingsManager.Register(new PlayerPrefsSaveSystem<GameSettings>());  // PlayerPrefs pour les settings
        progressManager.Register(new JsonSaveSystem<PlayerProgress>());       // JSON pour la progression

        // ➤ Création des données à sauvegarder
        GameSettings settings = new GameSettings { Volume = 80.0f, Fullscreen = true };
        PlayerProgress progress = new PlayerProgress { Level = 5, Score = 12345 };

        // ➤ Sauvegarde distincte
        settingsManager.Save("game_settings", settings);
        progressManager.Save("player_progress", progress);
        Debug.Log("Paramètres sauvegardés en PlayerPrefs et Progression en JSON.");

        // ➤ Chargement et affichage
        if (settingsManager.Exists("game_settings"))
        {
            GameSettings loadedSettings = settingsManager.Load("game_settings")[typeof(PlayerPrefsSaveSystem<GameSettings>)];
            Debug.Log($"[PlayerPrefs] Chargé: Volume={loadedSettings.Volume}, Fullscreen={loadedSettings.Fullscreen}");
        }

        if (progressManager.Exists("player_progress"))
        {
            PlayerProgress loadedProgress = progressManager.Load("player_progress")[typeof(JsonSaveSystem<PlayerProgress>)];
            Debug.Log($"[JSON] Chargé: Niveau={loadedProgress.Level}, Score={loadedProgress.Score}");
        }
    }
}

[System.Serializable]
public class GameSettings
{
    public float Volume;
    public bool Fullscreen;
}

[System.Serializable]
public class PlayerProgress
{
    public int Level;
    public int Score;
}
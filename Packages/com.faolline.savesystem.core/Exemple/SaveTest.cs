using SaveSystem;
using SaveSystem.SSJson;
using SaveSystem.SSPlayerPrefs;
using UnityEngine;



public class SaveSystemTestMono : MonoBehaviour
{
    private SaveManager<TestData> saveManager;
    private string jsonKey = "test_json";
    private string prefsKey = "test_prefs";

    void Start()
    {
        Debug.Log("🚀 Running Save System Tests...");

        // Initialisation du SaveManager
        saveManager = new SaveManager<TestData>();

        // Ajouter les systèmes de sauvegarde
        saveManager.Register(new JsonSaveSystem<TestData>());
        saveManager.Register(new PlayerPrefsSaveSystem<TestData>());

        // Exécuter les tests
        TestJsonSaveSystem();
        TestPlayerPrefsSaveSystem();

        Debug.Log("✅ All tests completed successfully!");
    }

    void TestJsonSaveSystem()
    {
        var data = new TestData { Name = "JSON Test", Score = 100 };
        saveManager.Save(jsonKey, data);
        bool exists = saveManager.Exists(jsonKey);
        Debug.Assert(exists, "❌ JSON Save System failed: Key does not exist!");

        var loadedData = saveManager.Load(jsonKey);
        Debug.Assert(loadedData[typeof(JsonSaveSystem<TestData>)].Name == "JSON Test", "❌ JSON data does not match!");
        Debug.Assert(loadedData[typeof(JsonSaveSystem<TestData>)].Score == 100, "❌ JSON Score mismatch!");

        saveManager.Delete(jsonKey);
        Debug.Assert(!saveManager.Exists(jsonKey), "❌ JSON Save System failed: Data not deleted!");
    }

    void TestPlayerPrefsSaveSystem()
    {
        var data = new TestData { Name = "PlayerPrefs Test", Score = 200 };
        saveManager.Save(prefsKey, data);
        bool exists = saveManager.Exists(prefsKey);
        Debug.Assert(exists, "❌ PlayerPrefs Save System failed: Key does not exist!");

        var loadedData = saveManager.Load(prefsKey);
        Debug.Assert(loadedData[typeof(PlayerPrefsSaveSystem<TestData>)].Name == "PlayerPrefs Test", "❌ PlayerPrefs data does not match!");
        Debug.Assert(loadedData[typeof(PlayerPrefsSaveSystem<TestData>)].Score == 200, "❌ PlayerPrefs Score mismatch!");

        saveManager.Delete(prefsKey);
        Debug.Assert(!saveManager.Exists(prefsKey), "❌ PlayerPrefs Save System failed: Data not deleted!");
    }

}

public class TestData
{
    public string Name;
    public int Score;
}

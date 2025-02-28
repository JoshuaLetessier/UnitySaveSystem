using NUnit.Framework;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveSystemTests
{
    private SaveManager<TestData> saveManager;
    private string jsonKey = "test_json";
    private string prefsKey = "test_prefs";
    private string encryptedKey = "test_encrypted";

    [SetUp]
    public void Setup()
    {
        saveManager = new SaveManager<TestData>();

        // Ajouter différents systèmes de sauvegarde
        saveManager.Register(new JsonSaveSystem<TestData>());
        saveManager.Register(new PlayerPrefsSaveSystem<TestData>());
        saveManager.Register(new EncryptedSaveDecorator<TestData>(new JsonSaveSystem<TestData>()));
    }

    [TearDown]
    public void Cleanup()
    {
        saveManager.Delete(jsonKey);
        saveManager.Delete(prefsKey);
        saveManager.Delete(encryptedKey);
    }

    [Test]
    public void Test_JsonSaveSystem()
    {
        var data = new TestData { Name = "JSON Test", Score = 100 };

        saveManager.Save(jsonKey, data);
        Assert.IsTrue(saveManager.Exists(jsonKey));

        var loadedData = saveManager.Load(jsonKey);
        Assert.AreEqual("JSON Test", loadedData[typeof(JsonSaveSystem<TestData>)].Name);
        Assert.AreEqual(100, loadedData[typeof(JsonSaveSystem<TestData>)].Score);

        saveManager.Delete(jsonKey);
        Assert.IsFalse(saveManager.Exists(jsonKey));
    }

    [Test]
    public void Test_PlayerPrefsSaveSystem()
    {
        var data = new TestData { Name = "PlayerPrefs Test", Score = 200 };

        saveManager.Save(prefsKey, data);
        Assert.IsTrue(saveManager.Exists(prefsKey));

        var loadedData = saveManager.Load(prefsKey);
        Assert.AreEqual("PlayerPrefs Test", loadedData[typeof(PlayerPrefsSaveSystem<TestData>)].Name);
        Assert.AreEqual(200, loadedData[typeof(PlayerPrefsSaveSystem<TestData>)].Score);

        saveManager.Delete(prefsKey);
        Assert.IsFalse(saveManager.Exists(prefsKey));
    }

    [Test]
    public void Test_EncryptedJsonSaveSystem()
    {
        var data = new TestData { Name = "Encrypted Test", Score = 300 };

        saveManager.Save(encryptedKey, data);
        Assert.IsTrue(saveManager.Exists(encryptedKey));

        var loadedData = saveManager.Load(encryptedKey);
        Assert.AreEqual("Encrypted Test", loadedData[typeof(EncryptedSaveDecorator<TestData>)].Name);
        Assert.AreEqual(300, loadedData[typeof(EncryptedSaveDecorator<TestData>)].Score);

        string filePath = Path.Combine(Application.persistentDataPath, encryptedKey + ".json");
        Assert.IsTrue(File.Exists(filePath));

        string fileContent = File.ReadAllText(filePath);
        Assert.AreNotEqual(JsonConvert.SerializeObject(data), fileContent); // Vérifie que les données sont bien encryptées

        saveManager.Delete(encryptedKey);
        Assert.IsFalse(saveManager.Exists(encryptedKey));
    }

    [Test]
    public void Test_LoadNonExistentData()
    {
        var loadedData = saveManager.Load("non_existent_key");
        Assert.IsNull(loadedData);
    }
}

public class TestData
{
    public string Name;
    public int Score;
}

# Save System for Unity

🎮 A modular save system for Unity, supporting JSON and PlayerPrefs.

## 📌 Features
✅ **Save and load** data using JSON and PlayerPrefs.  
✅ **Generic `SaveManager<T>` system**, compatible with any data type.  
✅ **Integration with Unity Package Manager (UPM).**  
✅ **Ready for extensions (Firebase, SQLite, encryption, etc.).**  

## 📂 Installation

### 📌 Method 1: Via Unity Package Manager (UPM)
1. **Open Unity**, go to `Window > Package Manager`.
2. Click on **"Add package from Git URL..."**.
3. Enter the following URL:
   ```
   [https://github.com/yourusername/savesystem.git](https://github.com/JoshuaLetessier/UnitySaveSystem.git?path=/Packages/com.Faolline.SaveSystem)
   ```
4. **Click `Add`**, and Unity will automatically download the library.

### 📌 Method 2: Local Installation
1. Download this repository.
2. Move the `com.yourusername.savesystem` folder into your project's `Packages/` directory.
3. Open **`Window > Package Manager`**, then click **"Add package from disk..."**.
4. Select `package.json`.

## 🚀 Usage

### 📌 1️⃣ Declare a `SaveManager<T>`
```csharp
SaveManager<GameSettings> settingsManager = new SaveManager<GameSettings>();
settingsManager.Register(new JsonSaveSystem<GameSettings>());
settingsManager.Register(new PlayerPrefsSaveSystem<GameSettings>());
```

### 📌 2️⃣ Save and Load Data
```csharp
settingsManager.Save("game_settings", new GameSettings { Volume = 80.0f, Fullscreen = true });

if (settingsManager.Exists("game_settings"))
{
    var data = settingsManager.Load("game_settings");
    Debug.Log($"📂 Loaded: Volume={data[typeof(JsonSaveSystem<GameSettings>)].Volume}");
}
```

## 📜 License
This project is licensed under the **MIT License**. You are free to use it in your projects.

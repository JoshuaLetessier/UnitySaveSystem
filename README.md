# Save System for Unity

🎮 A **modular save system for Unity**, supporting multiple storage methods (JSON, PlayerPrefs, and more).  
Easily extendable and configurable through Unity's **Package Manager (UPM)**.

---

## 📌 Features

✅ **Modular system** – Select only the save methods you need (JSON, PlayerPrefs, SQLite, etc.).  
✅ **Automatic package installation** via Unity Package Manager (UPM).  
✅ **Built-in UI** to enable/disable save methods from Unity's Editor.  
✅ **Generic `SaveManager<T>` architecture**, compatible with any data type.  

---

## 📂 Installation

### 📌 Method 1: Using Unity Package Manager (UPM)
1. **Open Unity**, go to `Window > Package Manager`.
2. Click on **"Add package from Git URL..."**.
3. Enter the following URL:
   ```
   https://github.com/JoshuaLetessier/UnitySaveSystem.git?path=/Packages/com.faolline.savesystem.core
   ```
4. Click **Add**, and Unity will install the core package.

🔹 **To add specific save methods**, use the **Save System Modules window** inside Unity (`Window > Save System Modules`).  

---

### 📌 Method 2: Local Installation
1. **Download this repository**.
2. Move the `com.faolline.savesystem` folder into your project's `Packages/` directory.
3. Open `Window > Package Manager`, then click **"Add package from disk..."**.
4. Select `package.json`.

---

## 🚀 Usage

### 📌 1️⃣ Enable Modules via Unity's UI
1. Open **`Window > Save System Modules`**.
2. Check the save methods you want (JSON, PlayerPrefs, etc.).
3. Click **"Apply Changes"**, Unity will auto-install the selected modules.

---

### 📌 2️⃣ Declare a `SaveManager<T>`
```csharp
SaveManager<GameSettings> settingsManager = new SaveManager<GameSettings>();
settingsManager.Register(new JsonSaveSystem<GameSettings>());
settingsManager.Register(new PlayerPrefsSaveSystem<GameSettings>());
```

---

### 📌 3️⃣ Save and Load Data
```csharp
settingsManager.Save("game_settings", new GameSettings { Volume = 80.0f, Fullscreen = true });

if (settingsManager.Exists("game_settings"))
{
    var result = settingsManager.Load("game_settings");

    if (result.TryGet<JsonSaveSystem<GameSettings>>(out var data))
    {
        Debug.Log($"📂 Loaded from JSON: Volume={data.Volume}");
    }
}
```

---

## 📦 Available Modules
| Module Name | Description |
|-------------|------------|
| **JSON Save System** | Saves data to JSON files. |
| **PlayerPrefs Save System** | Uses Unity's PlayerPrefs system. |

---

## 📜 License
This project is licensed under the **MIT License**. You are free to use, modify, and distribute it in your projects.

---

### 🚀 Future Improvements
- 🔐 **Encryption module** for secured save files.
- ☁ **Cloud storage support** (Firebase, Google Drive, AWS).
- 📂 **Multiple storage backends** (SQL, NoSQL, Binary).

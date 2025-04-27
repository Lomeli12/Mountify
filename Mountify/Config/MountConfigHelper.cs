using System.IO;
using System.Text;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Mountify.Data;
using Newtonsoft.Json;

namespace Mountify;

public class MountConfigHelper {
    private static string CONFIG_PATH = "";

    private readonly static string MOUNTIFY_FOLDER_NAME = "Mountify";

    private readonly static string BASE_FILE_NAME = "MountID-";

    public static void init(IDalamudPluginInterface pluginInterface) {
        CONFIG_PATH = pluginInterface.ConfigDirectory.FullName;
    }

    public static MountSettings loadFromFile(IPluginLog log, MountData mount) {
        validateConfigDir();
        var mountSettings = new MountSettings(mount.getID(), true);
        log.Debug($"Base Mount Data: {mountSettings}");

        var fileName = BASE_FILE_NAME + mount.getID() + ".json";

        var filePath = Path.Combine(CONFIG_PATH, fileName);
        if (File.Exists(filePath)) {
            log.Debug("Loading Mount Settings: " + filePath);
            var loadedData = JsonConvert.DeserializeObject<MountSettings>(File.ReadAllText(filePath, Encoding.UTF8));
            if (loadedData != null) mountSettings.setBGMEnabled(loadedData.isBGMEnabled());
        }

        return mountSettings;
    }

    public static void saveToFile(IPluginLog log, MountSettings mountSettings) {
        log.Debug(mountSettings.ToString());
        if (mountSettings.getID() < 1)
            return;

        validateConfigDir();
        var fileName = BASE_FILE_NAME + mountSettings.getID() + ".json";
        var filePath = Path.Combine(CONFIG_PATH, fileName);
        var json = JsonConvert.SerializeObject(mountSettings, Formatting.Indented);
        log.Debug($"Saving to file: {filePath}");
        File.WriteAllText(filePath, json, Encoding.UTF8);
    }

    private static void validateConfigDir() {
        if (!Directory.Exists(CONFIG_PATH))
            Directory.CreateDirectory(CONFIG_PATH);
    }
}

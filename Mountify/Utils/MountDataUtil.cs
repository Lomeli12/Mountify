using System.IO;
using System.Text;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Mountify.Data;
using Newtonsoft.Json;

namespace Mountify;

public class MountDataUtil {
    private static string CONFIG_PATH = "";

    private readonly static string MOUNTIFY_FOLDER_NAME = "Mountify";

    private readonly static string BASE_FILE_NAME = "MountID-";

    public static void init(IDalamudPluginInterface pluginInterface) {
        CONFIG_PATH = pluginInterface.ConfigDirectory.FullName;
    }

    public static MountData loadFromFile(IPluginLog log, MountData mount) {
        validateConfigDir();
        log.Debug($"Base Mount Data: {mount}");
        var fileName = BASE_FILE_NAME + mount.getID() + ".json";

        var filePath = Path.Combine(CONFIG_PATH, fileName);
        if (File.Exists(filePath)) {
            log.Debug("Loading Mount Settings: " + filePath);
            var loadedData = JsonConvert.DeserializeObject<MountData>(File.ReadAllText(filePath, Encoding.UTF8));
            if (loadedData != null) mount.safeCopyData(loadedData);
        }

        return mount;
    }

    public static void saveToFile(IPluginLog log, MountData mountData) {
        log.Debug($"Saving mount data: {mountData}");
        if (mountData.getID() < 1)
            return;

        validateConfigDir();
        var fileName = BASE_FILE_NAME + mountData.getID() + ".json";
        var filePath = Path.Combine(CONFIG_PATH, fileName);
        var json = JsonConvert.SerializeObject(mountData, Formatting.Indented);
        log.Debug($"Saving to file: {filePath}");
        File.WriteAllText(filePath, json, Encoding.UTF8);
    }

    private static void validateConfigDir() {
        if (!Directory.Exists(CONFIG_PATH))
            Directory.CreateDirectory(CONFIG_PATH);
    }
}

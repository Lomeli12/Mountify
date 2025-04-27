using System.Collections.Generic;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Config;
using Dalamud.Plugin.Services;
using Lumina.Excel.Sheets;
using Mountify.Data;
using Mountify.Utils;

namespace Mountify.Services;

public class MountService {
    private static MountService instance;
    private PluginServices services;
    private List<MountSettings> cachedMountSettings;
    public static void initService(PluginServices services) {
        if (instance == null)
            instance = new MountService(services);
    }
    
    public static MountService getInstance() => instance;

    private MountService(PluginServices services) {
        cachedMountSettings = [];

        this.services = services;
        services.condition.ConditionChange += onConditionChange;
    }

    public MountSettings getMountSettings(MountData mount) {
        var mountSettings = cachedMountSettings.Find(settings => settings.getID() == mount.getID());
        if (mountSettings == null) {
            mountSettings = MountConfigHelper.loadFromFile(services.log, mount);
            cachedMountSettings.Add(mountSettings);
        }

        return mountSettings;
    }

    public MountSettings setMountSettings(MountSettings newSettings) {
        var index = cachedMountSettings.FindIndex(settings => settings.getID() == newSettings.getID());
        if (index < 0)
            cachedMountSettings.RemoveAt(index);
        
        MountConfigHelper.saveToFile(services.log, newSettings);
        cachedMountSettings.Add(newSettings);
        return newSettings;
    }

    private void onConditionChange(ConditionFlag flag, bool value) {
        if (flag is not (ConditionFlag.Mounted or ConditionFlag.Mounted2))
            return;
        var mountID = PluginUtils.getCurrentMount(services.clientState);
        if (!value || mountID <= 0) {
            services.gameConfig.Set(SystemConfigOption.SoundChocobo, false);
            return;
        }
        
        var settings = getMountSettings(new MountData(mountID));
        services.gameConfig.Set(SystemConfigOption.SoundChocobo, settings.isBGMEnabled());
    }
}

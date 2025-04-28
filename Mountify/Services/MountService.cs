using System.Collections.Generic;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Config;
using Mountify.Data;
using Mountify.Utils;

namespace Mountify.Services;

public class MountService {
    private static MountService instance;
    private PluginServices services;
    private List<MountData> cachedMountSettings;

    public static void initService(PluginServices services) => instance = new MountService(services);

    public static MountService getInstance() => instance;

    private MountService(PluginServices services) {
        cachedMountSettings = [];

        this.services = services;
        services.condition.ConditionChange += onConditionChange;
    }

    public MountData getMountSettings(MountData mount) {
        var mountSettings = cachedMountSettings.Find(settings => settings.getID() == mount.getID());
        if (mountSettings == null) {
            mountSettings = MountDataUtil.loadFromFile(services.log, mount);
            cachedMountSettings.Add(mountSettings);
        }

        return mountSettings;
    }

    public void setMountSettings(MountData newSettings) {
        var index = cachedMountSettings.FindIndex(settings => settings.getID() == newSettings.getID());
        if (index < 0)
            cachedMountSettings.RemoveAt(index);

        MountDataUtil.saveToFile(services.log, newSettings);
        cachedMountSettings.Add(newSettings);
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

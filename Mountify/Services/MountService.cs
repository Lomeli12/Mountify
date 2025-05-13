using System.Collections.Generic;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Config;
using Mountify.Data;
using Mountify.Utils;

namespace Mountify.Services;

public class MountService {
    private static MountService instance;
    private List<MountData> cachedMountSettings;

    public static void initService() => instance = new MountService();

    public static MountService getInstance() => instance;

    private MountService() {
        cachedMountSettings = [];

        PluginServices.condition.ConditionChange += onConditionChange;
    }

    public MountData getMountSettings(MountData mount) {
        var mountSettings = cachedMountSettings.Find(settings => settings.getID() == mount.getID());
        if (mountSettings == null) {
            mountSettings = MountDataUtil.loadFromFile(mount);
            cachedMountSettings.Add(mountSettings);
        }

        return mountSettings;
    }

    public void setMountSettings(MountData newSettings) {
        var index = cachedMountSettings.FindIndex(settings => settings.getID() == newSettings.getID());
        if (index < 0)
            cachedMountSettings.RemoveAt(index);

        MountDataUtil.saveToFile(newSettings);
        cachedMountSettings.Add(newSettings);
    }

    private void onConditionChange(ConditionFlag flag, bool value) {
        if (flag is not (ConditionFlag.Mounted or ConditionFlag.Mounted2))
            return;

        var mountID = PluginUtils.getCurrentMount();
        if (!value || mountID <= 0) {
            PluginServices.gameConfig.Set(SystemConfigOption.SoundChocobo, false);
            return;
        }

        var settings = getMountSettings(new MountData(mountID));
        PluginServices.gameConfig.Set(SystemConfigOption.SoundChocobo, settings.isBGMEnabled());
    }
}

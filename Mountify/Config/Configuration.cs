using System;
using Dalamud.Configuration;
using Mountify.Services;

namespace Mountify;

[Serializable]
public class Configuration : IPluginConfiguration {
    public int Version { get; set; } = 0;

    public bool followMountBGMSettings { get; set; } = true;

    // the below exist just to make saving less cumbersome
    public void Save(PluginServices services) {
        services.pluginInterface.SavePluginConfig(this);
    }
}

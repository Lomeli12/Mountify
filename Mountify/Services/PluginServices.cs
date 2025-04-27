using System;
using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace Mountify.Services;

public class PluginServices : IDisposable {
    internal readonly IDalamudPluginInterface pluginInterface;
    private bool disposedValue;

    [PluginService] public ITextureProvider textureProvider { get; private set; } = null;

    [PluginService] public ICommandManager commandManager { get; private set; } = null;

    [PluginService] public IClientState clientState { get; private set; } = null;

    [PluginService] public IChatGui chatGUI { get; private set; } = null;

    [PluginService] public IDataManager dataManager { get; private set; } = null;

    [PluginService] public ISigScanner sigScanner { get; private set; } = null;

    [PluginService] public ICondition condition { get; private set; } = null;

    [PluginService] public IPluginLog log { get; private set; } = null;

    internal PluginServices(IDalamudPluginInterface pluginInterface) {
        this.pluginInterface = pluginInterface;
        _ = pluginInterface.Inject(this);
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
    }
}

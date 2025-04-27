using System;
using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace Mountify.Services;

public class PluginService : IDisposable {
    
    private bool disposedValue;
    
    [PluginService]
    public IDalamudPluginInterface pluginInterface { get; private set; } = null;
    
    [PluginService]
    public ITextureProvider textureProvider { get; private set; } = null;
    
    [PluginService]
    public ICommandManager commandManager { get; private set; } = null;
    
    [PluginService]
    public IClientState clientState { get; private set; } = null;
    
    [PluginService]
    public IDataManager dataManager { get; private set; } = null;
    
    [PluginService]
    public ISigScanner sigScanner { get; private set; } = null;
    
    [PluginService]
    public ICondition condition { get; private set; } = null;
    
    [PluginService]
    public IPluginLog log { get; private set; } = null;
    
#pragma warning disable SeStringEvaluator
    [PluginService]
    public ISeStringEvaluator seStringEvaluator { get; private set; } = null;
    
    public void Dispose() {
        GC.SuppressFinalize(this);
    }
}

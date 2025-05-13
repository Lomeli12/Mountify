using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace Mountify.Services;

public class PluginServices {
    public static IDalamudPluginInterface pluginInterface;

    [PluginService] public static ITextureProvider textureProvider { get; private set; } = null;

    [PluginService] public static ICommandManager commandManager { get; private set; } = null;

    [PluginService] public static IClientState clientState { get; private set; } = null;

    [PluginService] public static IChatGui chatGUI { get; private set; } = null;

    [PluginService] public static IDataManager dataManager { get; private set; } = null;

    [PluginService] public static ICondition condition { get; private set; } = null;

    [PluginService] public static IPluginLog log { get; private set; } = null;

    [PluginService] public static IGameConfig gameConfig { get; private set; } = null;

    public static void initService(IDalamudPluginInterface pluginInterface) {
        pluginInterface.Create<PluginServices>();
        PluginServices.pluginInterface = pluginInterface;
    }
}

using System;
using Dalamud.Game.Command;
using Dalamud.Plugin;
using Mountify.Services;

namespace Mountify;

public sealed class Mountify : IDalamudPlugin {
    readonly PluginServices pluginServices;

    private const string commandName = "/pmountify";

    public Configuration config { get; init; }

    public Mountify(IDalamudPluginInterface pluginInterface) {
        ArgumentNullException.ThrowIfNull(pluginInterface);

        pluginServices = new PluginServices(pluginInterface);
        initServices(pluginInterface);

        config = pluginServices.pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        pluginServices.commandManager.AddHandler(commandName, new CommandInfo(onCommand) {
            HelpMessage = "A useful message to display in /xlhelp"
        });
    }

    public void Dispose() {
        UIService.getInstance().Dispose();

        pluginServices.commandManager.RemoveHandler(commandName);
    }

    private void initServices(IDalamudPluginInterface pluginInterface) {
        UIService.getInstance().initWindows(this, pluginServices);
        ImageService.initService(pluginServices.textureProvider);
        MountService.initService(pluginServices);
        MountDataUtil.init(pluginInterface);
    }

    private void onCommand(string command, string args) {
        if (string.IsNullOrEmpty(args)) {
            UIService.getInstance().toggleMountsUI();
            return;
        }

        if (args.Equals("config", StringComparison.InvariantCultureIgnoreCase)) {
            UIService.getInstance().toggleConfigUi();
        }
    }
}

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
        initServices(pluginInterface);

        config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        PluginServices.commandManager.AddHandler(commandName, new CommandInfo(onCommand) {
            HelpMessage = "A useful message to display in /xlhelp"
        });
    }

    public void Dispose() {
        UIService.getInstance().Dispose();

        PluginServices.commandManager.RemoveHandler(commandName);
    }

    private void initServices(IDalamudPluginInterface pluginInterface) {
        PluginServices.initService(pluginInterface);
        BGMService.initService();
        UIService.getInstance().initWindows(this);
        MountService.initService();
        MountDataUtil.init();
    }

    private void onCommand(string command, string args) {
        if (string.IsNullOrEmpty(args)) {
            UIService.getInstance().toggleListUI();
            return;
        }

        if (args.Equals("config", StringComparison.InvariantCultureIgnoreCase)) {
            UIService.getInstance().toggleConfigUi();
        }
    }
}

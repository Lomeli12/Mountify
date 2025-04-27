using System;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Mountify.Data;
using Mountify.Services;
using Mountify.Windows;

namespace Mountify;

public sealed class Mountify : IDalamudPlugin {
    readonly PluginServices pluginServices;

    private const string commandName = "/pmountify";

    public Configuration config { get; init; }

    public readonly WindowSystem windowSystem = new("Mountify");
    private ConfigWindow configWindow { get; init; }
    private MountsWindow mountsWindow { get; init; }
    
    private MountListWindow MountListWindow { get; init; }

    public Mountify(IDalamudPluginInterface pluginInterface) {
        ArgumentNullException.ThrowIfNull(pluginInterface);

        pluginServices = new PluginServices(pluginInterface);

        config = pluginServices.pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        MountConfigHelper.init(pluginInterface);
        
        configWindow = new ConfigWindow(this, pluginServices);
        mountsWindow = new MountsWindow(this, pluginServices);
        MountListWindow = new MountListWindow(this, pluginServices);

        windowSystem.AddWindow(configWindow);
        windowSystem.AddWindow(mountsWindow);
        windowSystem.AddWindow(MountListWindow);

        pluginServices.commandManager.AddHandler(commandName, new CommandInfo(onCommand) {
            HelpMessage = "A useful message to display in /xlhelp"
        });

        pluginServices.pluginInterface.UiBuilder.Draw += drawUi;
        
        pluginServices.pluginInterface.UiBuilder.OpenConfigUi += toggleConfigUi;
        pluginServices.pluginInterface.UiBuilder.OpenMainUi += toggleMountsUI;
    }

    public void Dispose() {
        windowSystem.RemoveAllWindows();

        configWindow.Dispose();
        mountsWindow.Dispose();
        MountListWindow.Dispose();

        pluginServices.commandManager.RemoveHandler(commandName);
    }

    private void onCommand(string command, string args) {
        if (string.IsNullOrEmpty(args)) {
            toggleMountsUI();
            return;
        }

        if (args.Equals("config", StringComparison.InvariantCultureIgnoreCase)) {
            toggleConfigUi();
        }
    }

    private void drawUi() => windowSystem.Draw();

    public void toggleConfigUi() => configWindow.Toggle();

    public void toggleMountsUI() => mountsWindow.Toggle();

    public void toggleMountSettingsUi(MountData mount) {
        pluginServices.log.Info($"Toggling Window with {mount.ToString()}");
        MountListWindow.openMountData(mount);
    }
}

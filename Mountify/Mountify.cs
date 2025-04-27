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
    private MountListWindow mountListWindow { get; init; }
    
    private MountSettingsWindow mountSettingsWindow { get; init; }

    public Mountify(IDalamudPluginInterface pluginInterface) {
        ArgumentNullException.ThrowIfNull(pluginInterface);
        
        pluginServices = new PluginServices(pluginInterface);
        initServices(pluginInterface);
        
        config = pluginServices.pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        configWindow = new ConfigWindow(this, pluginServices);
        mountListWindow = new MountListWindow(this, pluginServices);
        mountSettingsWindow = new MountSettingsWindow(this, pluginServices);

        windowSystem.AddWindow(configWindow);
        windowSystem.AddWindow(mountListWindow);
        windowSystem.AddWindow(mountSettingsWindow);

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
        mountListWindow.Dispose();
        mountSettingsWindow.Dispose();

        pluginServices.commandManager.RemoveHandler(commandName);
    }

    private void initServices(IDalamudPluginInterface pluginInterface) {
        MountService.initService(pluginServices);
        MountDataUtil.init(pluginInterface);
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

    public void toggleMountsUI() => mountListWindow.Toggle();

    public void toggleMountSettingsUi(MountData mount) {
        pluginServices.log.Info($"Toggling Window with {mount}");
        mountSettingsWindow.openMountData(mount);
    }
}

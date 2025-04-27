using System;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Mountify.Data;
using Mountify.Windows;

namespace Mountify.Services;

public class UIService : IDisposable {
    private static UIService instance;

    public static UIService getInstance() {
        if (instance == null)
            instance = new UIService();
        return instance;
    }

    private readonly WindowSystem winSystem;
    private IPluginLog log;
    
    private ConfigWindow configWindow { get; set; }
    private MountListWindow mountListWindow { get; set; }
    private MountSettingsWindow mountSettingsWindow { get; set; }
    private UIService() {
        winSystem = new(MountifyVars.PLUGIN_NAME);
    }

    public void initWindows(Mountify plugin, PluginServices services) {
        log = services.log;
        
        configWindow = new ConfigWindow(plugin, services);
        mountListWindow = new MountListWindow(plugin, services);
        mountSettingsWindow = new MountSettingsWindow(plugin, services);

        winSystem.AddWindow(configWindow);
        winSystem.AddWindow(mountListWindow);
        winSystem.AddWindow(mountSettingsWindow);
        
        services.pluginInterface.UiBuilder.Draw += drawUi;
        services.pluginInterface.UiBuilder.OpenConfigUi += toggleConfigUi;
        services.pluginInterface.UiBuilder.OpenMainUi += toggleMountsUI;
    }
    
    private void drawUi() => winSystem.Draw();

    public void toggleConfigUi() => configWindow.Toggle();

    public void toggleMountsUI() => mountListWindow.Toggle();

    public void toggleMountSettingsUi(MountData mount) {
        log.Info($"Toggling Window with {mount}");
        mountSettingsWindow.openMountData(mount);
    }

    public void Dispose() {
        winSystem.RemoveAllWindows();
        
        configWindow.Dispose();
        mountListWindow.Dispose();
        mountSettingsWindow.Dispose();
    }

    public WindowSystem getWindowSystem() => winSystem;
}

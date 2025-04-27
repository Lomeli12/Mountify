using System;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Mountify.Data;
using Mountify.UI;

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
    
    private ConfigUI configUi { get; set; }
    private MountListUI mountListUi { get; set; }
    private MountSettingsUI mountSettingsUi { get; set; }
    private UIService() {
        winSystem = new(MountifyVars.PLUGIN_NAME);
    }

    public void initWindows(Mountify plugin, PluginServices services) {
        log = services.log;
        
        configUi = new ConfigUI(plugin, services);
        mountListUi = new MountListUI(plugin, services);
        mountSettingsUi = new MountSettingsUI(plugin, services);

        winSystem.AddWindow(configUi);
        winSystem.AddWindow(mountListUi);
        winSystem.AddWindow(mountSettingsUi);
        
        services.pluginInterface.UiBuilder.Draw += drawUi;
        services.pluginInterface.UiBuilder.OpenConfigUi += toggleConfigUi;
        services.pluginInterface.UiBuilder.OpenMainUi += toggleListUI;
    }
    
    private void drawUi() => winSystem.Draw();

    public void toggleConfigUi() => configUi.Toggle();

    public void toggleListUI() => mountListUi.Toggle();

    public void toggleMountSettingsUi(MountData mount) {
        log.Info($"Toggling Window with {mount}");
        mountSettingsUi.openMountData(mount);
    }

    public void Dispose() {
        winSystem.RemoveAllWindows();
        
        configUi.Dispose();
        mountListUi.Dispose();
        mountSettingsUi.Dispose();
    }

    public WindowSystem getWindowSystem() => winSystem;
}

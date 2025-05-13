using System;
using Dalamud.Interface.Windowing;
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

    private ConfigUI configUi { get; set; }
    private MountListUI mountListUi { get; set; }
    private MountSettingsUI mountSettingsUi { get; set; }

    private UIService() {
        winSystem = new(MountifyVars.PLUGIN_NAME);
    }

    public void initWindows(Mountify plugin) {
        configUi = new ConfigUI(plugin);
        mountListUi = new MountListUI(plugin);
        mountSettingsUi = new MountSettingsUI(plugin);

        winSystem.AddWindow(configUi);
        winSystem.AddWindow(mountListUi);
        winSystem.AddWindow(mountSettingsUi);

        PluginServices.pluginInterface.UiBuilder.Draw += drawUi;
        PluginServices.pluginInterface.UiBuilder.OpenConfigUi += toggleConfigUi;
        PluginServices.pluginInterface.UiBuilder.OpenMainUi += toggleListUI;
    }

    private void drawUi() => winSystem.Draw();

    public MountListUI getMountListUI() => mountListUi;

    public void toggleConfigUi() => configUi.Toggle();

    public void toggleListUI() => mountListUi.Toggle();

    public void toggleMountSettingsUi(MountData mount) {
        PluginServices.log.Info($"Toggling Window with {mount}");
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

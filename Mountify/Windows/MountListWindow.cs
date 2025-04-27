using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using Mountify.Data;
using Mountify.Services;

namespace Mountify.Windows;

public class MountListWindow : Window, IDisposable {
    private IPluginLog log;

    private ImageService imgService;

    private MountData mount;
    private bool enableBGM;
    private MountSettings mountSettings;

    public MountListWindow(Mountify plugin, PluginServices services) : base("Mountify-Mount_Settings", ImGuiWindowFlags.NoResize) {
        log = services.log;

        imgService = new ImageService(services.textureProvider);

        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(300, 200),
            MaximumSize = new Vector2(300, 200)
        };
    }

    public void openMountData(MountData mountData) {
        log.Debug(mountData.ToString());
        mount = mountData;
        mountSettings = MountConfigHelper.loadFromFile(log, mountData);
        enableBGM = mountSettings.isBGMEnabled();
        WindowName = mount.getFormattedName();
        Toggle();
    }


    public override void Draw() {
        var mountIcon = imgService.getIcon(mount.getIcon());
        ImGui.Image(mountIcon.ImGuiHandle, new Vector2(45, 45));
        ImGui.SameLine();
        ImGui.Text(mount.getFormattedName());
        ImGui.NewLine();
        ImGui.Checkbox("Enable Mount BGM", ref enableBGM);

        ImGui.NewLine();
        if (ImGui.Button("Save Mount Settings")) {
            mountSettings.setBGMEnabled(enableBGM);
            log.Debug($"Saving settings: {mountSettings}");
            MountConfigHelper.saveToFile(log, mountSettings);
        }
    }

    public void Dispose() { }
}

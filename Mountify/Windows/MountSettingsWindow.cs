using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Mountify.Data;
using Mountify.Services;

namespace Mountify.Windows;

public class MountSettingsWindow : Window, IDisposable {
    private PluginServices services;
    private ImageService imgService;

    private MountData mount;
    private bool enableBGM;
    private MountSettings mountSettings;

    public MountSettingsWindow(Mountify plugin, PluginServices services)
        : base("Mountify-Mount_Settings", ImGuiWindowFlags.NoResize) {
        this.services = services;

        imgService = new ImageService(services.textureProvider);

        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(300, 200),
            MaximumSize = new Vector2(300, 200)
        };
    }

    public void openMountData(MountData mountData) {
        services.log.Debug(mountData.ToString());
        mount = mountData;
        mountSettings = MountService.getInstance().getMountSettings(mountData);
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
            services.log.Debug($"Saving settings: {mountSettings}");
            MountService.getInstance().setMountSettings(mountSettings);
        }
    }

    public void Dispose() { }
}

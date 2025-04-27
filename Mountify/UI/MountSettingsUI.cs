using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Mountify.Data;
using Mountify.Services;

namespace Mountify.UI;

public class MountSettingsUI : Window, IDisposable {
    private PluginServices services;

    private MountData mount;
    private bool enableBGM;

    public MountSettingsUI(Mountify plugin, PluginServices services)
        : base("Mountify-Mount_Settings", ImGuiWindowFlags.NoResize) {
        this.services = services;


        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(300, 200),
            MaximumSize = new Vector2(300, 200)
        };
    }

    public void openMountData(MountData mountData) {
        services.log.Debug(mountData.ToString());
        mount = MountService.getInstance().getMountSettings(mountData);
        enableBGM = mount.isBGMEnabled();
        WindowName = mount.getFormattedName();
        Toggle();
    }


    public override void Draw() {
        var mountIcon = ImageService.getInstance().getIcon(mount.getIcon());
        ImGui.Image(mountIcon.ImGuiHandle, new Vector2(45, 45));
        ImGui.SameLine();
        ImGui.Text(mount.getFormattedName());
        ImGui.NewLine();
        ImGui.Checkbox("Enable Mount BGM", ref enableBGM);

        ImGui.NewLine();
        if (ImGui.Button("Save Mount Settings")) {
            mount.setBGMEnabled(enableBGM);
            services.log.Debug($"Saving settings: {mount}");
            MountService.getInstance().setMountSettings(mount);
        }
    }

    public void Dispose() { }
}

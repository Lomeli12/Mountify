using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Mountify.Data;
using Mountify.Services;
using Mountify.Utils;

namespace Mountify.UI;

public class MountSettingsUI : Window, IDisposable {
    private MountData mount;
    private bool enableBGM;

    public MountSettingsUI(Mountify plugin)
        : base("Mountify-Mount_Settings", ImGuiWindowFlags.NoResize) {
        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(300, 200),
            MaximumSize = new Vector2(300, 200)
        };
    }

    private void followParent() {
        var parentPos = UIService.getInstance().getMountListUI().Position;
        var parentSize = UIService.getInstance().getMountListUI().Size;
        if (parentPos != null && parentSize != null)
            Position = new Vector2(parentPos.Value.X + parentSize.Value.X + 10, parentPos.Value.Y);
    }

    public void openMountData(MountData mountData) {
        PluginServices.log.Debug(mountData.ToString());
        mount = MountService.getInstance().getMountSettings(mountData);
        enableBGM = mount.isBGMEnabled();
        WindowName = mount.getFormattedName();
        Toggle();
    }

    public override void Draw() {
        var mountIcon = PluginUtils.getIcon(mount.getIcon());
        ImGui.Image(mountIcon.ImGuiHandle, new Vector2(45, 45));
        ImGui.SameLine();
        ImGui.Text(mount.getFormattedName());

        ImGui.Checkbox("Enable Mount BGM", ref enableBGM);
        if (!ImGui.Button("Save Mount Settings"))
            return;

        if (mount.isBGMEnabled() != enableBGM)
            PluginServices.chatGUI.Print($"{mount.getFormattedName()} BGM is " + (enableBGM ? "enabled." : "disabled."));

        mount.setBGMEnabled(enableBGM);
        PluginServices.log.Debug($"Saving settings: {mount}");
        MountService.getInstance().setMountSettings(mount);
    }

    public void Dispose() { }
}

using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Mountify.Data;
using Mountify.Services;
using Mountify.Utils;

namespace Mountify.UI;

public class MountListUI : Window, IDisposable {
    private Mountify plugin;

    private string filterText;
    private bool test;

    private PlayerMountDB playerMounts;

    public MountListUI(Mountify plugin) : base("Mountify") {
        this.plugin = plugin;
        playerMounts = new PlayerMountDB();

        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(400, 400),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
    }

    public override void OnOpen() {
        base.OnOpen();
        playerMounts.queueRefresh();
        playerMounts.getMounts();
        var currentMount = PluginUtils.getCurrentMount();
        if (currentMount <= 0) {
            return;
        }

        playerMounts.moveIDToTop(currentMount);
    }

    public override void Draw() {
        if (playerMounts.getMounts().Count < 1) {
            ImGui.Text("No mounts found.");
            return;
        }

        /*ImGui.BeginChild("test");
        //ImGui.SetNextItemWidth(150);
        ImGui.InputText("##Test", ref filterText, 255);
        ImGui.EndChild();*/

        ImGui.BeginChild("MountList");

        foreach (var mount in playerMounts.getMounts().Where(mount => mount.getID() > 0)) {
            try {
                ImGui.SameLine();
                var mountIcon = PluginUtils.getIcon(mount.getIcon());
                if (ImGui.ImageButton(mountIcon.ImGuiHandle, new Vector2(45, 45)))
                    UIService.getInstance().toggleMountSettingsUi(mount);

                ImGui.SameLine();
                if (ImGui.Button(mount.getFormattedName(), new Vector2(150, 45)))
                    UIService.getInstance().toggleMountSettingsUi(mount);

                ImGui.NewLine();
            } catch (Exception e) {
                // ignored
            }
        }

        ImGui.EndChild();
    }

    public void Dispose() { }
}

using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using Mountify.Data;
using Mountify.Services;
using Mountify.Utils;

namespace Mountify.Windows;

public class MountsWindow : Window, IDisposable {
    private Mountify plugin;
    private IDataManager dataManager;
    private IClientState clientState;

    private string filterText;
    private bool test;

    private ImageService imgService;
    private PlayerMountDB playerMounts;

    public MountsWindow(Mountify plugin, PluginServices services) : base("Mountify") {
        this.plugin = plugin;
        dataManager = services.dataManager;
        clientState = services.clientState;
        imgService = new ImageService(services.textureProvider);
        playerMounts = new PlayerMountDB(dataManager);

        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(400, 400),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
    }

    public override void OnOpen() {
        base.OnOpen();
        playerMounts.queueRefresh();
        playerMounts.getMounts();
        var currentMount = PluginUtils.getCurrentMount(clientState);
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
                var mountIcon = imgService.getIcon(mount.getIcon());
                if (ImGui.ImageButton(mountIcon.ImGuiHandle, new Vector2(45, 45)))
                    toggleMountWindow(mount);

                ImGui.SameLine();
                if (ImGui.Button(mount.getFormattedName(), new Vector2(150, 45)))
                    toggleMountWindow(mount);
                
                ImGui.NewLine();
            } catch (Exception e) {
                // ignored
            }
        }

        ImGui.EndChild();
    }

    private void toggleMountWindow(MountData mount) {
        plugin.toggleMountSettingsUi(mount);
    }

    public void Dispose() { }
}

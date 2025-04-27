using System;
using System.Text;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.Character;

namespace Mountify.Utils;

public class PluginUtils {
    public static bool needsRefresh(ref long lastRefresh, int refreshRate) {
        if (Environment.TickCount64 < lastRefresh)
            return false;

        lastRefresh = Environment.TickCount64 + refreshRate;
        return true;
    }

    public static string toTitleCaseExtended(String s, sbyte article) {
        if (article == 1) return string.Intern(s);

        var builder = new StringBuilder(s);
        var lastSpace = true;
        for (var i = 0; i < builder.Length; i++) {
            if (builder[i] == ' ')
                lastSpace = true;
            else if (lastSpace) {
                lastSpace = false;
                builder[i] = char.ToUpperInvariant(builder[i]);
            }
        }

        return string.Intern(builder.ToString());
    }

    public unsafe static uint getCurrentMount(IClientState client) {
        uint currentMount = 0;
        try {
            IGameObject? player = client.LocalPlayer;
            if (player == null)
                return 0;

            Character* native = (Character*)player.Address;
            if (native == null)
                return 0;

            if (!native->IsMounted())
                return 0;
            
            MountContainer? mount = native->Mount;
            currentMount = mount.Value.MountId;
        } catch (Exception e) {
            // ignored
        }
        return currentMount;
    }
}

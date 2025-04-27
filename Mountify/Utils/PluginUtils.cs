using System;
using System.Text;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.Object;

namespace Mountify.Utils;

public class Utils {
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

    public static IPlayerCharacter getPlayer(IClientState client) {
        IPlayerCharacter player = null;
        if (client.LocalPlayer != null)
            player = client.LocalPlayer;
        return player;
    }
}

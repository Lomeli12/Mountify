using System;
using System.Text;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using Mountify.Services;

namespace Mountify.Utils;

public class PluginUtils {
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

    public unsafe static uint getCurrentMount() {
        uint currentMount = 0;
        try {
            IGameObject? player = PluginServices.clientState.LocalPlayer;
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

    public static IDalamudTextureWrap getIcon(int iconID, bool isHQ = false) =>
        PluginServices.textureProvider.GetFromGameIcon(new GameIconLookup((uint)iconID, isHQ)).GetWrapOrEmpty();
}

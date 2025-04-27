using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Plugin.Services;

namespace Mountify.Services;

public class ImageService {
    private ITextureProvider textureProvider;

    public ImageService(ITextureProvider textureProvider) {
        this.textureProvider = textureProvider;
    }

    public IDalamudTextureWrap getIcon(int iconID, bool isHQ = false) =>
        textureProvider.GetFromGameIcon(new GameIconLookup((uint)iconID, isHQ)).GetWrapOrEmpty();
}

using Lumina.Excel.Sheets;
using Mountify.Utils;

namespace Mountify.Data;

public class MountData {
    public static MountData DUMMY_DATA = new MountData();

    private uint id;
    private uint bgmID;
    private ushort icon;
    private string name;
    private sbyte article;

    private MountData() {
        id = 0;
        icon = 0;
        name = "";
        article = 0;
    }

    public MountData(Mount mount) {
        id = mount.RowId;
        icon = mount.Icon;
        name = mount.Singular.ExtractText();
        article = mount.Article;
        bgmID = 0;
        if (mount.RideBGM.IsValid)
            bgmID = mount.RideBGM.Value.RowId;
    }

    public uint getID() => id;
    public ushort getIcon() => icon;
    public string getName() => name;
    public uint getBGMID() => bgmID;

    public string getFormattedName() => PluginUtils.toTitleCaseExtended(getName(), article);

    public override string ToString() => $"[MountData ID={id} BGM={bgmID} Icon={icon} Name={name} Article={article}]";
}

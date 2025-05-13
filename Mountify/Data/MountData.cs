using Lumina.Excel.Sheets;
using Mountify.Utils;
using Newtonsoft.Json;

namespace Mountify.Data;

public class MountData {
    public static MountData DUMMY_DATA = new MountData();

    [JsonProperty] private uint id;
    [JsonProperty] private uint bgmID;
    [JsonProperty] private bool bgmEnabled;

    [JsonIgnore] private ushort icon;
    [JsonIgnore] private string name;
    [JsonIgnore] private sbyte article;

    public MountData() {
        id = 0;
        icon = 0;
        bgmID = 0;
        bgmEnabled = true;
        name = "";
        article = 0;
    }

    public MountData(uint id) {
        this.id = id;
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

    public bool isBGMEnabled() => bgmEnabled;

    public void setBGMEnabled(bool enabled) => bgmEnabled = enabled;

    public string getFormattedName() => PluginUtils.toTitleCaseExtended(getName(), article);

    // Only copy if IDs match, hence safe copy
    public void safeCopyData(MountData mount) {
        if (mount.getID() != id)
            return;

        bgmID = mount.getBGMID();
        bgmEnabled = mount.isBGMEnabled();
    }

    public override string ToString() =>
        $"[MountData ID={id} BGM ID={bgmID} BGM Enabled={bgmEnabled} Icon={icon} Name={name} Article={article}]";
}

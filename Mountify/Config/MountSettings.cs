using Newtonsoft.Json;

namespace Mountify.Data;

public class MountSettings {
    
    [JsonProperty]
    private uint ID;
    
    [JsonProperty]
    private bool bgmEnabled;
    
    [JsonProperty]
    private uint bgmID;

    public MountSettings(uint ID, bool bgmEnabled) {
        this.ID = ID;
        this.bgmEnabled = bgmEnabled;
    }

    public uint getID() => ID;
    
    public bool isBGMEnabled() => bgmEnabled;
    
    public void setBGMEnabled(bool enabled) => bgmEnabled = enabled;
    public override string ToString() => $"[MountSettings ID={ID} BGMEnabled={bgmEnabled}]";
}

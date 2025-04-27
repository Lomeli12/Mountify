using System.Collections.Generic;
using Lumina.Excel.Sheets;

namespace SamplePlugin.Data;

public class MountDB {
    public List<Mount> mounts;
    public string dbPath;
    public Dictionary<int, List<Mount>> sameNumberMap;
    private static MountDB instance = new MountDB();

    public MountDB() {
        
    }
}

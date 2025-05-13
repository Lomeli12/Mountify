using System.Collections.Generic;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.Sheets;
using Mountify.Services;

namespace Mountify.Data;

public class PlayerMountDB(List<MountData> mounts) {
    private bool refreshQueued;

    public PlayerMountDB() : this([]) { }

    private unsafe void refreshMounts() {
        var player = PlayerState.Instance();
        mounts.Clear();
        mounts.AddRange(from mount in PluginServices.dataManager.GetExcelSheet<Mount>()
                        where player->IsMountUnlocked(mount.RowId)
                        select new MountData(mount));
    }

    public void moveIDToTop(uint id) {
        if (mounts.Count < 1) return;
        var mount = mounts.Find(mount => mount.getID() == id);
        if (mount == null) return;
        mounts.Remove(mount);
        mounts.Insert(0, mount);
    }

    public MountData? getMountByID(uint id) {
        return mounts.Find(mount => mount.getID() == id);
    }

    public List<MountData> getMounts() {
        if (refreshQueued) {
            refreshMounts();
            refreshQueued = false;
        }

        return mounts;
    }

    public void queueRefresh() => refreshQueued = true;
}

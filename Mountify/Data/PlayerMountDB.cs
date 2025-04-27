using System.Collections.Generic;
using System.Linq;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.Sheets;

namespace Mountify.Data;

public class PlayerMountDB(List<MountData> mounts, IDataManager dataManager) {
    private bool refreshQueued;

    public PlayerMountDB(IDataManager data) : this([], data) { }

    private unsafe void refreshMounts() {
        var player = PlayerState.Instance();
        mounts.Clear();
        mounts.AddRange(from mount in dataManager.GetExcelSheet<Mount>()
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

    public List<MountData> getMounts() {
        if (refreshQueued) {
            refreshMounts();
            refreshQueued = false;
        }

        return mounts;
    }

    public void queueRefresh() => refreshQueued = true;
}

using Dalamud.Game.ClientState.Conditions;
using Lumina.Excel.Sheets;

namespace Mountify.Services;

public class MountService {
    private static MountService instance;

    public static MountService initMountService(PluginServices services) {
        if (instance == null)
            instance = new MountService(services);

        return instance;
    }

    private MountService(PluginServices services) {
        services.condition.ConditionChange += onConditionChange;
    }

    public void onConditionChange(ConditionFlag flag, bool value) {
        
    }
}

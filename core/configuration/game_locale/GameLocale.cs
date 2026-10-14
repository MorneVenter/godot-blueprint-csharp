using System.Globalization;

namespace Game.Core.Configuration.GameLocale;

public partial class GameLocale : Node
{
    public static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    public GameLocale()
    {
        CultureInfo.DefaultThreadCurrentCulture = Culture;
        CultureInfo.DefaultThreadCurrentUICulture = Culture;
        CultureInfo.CurrentCulture = Culture;
        CultureInfo.CurrentUICulture = Culture;
    }
}

using System.Globalization;
using System.Runtime.CompilerServices;

namespace Game.Core.Configuration.GameLocale;

public static class GameLocale
{
    public static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [ModuleInitializer]
    internal static void Apply()
    {
        CultureInfo.DefaultThreadCurrentCulture = Culture;
        CultureInfo.DefaultThreadCurrentUICulture = Culture;
        CultureInfo.CurrentCulture = Culture;
        CultureInfo.CurrentUICulture = Culture;
    }
}

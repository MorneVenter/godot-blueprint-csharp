namespace Blueprint;

[Tool]
public partial class Blueprint : EditorPlugin
{
    public const string EncryptionKeySetting = "blueprint/save_manager/saves/save_encryption_key";
    public const string FpsCounterSetting = "blueprint/debug/performance/show_fps_counter";

    private const string SaveManagerSingletonName = "SaveManager";
    private const string UiHolderSingletonName = "UIHolder";
    private const string FpsCounterSingletonName = "FPSCounter";

    private const string SaveManagerPath = "res://addons/Blueprint/Autoloads/SaveManager.cs";
    private const string UiHolderPath = "res://addons/Blueprint/Autoloads/UIHolder.cs";
    private const string FpsCounterPath = "res://addons/Blueprint/Autoloads/FpsCounter.cs";

    public override void _EnterTree()
    {
        CreateSetting(
            EncryptionKeySetting,
            "save_encryption_key",
            new Dictionary
            {
                ["name"] = EncryptionKeySetting,
                ["type"] = (int)Variant.Type.String,
                ["hint"] = (int)PropertyHint.None,
                ["hint_string"] = "String",
            }
        );

        CreateSetting(
            FpsCounterSetting,
            false,
            new Dictionary
            {
                ["name"] = FpsCounterSetting,
                ["type"] = (int)Variant.Type.Bool,
                ["hint"] = (int)PropertyHint.None,
            }
        );
    }

    public override void _ExitTree()
    {
        ProjectSettings.SetSetting(EncryptionKeySetting, default(Variant));
        ProjectSettings.SetSetting(FpsCounterSetting, default(Variant));
    }

    public override void _EnablePlugin()
    {
        AddAutoloadSingleton(SaveManagerSingletonName, SaveManagerPath);
        AddAutoloadSingleton(UiHolderSingletonName, UiHolderPath);
        AddAutoloadSingleton(FpsCounterSingletonName, FpsCounterPath);
    }

    private static void CreateSetting(string name, Variant defaultValue, Dictionary propertyInfo)
    {
        if (ProjectSettings.HasSetting(name))
        {
            return;
        }

        ProjectSettings.SetSetting(name, defaultValue);
        ProjectSettings.AddPropertyInfo(propertyInfo);
        ProjectSettings.Save();
    }
}

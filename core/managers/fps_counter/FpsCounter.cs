namespace Game.Core.Managers.FpsCounter;

public partial class FpsCounter : Control
{
    private bool _enabled;
    private Label? _label;

    public override void _Ready()
    {
        _enabled = this.GetConfig().ShowFPS;
        ProcessMode = ProcessModeEnum.Always;

        if (_enabled)
            CreateFpsLabel();
    }

    public override void _Process(double delta)
    {
        UpdateFps();
    }

    public void Toggle(bool value)
    {
        GD.Print(value);

        if (value)
            CreateFpsLabel();
        else
        {
            _label?.QueueFree();
            _label = null;
        }

        _enabled = value;
    }

    private void CreateFpsLabel()
    {
        _label?.QueueFree();

        _label = new Label
        {
            Size = new Vector2(160, 32),
            Position = new Vector2(8, 8),
            Text = "0 FPS",
            ZIndex = 99,
            ProcessMode = ProcessModeEnum.Always,
        };

        _label.SetAnchorsPreset(LayoutPreset.TopLeft);
        AddChild(_label);
    }

    private void UpdateFps()
    {
        if (!_enabled || !GodotObject.IsInstanceValid(_label))
            return;

        _label!.Text = $"{Engine.GetFramesPerSecond():F1} FPS";
    }
}

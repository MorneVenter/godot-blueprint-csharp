namespace Game.Core.Managers.FpsCounter;

public partial class FpsCounter : Control
{
    private static readonly Vector2 LabelSize = new(160, 32);
    private static readonly Vector2 LabelPosition = new(8, 8);

    private Label? _label;
    private double _shownFps;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        Toggle(this.GetConfig().ShowFps);
    }

    public override void _Process(double delta)
    {
        var fps = Engine.GetFramesPerSecond();

        if (fps == _shownFps || _label is null)
            return;

        _shownFps = fps;
        _label.Text = string.Create(GameLocale.Culture, $"{fps:F1} FPS");
    }

    public void Toggle(bool enabled)
    {
        _label?.QueueFree();
        _label = enabled ? CreateLabel() : null;
        _shownFps = double.NaN;
        SetProcess(enabled);
    }

    private Label CreateLabel()
    {
        var label = new Label
        {
            Size = LabelSize,
            Position = LabelPosition,
            ZIndex = (int)RenderingServer.CanvasItemZMax,
            ProcessMode = ProcessModeEnum.Always,
        };

        label.SetAnchorsPreset(LayoutPreset.TopLeft);
        AddChild(label);

        return label;
    }
}

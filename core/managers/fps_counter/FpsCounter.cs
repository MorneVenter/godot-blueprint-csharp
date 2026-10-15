namespace Game.Core.Managers.FpsCounter;

public partial class FpsCounter : CanvasLayer
{
    private static readonly Vector2 LabelPosition = new(8, 8);

    private readonly Label _label = new() { Position = LabelPosition };
    private double _shownFps;

    public override void _Ready()
    {
        Layer = (int)RenderingServer.CanvasLayerMax;
        ProcessMode = ProcessModeEnum.Always;
        AddChild(_label);
        Toggle(this.GetConfig().ShowFps);
    }

    public override void _Process(double delta)
    {
        var fps = Engine.GetFramesPerSecond();

        if (fps == _shownFps)
            return;

        _shownFps = fps;
        _label.Text = string.Create(GameLocale.Culture, $"{fps:F1} FPS");
    }

    public void Toggle(bool enabled)
    {
        Visible = enabled;
        _shownFps = double.NaN;
        SetProcess(enabled);
    }
}

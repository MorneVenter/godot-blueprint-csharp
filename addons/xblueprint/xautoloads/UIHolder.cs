namespace Blueprint;

public partial class UIHolder : Control
{
    public enum UiLayer
    {
        WorldBackground = 0,
        WorldForeground = 1,
        Hud = 2,
        Menu = 3,
        MenuOverlay = 4,
    }

    private readonly Array<Control> _uiLayers = [];

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        foreach (var layer in Enum.GetValues<UiLayer>())
        {
            var zIndex = (int)layer + 1;

            var control = new Control
            {
                Name = $"UILayer{zIndex}",
                ZIndex = zIndex,
                MouseFilter = MouseFilterEnum.Pass,
            };

            control.SetAnchorsPreset(LayoutPreset.FullRect);

            AddChild(control);
            _uiLayers.Add(control);
        }
    }

    public void Add(Control uiItem, UiLayer layer)
    {
        var layerControl = GetLayer(layer);

        if (uiItem.GetParent() == layerControl)
            return;

        uiItem.Reparent(layerControl);
    }

    public static void Remove(Control uiItem)
    {
        uiItem.QueueFree();
    }

    public void Unparent(Control uiItem, UiLayer layer)
    {
        var layerControl = GetLayer(layer);

        if (!IsInstanceValid(uiItem) || uiItem.GetParent() != layerControl)
            return;

        layerControl.RemoveChild(uiItem);
        uiItem.RequestReady();
    }

    public void ToggleVisible(UiLayer layer, bool value)
    {
        GetLayer(layer).Visible = value;
    }

    private Control GetLayer(UiLayer layer)
    {
        var index = (int)layer;

        if (index < 0 || index >= _uiLayers.Count)
            throw new ArgumentOutOfRangeException(nameof(layer), layer, "Invalid UI layer.");

        return _uiLayers[index];
    }
}

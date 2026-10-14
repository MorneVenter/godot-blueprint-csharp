namespace Game.Core.Managers.UIHolder;

public partial class UIHolder : Control
{
    public enum UILayer
    {
        WorldBackground = 0,
        WorldForeground = 1,
        Hud = 2,
        Menu = 3,
        MenuOverlay = 4,
    }

    private readonly List<Control> _uiLayers = [];

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        foreach (var layer in Enum.GetValues<UILayer>())
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

    /// <summary>
    /// Adds the control to a layer, moving it there if it already has a parent.
    /// </summary>
    public void Add(Control uiItem, UILayer layer)
    {
        var layerControl = GetLayer(layer);
        var parent = uiItem.GetParent();

        if (parent == layerControl)
            return;

        if (parent is null)
            layerControl.AddChild(uiItem);
        else
            uiItem.Reparent(layerControl);
    }

    public static void Remove(Control uiItem)
    {
        uiItem.QueueFree();
    }

    public void Unparent(Control uiItem, UILayer layer)
    {
        var layerControl = GetLayer(layer);

        if (!IsInstanceValid(uiItem) || uiItem.GetParent() != layerControl)
            return;

        layerControl.RemoveChild(uiItem);
        uiItem.RequestReady();
    }

    public void ToggleVisible(UILayer layer, bool value)
    {
        GetLayer(layer).Visible = value;
    }

    private Control GetLayer(UILayer layer)
    {
        var index = (int)layer;

        if (index < 0 || index >= _uiLayers.Count)
            throw new ArgumentOutOfRangeException(nameof(layer), layer, "Invalid UI layer.");

        return _uiLayers[index];
    }
}

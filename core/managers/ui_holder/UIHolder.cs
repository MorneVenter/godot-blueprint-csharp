namespace Game.Core.Managers.UIHolder;

public partial class UIHolder : Node
{
    public enum UILayer
    {
        WorldBackground = 1,
        WorldForeground = 2,
        Hud = 3,
        Menu = 4,
        MenuOverlay = 5,
    }

    private readonly Dictionary<UILayer, CanvasLayer> _layers = [];

    public override void _Ready()
    {
        foreach (var layer in Enum.GetValues<UILayer>())
        {
            var canvasLayer = new CanvasLayer
            {
                Name = layer.ToString(),
                Layer = (int)layer,
                FollowViewportEnabled = layer is UILayer.WorldBackground or UILayer.WorldForeground,
            };

            AddChild(canvasLayer);
            _layers.Add(layer, canvasLayer);
        }
    }

    /// <summary>
    /// Adds the control to a layer, moving it there if it already has a parent.
    /// </summary>
    public void Add(Control uiItem, UILayer layer)
    {
        var target = _layers[layer];
        var parent = uiItem.GetParent();

        if (parent == target)
            return;

        if (parent is not null)
            Detach(uiItem, parent);

        target.AddChild(uiItem);
    }

    public void Unparent(Control uiItem)
    {
        if (
            IsInstanceValid(uiItem)
            && uiItem.GetParent() is CanvasLayer parent
            && parent.GetParent() == this
        )
            Detach(uiItem, parent);
    }

    public void ToggleVisible(UILayer layer, bool value)
    {
        _layers[layer].Visible = value;
    }

    private static void Detach(Control uiItem, Node parent)
    {
        parent.RemoveChild(uiItem);
        uiItem.RequestReady();
    }
}

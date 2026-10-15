namespace Game.Core.Extensions.AutoloadExtension;

public static class AutoloadExtension
{
    public static T GetAutoload<T>(this Node node)
        where T : Node
    {
        if (!Autoload<T>.IsRegistered)
        {
            throw new InvalidOperationException(
                $"{Autoload<T>.Name} is not an autoload: register it in the Godot project under that name."
            );
        }

        if (!node.IsInsideTree())
        {
            throw new InvalidOperationException(
                $"GetAutoload<{Autoload<T>.Name}> needs a node inside the scene tree."
            );
        }

        return node.GetNodeOrNull<T>(Autoload<T>.Path)
            ?? throw new InvalidOperationException(
                $"Autoload {Autoload<T>.Name} is not at {Autoload<T>.Path}: freed, or reached before it was added."
            );
    }

    private static class Autoload<T>
        where T : Node
    {
        public static readonly string Name = typeof(T).Name;
        public static readonly bool IsRegistered = ProjectSettings.HasSetting($"autoload/{Name}");
        public static readonly NodePath Path = $"/root/{Name}";
    }
}

using Godot;

namespace Project.Assets.Scenes.CustomNodes.SceneHolder
{
	[Tool]
	public partial class SceneHolder : SubViewportContainer
	{
		private const string ORDER_ERROR =
			"A Scene3D node should not be placed lower in the node tree than a Scene2D.";
		private const string NOT_FOUND_ERROR =
			"This node doesn't have a Scene2D or Scene3D as a child. Add the required nodes or things will get weird.";

		public override string[] _GetConfigurationWarnings()
		{
			var children = GetChildren();
			var hasScene3D = false;
			var hasScene2D = false;

			foreach (var child in children)
			{
				if (child is Scene3D)
				{
					hasScene3D = true;
					if (hasScene2D)
					{
						return new string[] { ORDER_ERROR };
					}
				}
				else if (child is Scene2D)
				{
					hasScene2D = true;
				}
			}

			return !hasScene2D && hasScene3D
				? (new string[] { NOT_FOUND_ERROR })
				: System.Array.Empty<string>();
		}
	}
}

using Godot;

namespace Project.Assets.Scenes.CustomNodes.SceneHolder
{
	[Tool]
	public partial class Scene2D : SubViewport
	{
		private const string BAD_PARENT_ERROR =
			"This nodes needs to be a child of a SceneHolder node.";

		public override string[] _GetConfigurationWarnings()
		{
			var parent = GetParent();
			return parent is not SceneHolder
				? (new string[] { BAD_PARENT_ERROR })
				: System.Array.Empty<string>();
		}
	}
}

namespace Game.Core.Utilities;

public static class RotationHelper
{
    /// <summary>
    /// Interpolates each axis the shortest way around. Angles are in radians.
    /// </summary>
    public static Vector3 LerpRotation(Vector3 from, Vector3 to, float weight) =>
        new(
            Mathf.LerpAngle(from.X, to.X, weight),
            Mathf.LerpAngle(from.Y, to.Y, weight),
            Mathf.LerpAngle(from.Z, to.Z, weight)
        );
}

namespace Blueprint;

public static class RotationHelper
{
    /// <summary>
    /// Linearly interpolates between two Vector3 rotations in radians.
    /// </summary>
    public static Vector3 LerpRotation(Vector3 from, Vector3 to, float lerpAmount) =>
        new(
            Mathf.LerpAngle(from.X, to.X, lerpAmount),
            Mathf.LerpAngle(from.Y, to.Y, lerpAmount),
            Mathf.LerpAngle(from.Z, to.Z, lerpAmount)
        );
}

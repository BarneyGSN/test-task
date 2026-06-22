namespace zad_test.Geometry;
using System.Numerics;
public class CoordinateTransformer
{
    public Matrix4x4 globalToLocal (Vector3 p1, Vector3 p2, out float length)
    {
        Vector3 direction = p2 - p1;
        length = direction.Length();
        Vector3 uz = Vector3.Normalize(direction);

        Vector3 reference = MathF.Abs(uz.X) < 0.9f ? Vector3.UnitX : Vector3.UnitY;
        Vector3 ux = Vector3.Cross(reference, uz);
        Vector3 uy = Vector3.Cross(uz, ux);

        Matrix4x4 rotation = new Matrix4x4(
            ux.X, uy.X, uz.X, 0,
            ux.Y, uy.Y, uz.Y, 0,
            ux.Z, uy.Z, uz.Z, 0,
            0, 0, 0, 1
        );
        
        Matrix4x4 translation = Matrix4x4.CreateTranslation(-p1);
        return Matrix4x4.Multiply(translation, rotation);
    }

    public Matrix4x4 LocalToGlobal(Matrix4x4 globalToLocal)
    {
        Matrix4x4.Invert(globalToLocal, out Matrix4x4 localToGlobal);
        return localToGlobal;
    }
}
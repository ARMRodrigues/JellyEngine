using System.Numerics;

namespace JellyEngine;

public struct AABB
{
    public Vector3 Min;
    public Vector3 Max;

    public AABB(Vector3 min, Vector3 max)
    {
        Min = min;
        Max = max;
    }

    public bool Contains(Vector3 point)
    {
        return point.X >= Min.X && point.X <= Max.X &&
               point.Y >= Min.Y && point.Y <= Max.Y &&
               point.Z >= Min.Z && point.Z <= Max.Z;
    }

    public bool Contains(AABB other)
    {
        return other.Min.X >= Min.X && other.Max.X <= Max.X &&
               other.Min.Y >= Min.Y && other.Max.Y <= Max.Y &&
               other.Min.Z >= Min.Z && other.Max.Z <= Max.Z;
    }

    public void Expand(Vector3 point)
    {
        Min = Vector3.Min(Min, point);
        Max = Vector3.Max(Max, point);
    }

    public bool IntersectsRay(Ray ray, out float tMin, out float tMax)
    {
        tMin = 0.0f;
        tMax = float.MaxValue;

        Vector3 invDir = new(
            ray.Direction.X != 0 ? 1.0f / ray.Direction.X : float.MaxValue,
            ray.Direction.Y != 0 ? 1.0f / ray.Direction.Y : float.MaxValue,
            ray.Direction.Z != 0 ? 1.0f / ray.Direction.Z : float.MaxValue
        );

        if (!IntersectAxis(Min.X, Max.X, ray.Origin.X, invDir.X, ref tMin, ref tMax) ||
            !IntersectAxis(Min.Y, Max.Y, ray.Origin.Y, invDir.Y, ref tMin, ref tMax) ||
            !IntersectAxis(Min.Z, Max.Z, ray.Origin.Z, invDir.Z, ref tMin, ref tMax))
        {
            return false;
        }

        return tMax >= Math.Max(tMin, 0.0f);
    }

    private static bool IntersectAxis(float min, float max, float origin, float invDir, ref float tMin, ref float tMax)
    {
        var t1 = (min - origin) * invDir;
        var t2 = (max - origin) * invDir;

        if (t1 > t2) (t1, t2) = (t2, t1);

        tMin = Math.Max(tMin, t1);
        tMax = Math.Min(tMax, t2);

        return tMax >= tMin;
    }

    public bool IntersectsRay(Ray ray, out Vector3 intersectionPoint)
    {
        if (IntersectsRay(ray, out var tMin, out _))
        {
            intersectionPoint = ray.Origin + ray.Direction * tMin;
            return true;
        }

        intersectionPoint = Vector3.Zero;
        return false;
    }
}

using System;

namespace JellyEngine;

public readonly struct SubMesh
{
    public readonly int IndexStart;
    public readonly int IndexCount;
    public readonly int MaterialId;

    public SubMesh(int indexStart, int indexCount, int materialId)
    {
        IndexStart = indexStart;
        IndexCount = indexCount;
        MaterialId = materialId;
    }
}
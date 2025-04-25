using System;

namespace JellyEngine;

public readonly struct MeshAsset(Mesh mesh, List<Material> materials, string filePath, string meshMaterialFilePath)
{
    public readonly Mesh Mesh = mesh;
    public readonly List<Material> Materials = materials;
    public readonly string FilePath = filePath;
    public readonly string MaterialFilePath = meshMaterialFilePath;
}

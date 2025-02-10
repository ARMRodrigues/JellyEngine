using System.Numerics;

namespace JellyEngine;

public class Mesh
{
    public List<Vector3> Positions { get; set; } = [];
    public List<Vector3> Normals { get; set; } = [];
    public List<Vector4> Tangents { get; set; } = [];
    public List<Vector4> Colors { get; set; } = [];
    public List<Vector2> UV0 { get; set; } = [];
    public List<Vector2> UV1 { get; set; } = [];
    public float[] Vertices => GetCombinedData();
    public uint[] Indices { get; set; } = [];

    public float[] GetCombinedData()
    {
        int vertexCount = Positions.Count;
        // 3 (Position) + 3 (Normal) + 4 (Tangent) + 4 (Color) + 2 (UV0) + 2 (UV1)
        int dataSize = vertexCount * (3 + 3 + 4 + 4 + 2 + 2);

        var combinedData = new List<float>(dataSize);

        for (int i = 0; i < vertexCount; ++i)
        {
            // Position (3 floats)
            combinedData.Add(Positions[i].X);
            combinedData.Add(Positions[i].Y);
            combinedData.Add(Positions[i].Z);

            // Normal (3 floats)
            combinedData.Add(Normals[i].X);
            combinedData.Add(Normals[i].Y);
            combinedData.Add(Normals[i].Z);

            // Tangent (4 floats)
            combinedData.Add(Tangents[i].X);
            combinedData.Add(Tangents[i].Y);
            combinedData.Add(Tangents[i].Z);
            combinedData.Add(Tangents[i].W); // Handedness (if present)

            // Color (4 floats)
            combinedData.Add(Colors[i].X);
            combinedData.Add(Colors[i].Y);
            combinedData.Add(Colors[i].Z);
            combinedData.Add(Colors[i].W);

            // UV0 (2 floats)
            combinedData.Add(UV0[i].X);
            combinedData.Add(UV0[i].Y);

            // UV1 (2 floats)
            combinedData.Add(UV1[i].X);
            combinedData.Add(UV1[i].Y);
        }

        return [.. combinedData];
    }
}
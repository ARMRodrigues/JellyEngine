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
    public List<SubMesh> SubMeshes { get; set; } = [];
    public float[] GetCombinedData()
    {
        int vertexCount = Positions.Count;
        // 3 (Position) + 3 (Normal) + 4 (Tangent) + 4 (Color) + 2 (UV0) + 2 (UV1)
        int dataSize = vertexCount * (3 + 3 + 4 + 4 + 2 + 2);

        var combinedData = new List<float>(dataSize);

        for (int i = 0; i < vertexCount; ++i)
        {
            // Position (3 floats) - Obrigatório
            combinedData.Add(Positions[i].X);
            combinedData.Add(Positions[i].Y);
            combinedData.Add(Positions[i].Z);

            // Normal (3 floats) - Preenche com (0, 0, 0) se não houver dados
            if (i < Normals.Count)
            {
                combinedData.Add(Normals[i].X);
                combinedData.Add(Normals[i].Y);
                combinedData.Add(Normals[i].Z);
            }
            else
            {
                combinedData.Add(0); // X
                combinedData.Add(0); // Y
                combinedData.Add(0); // Z
            }

            // Tangent (4 floats) - Preenche com (0, 0, 0, 0) se não houver dados
            if (i < Tangents.Count)
            {
                combinedData.Add(Tangents[i].X);
                combinedData.Add(Tangents[i].Y);
                combinedData.Add(Tangents[i].Z);
                combinedData.Add(Tangents[i].W);
            }
            else
            {
                combinedData.Add(0); // X
                combinedData.Add(0); // Y
                combinedData.Add(0); // Z
                combinedData.Add(0); // W
            }

            // Color (4 floats) - Preenche com (1, 1, 1, 1) se não houver dados (cor branca)
            if (i < Colors.Count)
            {
                combinedData.Add(Colors[i].X);
                combinedData.Add(Colors[i].Y);
                combinedData.Add(Colors[i].Z);
                combinedData.Add(Colors[i].W);
            }
            else
            {
                combinedData.Add(1); // R
                combinedData.Add(1); // G
                combinedData.Add(1); // B
                combinedData.Add(1); // A
            }

            // UV0 (2 floats) - Preenche com (0, 0) se não houver dados
            if (i < UV0.Count)
            {
                combinedData.Add(UV0[i].X);
                combinedData.Add(UV0[i].Y);
            }
            else
            {
                combinedData.Add(0); // U
                combinedData.Add(0); // V
            }

            // UV1 (2 floats) - Preenche com (0, 0) se não houver dados
            if (i < UV1.Count)
            {
                combinedData.Add(UV1[i].X);
                combinedData.Add(UV1[i].Y);
            }
            else
            {
                combinedData.Add(0); // U
                combinedData.Add(0); // V
            }
        }

        return combinedData.ToArray();
    }

    public IReadOnlyList<SubMesh> GetSubMeshesOrDefault()
    {
        if (SubMeshes.Count == 0)
        {
            return new List<SubMesh>
            {
                new SubMesh(0, Indices.Length, 0)
            };
        }

        return SubMeshes;
    }
}
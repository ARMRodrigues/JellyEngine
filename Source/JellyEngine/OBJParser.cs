using System;
using System.Globalization;
using System.Numerics;

namespace JellyEngine;

public class OBJParser
{
    public static MeshAsset Load(string objPath)
    {
        var mesh = new Mesh();
        var materials = new List<Material>();
        var materialMap = new Dictionary<string, int>();

        var positions = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        var currentIndices = new List<uint>();
        int currentMaterialId = 0;
        int indexOffset = 0;

        string? materialLibPath = null;
        string? currentMaterialName = null;

        using var reader = new StreamReader(objPath);
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

            string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            switch (parts[0])
            {
                case "mtllib":
                    if (parts.Length > 1)
                        materialLibPath = Path.Combine(Path.GetDirectoryName(objPath)!, parts[1]);
                    break;

                case "usemtl":
                    if (currentIndices.Count > 0)
                    {
                        mesh.SubMeshes.Add(new SubMesh(indexOffset, currentIndices.Count, currentMaterialId));
                        indexOffset += currentIndices.Count;
                        mesh.Indices = mesh.Indices.Concat(currentIndices).ToArray();
                        currentIndices.Clear();
                    }

                    currentMaterialName = parts[1];
                    if (!materialMap.TryGetValue(currentMaterialName, out currentMaterialId))
                    {
                        currentMaterialId = materials.Count;
                        materials.Add(new Material(currentMaterialName));
                        materialMap[currentMaterialName] = currentMaterialId;
                    }
                    break;

                case "v":
                    positions.Add(ParseVector3(parts));
                    break;

                case "vt":
                    uvs.Add(ParseVector2(parts));
                    break;

                case "vn":
                    normals.Add(ParseVector3(parts));
                    break;

                case "f":
                    if (parts.Length < 4) break; // ignora se a face não for ao menos um triângulo

                    for (int i = 2; i < parts.Length; i++)
                    {
                        int[] v0 = ParseFaceVertex(parts[1], positions, uvs, normals, mesh);
                        int[] v1 = ParseFaceVertex(parts[i - 1], positions, uvs, normals, mesh);
                        int[] v2 = ParseFaceVertex(parts[i], positions, uvs, normals, mesh);

                        currentIndices.Add((uint)v0[0]);
                        currentIndices.Add((uint)v1[0]);
                        currentIndices.Add((uint)v2[0]);
                    }
                    break;
            }
        }

        // Último submesh
        if (currentIndices.Count > 0)
        {
            mesh.SubMeshes.Add(new SubMesh(indexOffset, currentIndices.Count, currentMaterialId));
            mesh.Indices = mesh.Indices.Concat(currentIndices).ToArray();
        }

        // Carrega materiais se o arquivo foi especificado
        if (materialLibPath != null && File.Exists(materialLibPath))
        {
            LoadMaterials(materialLibPath, materials, materialMap);
        }

        return new MeshAsset(mesh, materials, objPath, materialLibPath);
    }

    private static void LoadMaterials(string mtlPath, List<Material> materials, Dictionary<string, int> materialMap)
    {
        using var reader = new StreamReader(mtlPath);
        Material? current = null;

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

            string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            switch (parts[0])
            {
                case "newmtl":
                    current = new Material(parts[1]);
                    if (materialMap.TryGetValue(parts[1], out int id))
                        materials[id] = current;
                    break;

                case "Kd":
                    if (current != null && parts.Length >= 4)
                    {
                        Vector3 colorVec = ParseVector3(parts);
                        current.Color = new Color(colorVec.X, colorVec.Y, colorVec.Z);
                        Console.WriteLine($"Minha cor é: {current.Color.ToVector3()}");
                    }
                    break;

                case "map_Kd":
                    if (current != null)
                    {
                        var texturePath = Path.Combine(Path.GetDirectoryName(mtlPath)!, parts[1]);
                        current.Albedo = new Texture(texturePath);
                    }
                    break;
            }
        }
    }

    private static Vector3 ParseVector3(string[] parts)
    {
        return new Vector3(
            float.Parse(parts[1], CultureInfo.InvariantCulture),
            float.Parse(parts[2], CultureInfo.InvariantCulture),
            float.Parse(parts[3], CultureInfo.InvariantCulture));
    }

    private static Vector2 ParseVector2(string[] parts)
    {
        return new Vector2(
            float.Parse(parts[1], CultureInfo.InvariantCulture),
            float.Parse(parts[2], CultureInfo.InvariantCulture));
    }

    private static int ParseIndex(string token, int count)
    {
        try
        {
            int index = int.Parse(token, CultureInfo.InvariantCulture);
            return index < 0 ? count + index : index - 1;
        }
        catch
        {
            throw new FormatException($"Invalid index: '{token}'");
        }
    }

    private static int[] ParseFaceVertex(string facePart, List<Vector3> positions, List<Vector2> uvs, List<Vector3> normals, Mesh mesh)
    {
        string[] indices = facePart.Split('/');
        int posIdx = ParseIndex(indices[0], positions.Count);
        int uvIdx = indices.Length > 1 && indices[1] != "" ? ParseIndex(indices[1], uvs.Count) : -1;
        int normIdx = indices.Length > 2 ? ParseIndex(indices[2], normals.Count) : -1;

        mesh.Positions.Add(positions[posIdx]);
        mesh.UV0.Add(uvIdx >= 0 ? uvs[uvIdx] : Vector2.Zero);
        mesh.Normals.Add(normIdx >= 0 ? normals[normIdx] : Vector3.Zero);
        mesh.Colors.Add(Vector4.One);
        mesh.Tangents.Add(Vector4.Zero);

        return new int[] { mesh.Positions.Count - 1 };
    }
}

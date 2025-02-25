using System.Numerics;
using JellyEngine;

namespace JellyGame.Scenes.Terrain;

public class IslandGenerator
{
    private static readonly Color[] TERRAIN_COLS = new Color[]
    {
        new Color(201, 178, 99),  // Areia
        new Color(135, 184, 82),  // Grama clara
        new Color(80, 171, 93),   // Grama escura
        new Color(120, 120, 120), // Rocha
        new Color(200, 200, 210)  // Neve
    };

    public int GridSize { get; set; } = 100;
    public float HeightMultiplier { get; set; } = 10f;
    public float EdgeHeight { get; set; } = -3f;

    private HeightmapGenerator _heightmapGenerator { get; set; }
    private ColorGenerator _colorGenerator { get; set; }

    public IslandGenerator()
    {
        var noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        noise.SetFrequency(0.055f);
        noise.SetFractalOctaves(5);

        _heightmapGenerator = new HeightmapGenerator(GridSize, noise);
        _colorGenerator = new ColorGenerator(TERRAIN_COLS, 0.45f);
    }

    public Mesh GenerateTopMesh()
    {
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        var indices = new List<uint>();
        var heightMap = _heightmapGenerator.HeightMap;
        var colourMap = _colorGenerator.GenerateColours(heightMap, HeightMultiplier);
        var colors = new List<Vector4>();

        var offsetX = (GridSize - 1) / -2f;
        var offsetZ = (GridSize - 1) / -2f;

        for (int i = 0; i < 5; i++) // Testa algumas posições aleatórias
{
    Console.WriteLine($"Color {i}: {colourMap[i, i].ToVector4()}");
}


        for (var z = 0; z < GridSize; z++)
        {
            for (var x = 0; x < GridSize; x++)
            {
                var v0 = new Vector3(x + offsetX, _heightmapGenerator.FallOffMap[x, z] * HeightMultiplier, z + offsetZ);
                var v1 = new Vector3(x + 1 + offsetX, _heightmapGenerator.FallOffMap[x + 1, z] * HeightMultiplier, z + offsetZ);
                var v2 = new Vector3(x + offsetX, _heightmapGenerator.FallOffMap[x, z + 1] * HeightMultiplier, z + 1 + offsetZ);
                var v3 = new Vector3(x + 1 + offsetX, _heightmapGenerator.FallOffMap[x + 1, z + 1] * HeightMultiplier, z + 1 + offsetZ);

                // Pegando cores dos vértices
                var c0 = colourMap[x, z];
                var c1 = colourMap[x + 1, z];
                var c2 = colourMap[x, z + 1];
                var c3 = colourMap[x + 1, z + 1];

                // Cor média de cada triângulo
                var color1 = (c0 + c1 + c2) / 3f;
                var color2 = (c1 + c2 + c3) / 3f;

                AddTriangle(vertices, normals, colors, uvs, v0, v2, v1, c0, c2, c1);
                AddTriangle(vertices, normals, colors, uvs, v1, v2, v3, c1, c2, c3);
            }
        }


        var mesh = new Mesh()
        {
            Positions = vertices,
            Normals = normals,
            Colors = colors,
            UV0 = uvs,
            Indices = Enumerable.Range(0, vertices.Count).Select(i => (uint)i).ToArray(), // Cada triângulo tem seus próprios vértices
        };

        return mesh;
    }

    private void AddTriangle(List<Vector3> vertices, List<Vector3> normals, List<Vector4> colors, List<Vector2> uvs,
        Vector3 v0, Vector3 v1, Vector3 v2, Color c1, Color c2, Color c3)
    {
        // Calcular a normal do triângulo
        var normal = Vector3.Normalize(Vector3.Cross(v1 - v0, v2 - v0));

        // Adicionar os vértices (cada triângulo tem seus próprios vértices, sem compartilhamento)
        vertices.Add(v0);
        vertices.Add(v1);
        vertices.Add(v2);

        // Adicionar normais (mesma normal para todos os três vértices do triângulo)
        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);

        colors.Add(c1.ToVector4());
        colors.Add(c2.ToVector4());
        colors.Add(c3.ToVector4());

        // Adicionar UVs
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 1));
    }

    public Mesh GenerateBorderMesh()
    {
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        var indices = new List<uint>();

        var offsetX = (GridSize - 1) / -2f;
        var offsetZ = (GridSize - 1) / -2f;

        // Criar a base
        for (var z = 0; z < GridSize; z++)
        {
            for (var x = 0; x < GridSize; x++)
            {
                var heightA = _heightmapGenerator.FallOffMap[x, z] * HeightMultiplier;
                var heightB = _heightmapGenerator.FallOffMap[x, z + 1] * HeightMultiplier;
                var heightC = _heightmapGenerator.FallOffMap[x + 1, z] * HeightMultiplier;
                var heightD = _heightmapGenerator.FallOffMap[x + 1, z + 1] * HeightMultiplier;

                if (x == 0)
                {
                    var rightHeight = _heightmapGenerator.FallOffMap[x, z] * HeightMultiplier;
                    var leftHeight = _heightmapGenerator.FallOffMap[x, z + 1] * HeightMultiplier;

                    var a = new Vector3(x + offsetX, heightA, z + offsetZ);
                    var b = new Vector3(x + offsetX, EdgeHeight, z + offsetZ);
                    var c = new Vector3(x + offsetX, heightB, z + offsetZ + 1);
                    var d = new Vector3(x + offsetX, EdgeHeight, z + offsetZ + 1);

                    var v = new Vector3[] { a, b, c, b, d, c };
                    var uv = new Vector2[] { new(0, 1), new(0, 0), new(1, 1), new(0, 0), new(1, 0), new(1, 1) };

                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        uvs.Add(uv[k]);
                        indices.Add((uint)indices.Count);
                    }
                }
                if (x == GridSize - 1)
                {
                    var a = new Vector3(x + offsetX + 1, heightC, z + offsetZ);
                    var b = new Vector3(x + offsetX + 1, EdgeHeight, z + offsetZ);
                    var c = new Vector3(x + offsetX + 1, heightD, z + offsetZ + 1);
                    var d = new Vector3(x + offsetX + 1, EdgeHeight, z + offsetZ + 1);

                    var v = new Vector3[] { a, c, b, b, c, d };
                    var uv = new Vector2[] { new(0, 1), new(1, 1), new(0, 0), new(0, 0), new(1, 1), new(1, 0) };

                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        uvs.Add(uv[k]);
                        indices.Add((uint)indices.Count);
                    }
                }
                if (z == 0)
                {
                    var a = new Vector3(x + offsetX, heightA, z + offsetZ);
                    var b = new Vector3(x + offsetX, EdgeHeight, z + offsetZ);
                    var c = new Vector3(x + offsetX + 1, heightC, z + offsetZ);
                    var d = new Vector3(x + offsetX + 1, EdgeHeight, z + offsetZ);

                    var v = new Vector3[] { a, c, b, b, c, d };
                    var uv = new Vector2[] { new(0, 1), new(1, 1), new(0, 0), new(0, 0), new(1, 1), new(1, 0) };

                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        uvs.Add(uv[k]);
                        indices.Add((uint)indices.Count);
                    }
                }
                if (z == GridSize - 1)
                {
                    var a = new Vector3(x + offsetX, heightB, z + offsetZ + 1);
                    var b = new Vector3(x + offsetX, EdgeHeight, z + offsetZ + 1);
                    var c = new Vector3(x + offsetX + 1, heightD, z + offsetZ + 1);
                    var d = new Vector3(x + offsetX + 1, EdgeHeight, z + offsetZ + 1);

                    var v = new Vector3[] { a, b, c, b, d, c };
                    var uv = new Vector2[] { new(0, 1), new(0, 0), new(1, 1), new(0, 0), new(1, 0), new(1, 1) };

                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        uvs.Add(uv[k]);
                        indices.Add((uint)indices.Count);
                    }
                }
            }
        }

        var mesh = new Mesh()
        {
            Positions = vertices,
            Normals = normals,
            UV0 = uvs,
            Indices = indices.ToArray(),
        };

        return mesh;
    }

    public Mesh GenerateBottomMesh()
    {
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        var indices = new List<uint>();

        var offsetX = (GridSize - 1) / -2f;
        var offsetZ = (GridSize - 1) / -2f;

        for (var z = 0; z <= GridSize; z++)
        {
            for (var x = 0; x <= GridSize; x++)
            {
                vertices.Add(new Vector3(x + offsetX, EdgeHeight, z + offsetZ));
                normals.Add(new Vector3(0, 1, 0));
                uvs.Add(new Vector2((float)x / GridSize, (float)z / GridSize));
            }
        }

        for (var z = 0; z < GridSize; z++)
        {
            for (var x = 0; x < GridSize; x++)
            {
                var topLeft = (uint)(z * (GridSize + 1) + x);
                var topRight = topLeft + 1;
                var bottomLeft = topLeft + (uint)(GridSize + 1);
                var bottomRight = bottomLeft + 1;

                indices.Add(topLeft);
                indices.Add(topRight);
                indices.Add(bottomRight);

                indices.Add(topLeft);
                indices.Add(bottomRight);
                indices.Add(bottomLeft);
            }
        }

        var mesh = new Mesh()
        {
            Positions = vertices,
            Normals = normals,
            UV0 = uvs,
            Indices = [.. indices]
        };

        return mesh;
    }
}
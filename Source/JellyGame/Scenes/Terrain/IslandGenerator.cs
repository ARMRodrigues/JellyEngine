using System.Numerics;
using JellyEngine;

namespace JellyGame.Scenes.Terrain;

public class IslandGenerator
{
    public int GridSize { get; set; } = 50;
    public float NoiseThreshold { get; set; } = 0.3f;

    private HeightmapGenerator _heightmapGenerator { get; set; }

    private Dictionary<(int, int), Vector3> topVertices = [];

    public IslandGenerator()
    {
        var noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        noise.SetFrequency(0.055f);
        noise.SetFractalOctaves(5);
        _heightmapGenerator = new HeightmapGenerator(GridSize, noise);
    }

    public Mesh GenerateMesh()
    {
        var vertices = new List<Vector3>();
        var uvs = new List<Vector2>();

        var vertCountX = GridSize + 1;
        var vertCountZ = GridSize + 1;

        for (var z = 0; z < vertCountZ; z++)
        {
            for (var x = 0; x < vertCountX; x++)
            {
                var y = _heightmapGenerator.FallOffMap[x, z];

                if (y > NoiseThreshold)
                    y *= 16f;

                vertices.Add(new Vector3(x, y, z));
                topVertices[(x, z)] = new Vector3(x, y * 16f, z);

                float u = (float)x / GridSize;
                float v = 1.0f - ((float)z / GridSize); // Inverte o V
                uvs.Add(new Vector2(u, v));
            }
        }

        var indices = new List<uint>();

        for (var z = 0; z < GridSize; z++)
        {
            for (var x = 0; x < GridSize; x++)
            {
                var topLeft = (uint)(z * vertCountX + x);
                var topRight = topLeft + 1;
                var bottomLeft = (uint)(topLeft + vertCountX);
                var bottomRight = (uint)(bottomLeft + 1);

                indices.Add(topLeft);
                indices.Add(bottomRight);
                indices.Add(topRight);

                indices.Add(topLeft);
                indices.Add(bottomLeft);
                indices.Add(bottomRight);
            }
        }

        var mesh = new Mesh()
        {
            Positions = vertices,
            UV0 = uvs,
            Indices = indices.ToArray(),
        };

        return mesh;
    }

    public Mesh GenerateMeshV2()
    {
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        var indices = new List<uint>();

        for (var z = 0; z < GridSize; z++)
        {
            for (var x = 0; x < GridSize; x++)
            {
                // Criar os 4 vértices do quadrado
                var v0 = new Vector3(x, _heightmapGenerator.FallOffMap[x, z] * 16f, z);
                var v1 = new Vector3(x + 1, _heightmapGenerator.FallOffMap[x + 1, z] * 16f, z);
                var v2 = new Vector3(x, _heightmapGenerator.FallOffMap[x, z + 1] * 16f, z + 1);
                var v3 = new Vector3(x + 1, _heightmapGenerator.FallOffMap[x + 1, z + 1] * 16f, z + 1);

                AddTriangle(vertices, normals, uvs, v0, v2, v1);
                AddTriangle(vertices, normals, uvs, v1, v2, v3);

            }
        }

        var mesh = new Mesh()
        {
            Positions = vertices,
            Normals = normals,  // Agora temos normais calculadas por face
            UV0 = uvs,
            Indices = Enumerable.Range(0, vertices.Count).Select(i => (uint)i).ToArray(), // Cada triângulo tem seus próprios vértices
        };

        return mesh;
    }

    private void AddTriangle(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, Vector3 v0, Vector3 v1, Vector3 v2)
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

        float baseY = -0.5f; // Valor base para a borda
        var vertCountX = GridSize + 1;
        var vertCountZ = GridSize + 1;

        // Criar a base
        for (var z = 0; z < GridSize; z++)
        {
            for (var x = 0; x < GridSize; x++)
            {
                var baseHeight = -0.5f * 3f;

                var heightA = _heightmapGenerator.FallOffMap[x, z] * 16f;
                var heightB = _heightmapGenerator.FallOffMap[x, z + 1] * 16f;
                var heightC = _heightmapGenerator.FallOffMap[x + 1, z] * 16f;
                var heightD = _heightmapGenerator.FallOffMap[x + 1, z + 1] * 16f;

                if (x == 0)
                {
                    var rightHeight = _heightmapGenerator.FallOffMap[x, z] * 16f;
                    var leftHeight = _heightmapGenerator.FallOffMap[x, z + 1] * 16f;

                    var a = new Vector3(x, rightHeight, z);
                    var b = new Vector3(x, baseHeight, z);
                    var c = new Vector3(x, leftHeight, z + 1);
                    var d = new Vector3(x, baseHeight, z + 1);

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
                    var a = new Vector3(x + 1, heightC, z);
                    var b = new Vector3(x + 1, baseHeight, z);
                    var c = new Vector3(x + 1, heightD, z + 1);
                    var d = new Vector3(x + 1, baseHeight, z + 1);

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
                    var a = new Vector3(x, heightA, z);
                    var b = new Vector3(x, baseHeight, z);
                    var c = new Vector3(x + 1, heightC, z);
                    var d = new Vector3(x + 1, baseHeight, z);

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
                    var a = new Vector3(x, heightB, z + 1);
                    var b = new Vector3(x, baseHeight, z + 1);
                    var c = new Vector3(x + 1, heightD, z + 1);
                    var d = new Vector3(x + 1, baseHeight, z + 1);

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

    public Mesh GenerateMirrorMesh()
    {
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        var indices = new List<uint>();

        bool mirror = true;


        var noise = new FastNoiseLite();
        noise.SetFrequency(0.27f);
        var newHeightMap = new HeightmapGenerator(GridSize, noise);

        for (var z = 0; z < GridSize; z++)
        {
            for (var x = 0; x < GridSize; x++)
            {
                // Criar os 4 vértices do quadrado
                var v0 = new Vector3(x, newHeightMap.FallOffMap[x, z] * 16f, z);
                var v1 = new Vector3(x + 1, newHeightMap.FallOffMap[x + 1, z] * 16f, z);
                var v2 = new Vector3(x, newHeightMap.FallOffMap[x, z + 1] * 16f, z + 1);
                var v3 = new Vector3(x + 1, newHeightMap.FallOffMap[x + 1, z + 1] * 16f, z + 1);

               // Se mirrorDown for true, espelha os vértices ao longo do eixo Y (inverte Y)
            if (mirror)
            {
                v0.Y = -v0.Y;
                v1.Y = -v1.Y;
                v2.Y = -v2.Y;
                v3.Y = -v3.Y;
            }

            AddTriangle(vertices, normals, uvs, v0, v1, v2);
                AddTriangle(vertices, normals, uvs, v1, v3, v2);
            }
        }

        var mesh = new Mesh()
        {
            Positions = vertices,
            Normals = normals,  // Agora temos normais calculadas por face
            UV0 = uvs,
            Indices = Enumerable.Range(0, vertices.Count).Select(i => (uint)i).ToArray(), // Cada triângulo tem seus próprios vértices
        };

        return mesh;
    }

    // Função para calcular a normal a partir de 3 vértices
    private Vector3 CalculateNormal(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        var edge1 = v1 - v0;
        var edge2 = v2 - v0;

        return new Vector3(
            edge1.Y * edge2.Z - edge1.Z * edge2.Y,
            edge1.Z * edge2.X - edge1.X * edge2.Z,
            edge1.X * edge2.Y - edge1.Y * edge2.X
        ); // Normal não normalizada
    }
}
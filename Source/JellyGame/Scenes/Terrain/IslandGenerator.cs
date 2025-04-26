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

    public int GridSize { get; set; } = 200;
    public int Width { get; set; } = 600;
    public int Height { get; set; } = 600;
    public float HeightMultiplier { get; set; } = 16f;
    public float EdgeHeight { get; set; } = -3f;
    
    public Terrain Terrain { get; set; }

    private HeightmapGenerator _heightmapGenerator { get; set; }
    private ColorGenerator _colorGenerator { get; set; }
    private Curve _curve;

    public IslandGenerator()
    {
        var anotherSeed = new Random().Next();

        Terrain = new Terrain()
        {
            Seed = anotherSeed,
            Width = Width,
            Height = HeightMultiplier,
            Depth = Height,
            NoiseScale = 250f,
            Octaves = 5,
            Lacunarity = 0.35f,
            Persistance = 0.3f
        };

        _heightmapGenerator = new HeightmapGenerator(Terrain);
        _colorGenerator = new ColorGenerator(TERRAIN_COLS, 0.45f);
        
        _curve = new Curve(
            new CurveKey(0.0f, 0.0f, 0.0f, 0.0f),   // Início (plano)
            new CurveKey(0.4f, 0.02f, 0.0f, 0.1f),  // Baixo, mas não zero
            new CurveKey(0.8f, 0.5f, 0.3f, 0.3f), 
            new CurveKey(1.0f, 1.0f, 0.5f, 0.0f)    // Suavemente subindo
        );
        
        Console.WriteLine(_curve.Evaluate(0.4f));
    }

    public Mesh GenerateTopMesh()
    {
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        var indices = new List<uint>();
        var heightMap = _heightmapGenerator.FallOffMap;
        var biomeMap = _heightmapGenerator.BiomeMap;
        var colourMap = _colorGenerator.GenerateColours(heightMap, HeightMultiplier);
        var colors = new List<Vector4>();

        var offsetX = (Width - 1) / -2f;
        var offsetZ = (Height - 1) / -2f;

        for (int i = 0; i < 5; i++) // Testa algumas posições aleatórias
        {
            Console.WriteLine($"Color {i}: {colourMap[i, i].ToVector4()}");
        }
        
        for (var z = 0; z < Height; z++)
        {
            for (var x = 0; x < Width; x++)
            {
                var height1 = heightMap[x, z];
                var height2 = heightMap[x + 1, z];
                var height3 = heightMap[x, z + 1];
                var height4 = heightMap[x + 1, z + 1];
                
                var c0 = GetColor(height1);
                var c1 = GetColor(height2);
                var c2 = GetColor(height3);
                var c3 = GetColor(height4);
                
                /*height1 = _curve.Evaluate(height1);
                height2 = _curve.Evaluate(height2);
                height3 = _curve.Evaluate(height3);
                height4 = _curve.Evaluate(height4);*/
                
                
                var v0 = new Vector3(x + offsetX, AdjustMountain(height1) * HeightMultiplier, z + offsetZ);
                var v1 = new Vector3(x + 1 + offsetX, AdjustMountain(height2) * HeightMultiplier, z + offsetZ);
                var v2 = new Vector3(x + offsetX, AdjustMountain(height3) * HeightMultiplier, z + 1 + offsetZ);
                var v3 = new Vector3(x + 1 + offsetX, AdjustMountain(height4) * HeightMultiplier, z + 1 + offsetZ);

                // Pegando cores dos vértices
                /*var c0 = colourMap[x, z];
                var c1 = colourMap[x + 1, z];
                var c2 = colourMap[x, z + 1];
                var c3 = colourMap[x + 1, z + 1];*/
                
                

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
    
    float EvaluateCurve(float t)
    {
        // Curva tipo Seno suave, para garantir uma transição suave entre 0.4 e 0.426
        return 0.4f + (float)(Math.Sin(t * Math.PI * 0.5f) * (0.426f - 0.4f));  // Ajusta entre 0.4 e 0.426
    }
    
    float AdjustMountain(float height)
    {
        var mountainStarterd = 0.650f;
        
       if (height >= mountainStarterd)
       {
            // Mantém os picos ajustados corretamente
            float factor = (height - mountainStarterd) / 0.2f;
            return mountainStarterd+ (float)(Math.Sin(factor * Math.PI * 0.5f) * 1.5f);
       }

       var interval = 0.4f;
       if (height < interval)
       {
           var newInter = ((height / interval) - 0.5f) * (interval * 2f);
           return newInter;
       }
       
       return height;
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

        var offsetX = (Width - 1) / -2f;
        var offsetZ = (Height - 1) / -2f;

        // Criar a base
        for (var z = 0; z < Height; z++)
        {
            for (var x = 0; x < Width; x++)
            {
                var heightA = _heightmapGenerator.FallOffMap[x, z];
                var heightB = _heightmapGenerator.FallOffMap[x, z + 1];
                var heightC = _heightmapGenerator.FallOffMap[x + 1, z];
                var heightD = _heightmapGenerator.FallOffMap[x + 1, z + 1] ;

                heightA = _curve.Evaluate(heightA) * HeightMultiplier;
                heightB = _curve.Evaluate(heightB) * HeightMultiplier;
                heightC = _curve.Evaluate(heightC) * HeightMultiplier;
                heightD = _curve.Evaluate(heightD) * HeightMultiplier;

                if (x == 0)
                {
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
                if (x == Width - 1)
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
                if (z == Height - 1)
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

    public Texture GenerateNoiseTexture()
    {
        int width = Width;
        int height = Height;
    
        var pixels = new Color[width * height];
        
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var noiseValue = _heightmapGenerator.FallOffMap[x,y];
;                //noiseValue = MathUtils.Clamp01(noiseValue);
                pixels[y * width + x] = GetColor(noiseValue);
                //pixels[y * width + x] = Color.Lerp(new Color(0,0,0,1), Color.White, noiseValue);
            }
        }
        
        return new Texture(width, height, pixels);
    }
    
    Color GetColor(float value)
    {
        /*if (value >= 0.0f && value < 0.3f) return new Color("#1b82c5");
        if (value >= 0.3f && value < 0.4f) return new Color("#2188d9"); 
        if (value >= 0.4f && value < 0.426f) return new Color("#dbba98");
        if (value >= 0.426f && value < 0.537f) return new Color("#4bba50");
        if (value >= 0.537f && value < 0.65f) return new Color("#46b249");
        if (value >= 0.65f && value < 0.74f) return new Color("#6d6b5e");
        if (value >= 0.74f && value < 0.8f) return new Color("#645952");*/
        
        if (value < 0.300) return new Color("#1b82c5");
        if (value < 0.400) return new Color("#2188d9");
        if (value < 0.426) return new Color("#dbba98");
        if (value < 0.537) return new Color("#4bba50");
        if (value < 0.650) return new Color("#46b249");
        if (value < 0.740) return new Color("#6d6b5e");
        if (value < 0.800) return new Color("#645952");

        return Color.White;
    }
}
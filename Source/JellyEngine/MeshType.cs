using System.Numerics;

namespace JellyEngine;

public static class MeshType
{
    public static Mesh Quad => CreateQuad();
    public static Mesh Cube => CreateCube();
    public static Mesh Plane => CreatePlane(2.0f, 2.0f, 1, 1);
    public static Mesh Capsule => CreateCapsule(0.5f, 2.0f, 32, 32);
    private static Mesh CreateQuad()
    {
        // Create individual lists
        var positions = new List<Vector3>
        {
            new(-0.5f, -0.5f, 0.0f), // Bottom left
            new( 0.5f, -0.5f, 0.0f), // Bottom right
            new( 0.5f,  0.5f, 0.0f), // Top right
            new(-0.5f,  0.5f, 0.0f)  // Top left
        };

        var normals = new List<Vector3>
        {
            new(0.0f, 0.0f, 1.0f),
            new(0.0f, 0.0f, 1.0f),
            new(0.0f, 0.0f, 1.0f),
            new(0.0f, 0.0f, 1.0f)
        };

        var tangents = new List<Vector4>
        {
            new(1.0f, 0.0f, 0.0f, 1.0f),
            new(1.0f, 0.0f, 0.0f, 1.0f),
            new(1.0f, 0.0f, 0.0f, 1.0f),
            new(1.0f, 0.0f, 0.0f, 1.0f)
        };

        var colors = new List<Vector4>
        {
            new(1.0f, 1.0f, 1.0f, 1.0f),
            new(1.0f, 1.0f, 1.0f, 1.0f),
            new(1.0f, 1.0f, 1.0f, 1.0f),
            new(1.0f, 1.0f, 1.0f, 1.0f)
        };

        var uv0 = new List<Vector2>
        {
            new(0.0f, 0.0f), // Bottom left
            new(1.0f, 0.0f), // Bottom right
            new(1.0f, 1.0f), // Top right
            new(0.0f, 1.0f)  // Top left
        };

        var uv1 = new List<Vector2>
        {
            new(0.0f, 0.0f),
            new(1.0f, 0.0f),
            new(1.0f, 1.0f),
            new(0.0f, 1.0f)
        };

        var indices = new List<uint>
        {
            0, 1, 2, // First triangle
            0, 2, 3  // Second triangle
        };

        var mesh = new Mesh
        {
            Positions = positions,
            Normals = normals,
            Tangents = tangents,
            Colors = colors,
            UV0 = uv0,
            UV1 = uv1,
            Indices = [.. indices]
        };

        return mesh;
    }

    public static Mesh CreatePlane(float sizeX, float sizeZ, int subdivX, int subdivZ)
    {
        var positions = new List<Vector3>();
        var normals = new List<Vector3>();
        var tangents = new List<Vector4>();
        var colors = new List<Vector4>();
        var uv0 = new List<Vector2>();
        var indices = new List<uint>();

        float stepX = sizeX / subdivX;
        float stepZ = sizeZ / subdivZ;
        float halfX = sizeX * 0.5f;
        float halfZ = sizeZ * 0.5f;

        for (int z = 0; z <= subdivZ; z++)
        {
            for (int x = 0; x <= subdivX; x++)
            {
                float posX = -halfX + x * stepX;
                float posZ = -halfZ + z * stepZ;

                positions.Add(new Vector3(posX, 0.0f, posZ));
                normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
                tangents.Add(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
                colors.Add(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                uv0.Add(new Vector2((float)x / subdivX, 1.0f - (float)z / subdivZ));
            }
        }

        for (int z = 0; z < subdivZ; z++)
        {
            for (int x = 0; x < subdivX; x++)
            {
                int topLeft = z * (subdivX + 1) + x;
                int topRight = topLeft + 1;
                int bottomLeft = (z + 1) * (subdivX + 1) + x;
                int bottomRight = bottomLeft + 1;

                indices.Add((uint)topLeft);
                indices.Add((uint)bottomLeft);
                indices.Add((uint)topRight);

                indices.Add((uint)topRight);
                indices.Add((uint)bottomLeft);
                indices.Add((uint)bottomRight);
            }
        }

        return new Mesh
        {
            Positions = positions,
            Normals = normals,
            Tangents = tangents,
            Colors = colors,
            UV0 = uv0,
            Indices = indices.ToArray()
        };
    }

    private static Mesh CreateCube()
    {
        // Define the vertices for the cube (positions, colors, texture coordinates)
        var positions = new List<Vector3>
        {
            // Front face
            new(-0.5f, -0.5f, 0.5f),
            new( 0.5f, -0.5f, 0.5f),
            new( 0.5f,  0.5f, 0.5f),
            new(-0.5f,  0.5f, 0.5f),

            // Back face
            new( 0.5f, -0.5f, -0.5f),
            new(-0.5f, -0.5f, -0.5f),
            new(-0.5f,  0.5f, -0.5f),
            new( 0.5f,  0.5f, -0.5f),

            // Left face
            new(-0.5f, -0.5f, -0.5f),
            new(-0.5f, -0.5f,  0.5f),
            new(-0.5f,  0.5f,  0.5f),
            new(-0.5f,  0.5f, -0.5f),

            // Right face
            new( 0.5f, -0.5f,  0.5f),
            new( 0.5f, -0.5f, -0.5f),
            new( 0.5f,  0.5f, -0.5f),
            new( 0.5f,  0.5f,  0.5f),

            // Top face
            new(-0.5f,  0.5f,  0.5f),
            new( 0.5f,  0.5f,  0.5f),
            new( 0.5f,  0.5f, -0.5f),
            new(-0.5f,  0.5f, -0.5f),

            // Bottom face
            new(-0.5f, -0.5f, -0.5f),
            new( 0.5f, -0.5f, -0.5f),
            new( 0.5f, -0.5f,  0.5f),
            new(-0.5f, -0.5f,  0.5f)
        };

        var normals = new List<Vector3>
        {
            // Front face
            new( 0.0f,  0.0f,  1.0f),
            new( 0.0f,  0.0f,  1.0f),
            new( 0.0f,  0.0f,  1.0f),
            new( 0.0f,  0.0f,  1.0f),

            // Back face
            new( 0.0f,  0.0f, -1.0f),
            new( 0.0f,  0.0f, -1.0f),
            new( 0.0f,  0.0f, -1.0f),
            new( 0.0f,  0.0f, -1.0f),

            // Left face
            new(-1.0f,  0.0f,  0.0f),
            new(-1.0f,  0.0f,  0.0f),
            new(-1.0f,  0.0f,  0.0f),
            new(-1.0f,  0.0f,  0.0f),

            // Right face
            new( 1.0f,  0.0f,  0.0f),
            new( 1.0f,  0.0f,  0.0f),
            new( 1.0f,  0.0f,  0.0f),
            new( 1.0f,  0.0f,  0.0f),

            // Top face
            new( 0.0f,  1.0f,  0.0f),
            new( 0.0f,  1.0f,  0.0f),
            new( 0.0f,  1.0f,  0.0f),
            new( 0.0f,  1.0f,  0.0f),

            // Bottom face
            new( 0.0f, -1.0f,  0.0f),
            new( 0.0f, -1.0f,  0.0f),
            new( 0.0f, -1.0f,  0.0f),
            new( 0.0f, -1.0f,  0.0f)
        };

        // Initialize all normals with a placeholder value (you can calculate them based on your needs)
        var tangents = new List<Vector4>(new Vector4[positions.Count]);
        for (int i = 0; i < tangents.Count; i++)
        {
            tangents[i] = new Vector4(1.0f, 0.0f, 0.0f, 1.0f); // Placeholder
        }

        /*var colors = new List<Vector4>
        {
            // Front face
            new(1.0f, 0.0f, 0.0f, 1.0f),
            new(1.0f, 0.0f, 0.0f, 1.0f),
            new(1.0f, 0.0f, 0.0f, 1.0f),
            new(1.0f, 0.0f, 0.0f, 1.0f),

            // Back face
            new(0.0f, 1.0f, 0.0f, 1.0f),
            new(0.0f, 1.0f, 0.0f, 1.0f),
            new(0.0f, 1.0f, 0.0f, 1.0f),
            new(0.0f, 1.0f, 0.0f, 1.0f),

            // Left face
            new(0.0f, 0.0f, 1.0f, 1.0f),
            new(0.0f, 0.0f, 1.0f, 1.0f),
            new(0.0f, 0.0f, 1.0f, 1.0f),
            new(0.0f, 0.0f, 1.0f, 1.0f),

            // Right face
            new(1.0f, 1.0f, 0.0f, 1.0f),
            new(1.0f, 1.0f, 0.0f, 1.0f),
            new(1.0f, 1.0f, 0.0f, 1.0f),
            new(1.0f, 1.0f, 0.0f, 1.0f),

            // Top face
            new(1.0f, 0.0f, 1.0f, 1.0f),
            new(1.0f, 0.0f, 1.0f, 1.0f),
            new(1.0f, 0.0f, 1.0f, 1.0f),
            new(1.0f, 0.0f, 1.0f, 1.0f),

            // Bottom face
            new(0.0f, 1.0f, 1.0f, 1.0f),
            new(0.0f, 1.0f, 1.0f, 1.0f),
            new(0.0f, 1.0f, 1.0f, 1.0f),
            new(0.0f, 1.0f, 1.0f, 1.0f)
        };*/
        var colors = new List<Vector4>();
        for (int i = 0; i < positions.Count; i++)
        {
            colors.Add(new Vector4(1, 1, 1, 1));  // Bottom-left
            //colors.Add(new Vector2(1.0f, 0.0f));  // Bottom-right
            // colors.Add(new Vector2(1.0f, 1.0f));  // Top-right
            // colors.Add(new Vector2(0.0f, 1.0f));  // Top-left
        }

        // Create a list of UVs for each face (4 UVs per face)
        var uv0 = new List<Vector2>();
        for (int i = 0; i < positions.Count; i += 4)
        {
            uv0.Add(new Vector2(0.0f, 0.0f));  // Bottom-left
            uv0.Add(new Vector2(1.0f, 0.0f));  // Bottom-right
            uv0.Add(new Vector2(1.0f, 1.0f));  // Top-right
            uv0.Add(new Vector2(0.0f, 1.0f));  // Top-left
        }

        // Second UV set (UV2)
        var uv1 = new List<Vector2>(new Vector2[positions.Count]);

        // Define indices for the cube faces
        var indices = new List<uint>
        {
            // Front face
            0, 1, 2,  0, 2, 3,
            // Back face
            4, 5, 6,  4, 6, 7,
            // Left face
            8, 9, 10, 8, 10, 11,
            // Right face
            12, 13, 14, 12, 14, 15,
            // Top face
            16, 17, 18, 16, 18, 19,
            // Bottom face
            20, 21, 22, 20, 22, 23
        };

        var mesh = new Mesh
        {
            Positions = positions,
            Normals = normals,
            Tangents = tangents,
            Colors = colors,
            UV0 = uv0,
            UV1 = uv1,
            Indices = [.. indices]
        };

        return mesh;
    }

    public static Mesh CreateSphere(float radius, int slices, int stacks)
    {
        var positions = new List<Vector3>();
        var normals = new List<Vector3>();
        var indices = new List<uint>();

        for (int i = 0; i <= stacks; i++)
        {
            float phi = MathF.PI * i / stacks;
            float y = radius * MathF.Cos(phi);
            float scale = radius * MathF.Sin(phi);

            for (int j = 0; j <= slices; j++)
            {
                float theta = 2 * MathF.PI * j / slices;
                float x = scale * MathF.Cos(theta);
                float z = scale * MathF.Sin(theta);

                var normal = Vector3.Normalize(new Vector3(x, y, z));
                positions.Add(new Vector3(x, y, z));
                normals.Add(normal);
            }
        }

        for (int i = 0; i < stacks; i++)
        {
            for (int j = 0; j < slices; j++)
            {
                int first = i * (slices + 1) + j;
                int second = first + slices + 1;

                indices.Add((uint)first);
                indices.Add((uint)second);
                indices.Add((uint)(first + 1));

                indices.Add((uint)second);
                indices.Add((uint)(second + 1));
                indices.Add((uint)(first + 1));
            }
        }

        return new Mesh { Positions = positions, Normals = normals, Indices = indices.ToArray() };
    }

    private static Mesh CreateCapsule(float radius, float height, int segments, int rings)
    {
        // A altura da parte cilíndrica é a altura total menos o raio das duas semiesferas
        float cylinderHeight = height - 2 * radius;

        // Criar as partes da cápsula
        var sphereTop = CreateSphere(radius, segments, rings / 2);
        var sphereBottom = CreateSphere(radius, segments, rings / 2);
        var cylinder = CreateCylinder(radius, cylinderHeight, segments);

        // Ajustar posições da esfera superior
        for (int i = 0; i < sphereTop.Positions.Count; i++)
        {
            sphereTop.Positions[i] = new Vector3(
                sphereTop.Positions[i].X,
                sphereTop.Positions[i].Y + cylinderHeight / 2,
                sphereTop.Positions[i].Z
            );
        }

        // Ajustar posições da esfera inferior
        for (int i = 0; i < sphereBottom.Positions.Count; i++)
        {
            sphereBottom.Positions[i] = new Vector3(
                sphereBottom.Positions[i].X,
                sphereBottom.Positions[i].Y - cylinderHeight / 2,
                sphereBottom.Positions[i].Z
            );
        }

        // Combinar vértices e índices
        var positions = sphereTop.Positions.Concat(sphereBottom.Positions).Concat(cylinder.Positions).ToList();
        var normals = sphereTop.Normals.Concat(sphereBottom.Normals).Concat(cylinder.Normals).ToList();
        var tangents = sphereTop.Tangents.Concat(sphereBottom.Tangents).Concat(cylinder.Tangents).ToList();
        var colors = sphereTop.Colors.Concat(sphereBottom.Colors).Concat(cylinder.Colors).ToList();

        // Calcular UVs corretamente
        var uv0 = new List<Vector2>();

        // UVs para a esfera superior
        for (int i = 0; i < sphereTop.Positions.Count; i++)
        {
            float u = (float)i / (sphereTop.Positions.Count - 1);
            float v = 0.5f + 0.5f * (sphereTop.Positions[i].Y / radius); // Mapear Y para V
            uv0.Add(new Vector2(u, v));
        }

        // UVs para a esfera inferior
        for (int i = 0; i < sphereBottom.Positions.Count; i++)
        {
            float u = (float)i / (sphereBottom.Positions.Count - 1);
            float v = 0.5f - 0.5f * (sphereBottom.Positions[i].Y / radius); // Mapear Y para V
            uv0.Add(new Vector2(u, v));
        }

        // UVs para o cilindro
        for (int i = 0; i < cylinder.Positions.Count; i++)
        {
            float u = (float)i / (segments); // Mapear ao redor do cilindro
            float v = (cylinder.Positions[i].Y + cylinderHeight / 2) / cylinderHeight; // Mapear ao longo da altura
            uv0.Add(new Vector2(u, v));
        }

        // Ajustar índices para combinar as malhas
        var indices = new List<uint>();
        indices.AddRange(sphereTop.Indices);
        indices.AddRange(sphereBottom.Indices.Select(i => i + (uint)sphereTop.Positions.Count));
        indices.AddRange(cylinder.Indices.Select(i => i + (uint)(sphereTop.Positions.Count + sphereBottom.Positions.Count)));

        // Verificar e corrigir as normais do cilindro
        for (int i = 0; i < cylinder.Positions.Count; i++)
        {
            // A normal do cilindro deve apontar para fora
            var normal = new Vector3(cylinder.Positions[i].X, 0.0f, cylinder.Positions[i].Z);
            normal = Vector3.Normalize(normal);
            normals[sphereTop.Positions.Count + sphereBottom.Positions.Count + i] = normal;
        }

        return new Mesh
        {
            Positions = positions,
            Normals = normals,
            Tangents = tangents,
            Colors = colors,
            UV0 = uv0,
            Indices = indices.ToArray()
        };
    }

    private static Mesh CreateCylinder(float radius, float height, int segments)
    {
        var positions = new List<Vector3>();
        var normals = new List<Vector3>();
        var tangents = new List<Vector4>();
        var colors = new List<Vector4>();
        var uv0 = new List<Vector2>();
        var indices = new List<uint>();

        float halfHeight = height / 2;

        // Top and bottom circles
        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * 2.0f * MathF.PI;
            float x = MathF.Cos(angle) * radius;
            float z = MathF.Sin(angle) * radius;

            // Top vertex
            positions.Add(new Vector3(x, halfHeight, z));
            normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
            tangents.Add(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
            colors.Add(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
            uv0.Add(new Vector2((float)i / segments, 1.0f));

            // Bottom vertex
            positions.Add(new Vector3(x, -halfHeight, z));
            normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
            tangents.Add(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
            colors.Add(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
            uv0.Add(new Vector2((float)i / segments, 0.0f));
        }

        // Side faces
        for (int i = 0; i < segments; i++)
        {
            int topLeft = i * 2;
            int topRight = topLeft + 2;
            int bottomLeft = topLeft + 1;
            int bottomRight = topRight + 1;

            indices.Add((uint)topLeft);
            indices.Add((uint)bottomLeft);
            indices.Add((uint)topRight);

            indices.Add((uint)topRight);
            indices.Add((uint)bottomLeft);
            indices.Add((uint)bottomRight);
        }

        return new Mesh
        {
            Positions = positions,
            Normals = normals,
            Tangents = tangents,
            Colors = colors,
            UV0 = uv0,
            Indices = indices.ToArray()
        };
    }
}


using System.Numerics;

namespace JellyEngine;

public static class MeshType
{
    public static Mesh Quad => CreateQuad();
    public static Mesh Cube => CreateCube();
    public static Mesh Plane => CreatePlane(1.0f, 1.0f, 1, 1);
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
        for (int i = 0; i < positions.Count; i ++)
        {
            colors.Add(new Vector4(1,1,1,1));  // Bottom-left
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
}

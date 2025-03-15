using System.Numerics;

namespace JellyEngine;

public class ChunkBuilder
{
    private readonly int _chunkSize;
    private readonly int _chunkArea;
    private readonly int _chunkVolume;
    
    public byte[] Voxels { get; private set; }
    
    public Mesh Mesh { get; private set; }

    public ChunkBuilder(int chunkSize)
    {
        _chunkSize = chunkSize;
        _chunkArea = _chunkSize * _chunkSize;
        _chunkVolume = _chunkArea * _chunkSize;

        Voxels = new byte[_chunkVolume];

        InitializeVoxels();
        Mesh = BuildMesh();
    }

    private void InitializeVoxels()
    {
        var noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        noise.SetFrequency(0.025f);

        for (int x = 0; x < _chunkSize; x++)
        {
            for (int z = 0; z < _chunkSize; z++)
            {
                for (int y = 0; y < _chunkSize; y++)
                {
                    var value = noise.GetNoise(x, y, z);
                    Voxels[x + _chunkSize * z + _chunkArea * y] = value >= 0 ? (byte)(x + y + z) : (byte)0;
                }
            }
        }
    }

    private Mesh BuildMesh()
    {
        var mesh = new Mesh();
        var indices = new List<uint>();
        var currentIndex = 0u;
        
        var topNormal = new Vector3(0, 1, 0);
        var bottomNormal = new Vector3(0, -1, 0);
        var rightNormal = new Vector3(1, 0, 0);
        var leftNormal = new Vector3(-1, 0, 0);
        var backNormal = new Vector3(0, 0, -1);
        var frontNormal = new Vector3(0, 0, 1);
        
        var uv00 = new Vector2(0, 0);
        var uv01 = new Vector2(0, 1);
        var uv10 = new Vector2(1, 0);
        var uv11 = new Vector2(1, 1);

        for (var x = 0; x < _chunkSize; x++)
        {
            for (var y = 0; y < _chunkSize; y++)
            {
                for (var z = 0; z < _chunkSize; z++)
                {
                    var voxelIndex = x + _chunkSize * z + _chunkArea * y;
                    var voxelId = Voxels[voxelIndex];

                    if (voxelId == 0)
                    {
                        continue;
                    }

                    // Top face 
                    if (IsVoid(new Vector3(x, y + 1, z)))
                    {
                        mesh.Positions.Add(new Vector3(x, y + 1, z));
                        mesh.Positions.Add(new Vector3(x + 1, y + 1, z));
                        mesh.Positions.Add(new Vector3(x + 1, y + 1, z + 1));
                        mesh.Positions.Add(new Vector3(x, y + 1, z + 1));

                        mesh.Normals.AddRange(new[] { topNormal, topNormal, topNormal, topNormal });
                        mesh.UV0.AddRange(new[] { uv01, uv11, uv10, uv00 });
                        
                        for (var i = 0; i < 4; i++)
                        {
                            mesh.Colors.Add((new Color(1,1,1)).ToVector4());
                        }

                        indices.AddRange(new[] { currentIndex, currentIndex + 3, currentIndex + 2, currentIndex, currentIndex + 2, currentIndex + 1 });
                        currentIndex += 4;
                    }
                    // bottom face
                    if (IsVoid(new Vector3(x, y - 1, z)))
                    {
                        mesh.Positions.Add(new Vector3(x, y, z));
                        mesh.Positions.Add(new Vector3(x + 1, y, z));
                        mesh.Positions.Add(new Vector3(x + 1, y, z + 1));
                        mesh.Positions.Add(new Vector3(x, y, z + 1));

                        mesh.Normals.AddRange(new[] { bottomNormal, bottomNormal, bottomNormal, bottomNormal });
                        mesh.UV0.AddRange(new[] { uv00, uv10, uv11, uv01 });
                        
                        for (var i = 0; i < 4; i++)
                        {
                            mesh.Colors.Add((new Color(1,1,1)).ToVector4());
                        }
                        
                        indices.AddRange(new[] { currentIndex, currentIndex + 2, currentIndex + 3, currentIndex, currentIndex + 1, currentIndex + 2 });
                        currentIndex += 4;
                    }

                    // right face
                    if (IsVoid(new Vector3(x + 1, y, z)))
                    {
                        mesh.Positions.Add(new Vector3(x + 1, y, z));
                        mesh.Positions.Add(new Vector3(x + 1, y + 1, z));
                        mesh.Positions.Add(new Vector3(x + 1, y + 1, z + 1));
                        mesh.Positions.Add(new Vector3(x + 1, y, z + 1));

                        mesh.Normals.AddRange(new[] { rightNormal, rightNormal, rightNormal, rightNormal });
                        mesh.UV0.AddRange(new[] { uv10, uv11, uv01, uv00 });

                        for (var i = 0; i < 4; i++)
                        {
                            mesh.Colors.Add((new Color(1,1,1)).ToVector4());
                        }

                        indices.AddRange(new[] { currentIndex, currentIndex + 1, currentIndex + 2, currentIndex, currentIndex + 2, currentIndex + 3 });
                        currentIndex += 4;
                    }

                    // left face
                    if (IsVoid(new Vector3(x - 1, y, z)))
                    {
                        mesh.Positions.Add(new Vector3(x, y, z));
                        mesh.Positions.Add(new Vector3(x, y + 1, z));
                        mesh.Positions.Add(new Vector3(x, y + 1, z + 1));
                        mesh.Positions.Add(new Vector3(x, y, z + 1));

                        mesh.Normals.AddRange(new[] { leftNormal, leftNormal, leftNormal, leftNormal });
                        mesh.UV0.AddRange(new[] { uv00, uv01, uv11, uv10 });
                        
                        for (var i = 0; i < 4; i++)
                        {
                            mesh.Colors.Add((new Color(1,1,1)).ToVector4());
                        }

                        indices.AddRange(new[] { currentIndex, currentIndex + 2, currentIndex + 1, currentIndex, currentIndex + 3, currentIndex + 2 });
                        currentIndex += 4;
                    }

                    // back face
                    if (IsVoid(new Vector3(x, y, z - 1)))
                    {
                        mesh.Positions.Add(new Vector3(x, y, z));
                        mesh.Positions.Add(new Vector3(x, y + 1, z));
                        mesh.Positions.Add(new Vector3(x + 1, y + 1, z));
                        mesh.Positions.Add(new Vector3(x + 1, y, z));

                        mesh.Normals.AddRange(new[] { backNormal, backNormal, backNormal, backNormal });
                        mesh.UV0.AddRange(new[] { uv10, uv11, uv01, uv00 });
                        
                        for (var i = 0; i < 4; i++)
                        {
                            mesh.Colors.Add((new Color(1,1,1)).ToVector4());
                        }

                        indices.AddRange(new[] { currentIndex, currentIndex + 1, currentIndex + 2, currentIndex, currentIndex + 2, currentIndex + 3 });
                        currentIndex += 4;
                    }

                    // front face
                    if (IsVoid(new Vector3(x, y, z + 1)))
                    {
                        mesh.Positions.Add(new Vector3(x, y, z + 1));
                        mesh.Positions.Add(new Vector3(x, y + 1, z + 1));
                        mesh.Positions.Add(new Vector3(x + 1, y + 1, z + 1));
                        mesh.Positions.Add(new Vector3(x + 1, y, z + 1));

                        mesh.Normals.AddRange(new[] { frontNormal, frontNormal, frontNormal, frontNormal });
                        mesh.UV0.AddRange(new[] { uv00, uv01, uv11, uv10 });
                        
                        for (var i = 0; i < 4; i++)
                        {
                            mesh.Colors.Add((new Color(1f,1,1)).ToVector4());
                        }

                        indices.AddRange(new[] { currentIndex, currentIndex + 2, currentIndex + 1, currentIndex, currentIndex + 3, currentIndex + 2 });
                        currentIndex += 4;
                    }
                }
            }
        }
        
        mesh.Indices = indices.ToArray();

        return mesh;
    }
    
    private bool IsVoid(Vector3 voxelPos)
    {
        var x = (int)voxelPos.X;
        var y = (int)voxelPos.Y;
        var z = (int)voxelPos.Z;

        if (x >= 0 && x < _chunkSize && y >= 0 && y < _chunkSize && z >= 0 && z < _chunkSize)
        {
            var index = x + _chunkSize * z + _chunkArea * y;
            return Voxels[index] == 0;
        }
        
        return true;
    }
}
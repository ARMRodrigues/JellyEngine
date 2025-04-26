using System;

namespace JellyGame.Scenes.Guild;

public struct Block
{
    public BlockType Type;
    public bool IsSolid => Type != BlockType.Air;
}

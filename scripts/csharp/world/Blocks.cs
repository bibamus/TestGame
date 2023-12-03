using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace TestGame.world;

public static class Blocks
{
    static readonly IDictionary<BlockType, Block> _blocks = new Dictionary<BlockType, Block>();

    static Blocks()
    {
        _blocks.Add(BlockType.Dirt, new Block(BlockType.Dirt, 0));
        _blocks.Add(BlockType.Stone, new Block(BlockType.Stone, 1));
    }
    
    public static Block GetBlock(BlockType type)
    {
        return _blocks[type];
    }
}

public enum BlockType
{
    Dirt,
    Stone
}
using System;
using Godot;

namespace TestGame.world;

public partial class World : Node
{
    private TileMap _tileMap;
    
    [Export] private int _worldWidth;
    [Export] private int _worldHeight;

    private  Block[,] _blocks;

    public override void _Ready()
    {
        _blocks = new Block[_worldWidth, _worldHeight];
        _tileMap = GetNode<TileMap>("TileMap");

        Randomize();
        UpdateTilemap();
    }

    private void UpdateTilemap()
    {
        for (var x = 0; x < _worldWidth; x++)
        {
            for (var y = 0; y < _worldHeight; y++)
            {
                var block = _blocks[x, y];
                _tileMap.SetCell(0, new Vector2I(x, y), block.SourceId, new Vector2I(7, 1));
            }
        }
    }

    private void Randomize()
    {
        for (var x = 0; x < _worldWidth; x++)
        {
            for (var y = 0; y < _worldHeight; y++)
            {
                var next = new Random().Next(0, 2);
                var block = next == 1 ? Blocks.GetBlock(BlockType.Stone) : Blocks.GetBlock(BlockType.Dirt);
                _blocks[x, y] = block;
            }
        }
    }
}
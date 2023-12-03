using System;
using Godot;

namespace TestGame.world;

public partial class World : Node
{
    private TileMap _tileMap;

    [Export] private int _worldWidth;
    [Export] private int _worldHeight;

    private WorldData _worldData;

    public override void _Ready()
    {
        _tileMap = GetNode<TileMap>("TileMap");
        var generator = GetNode<WorldGenerator>("WorldGenerator");
        var settings = new WorldGenerationSettings
        {
            WorldWidth = _worldWidth,
            WorldHeight = _worldHeight,
            Seed = new Random().Next()
        };
        _worldData = generator.GenerateWorld(settings);
        UpdateTilemap();
    }

    private void UpdateTilemap()
    {
        for (var x = 0; x < _worldWidth; x++)
        {
            for (var y = 0; y < _worldHeight; y++)
            {
                var block = _worldData.Blocks[x, y];
                if (block != null)
                {
                    var coords = new Vector2I(
                        x - _worldData.WorldWidth / 2,
                        -y + _worldData.WorldHeight / 2
                    );
                    _tileMap.SetCell(0, coords, block.SourceId,
                        new Vector2I(7, 1));
                }
            }
        }
    }
}
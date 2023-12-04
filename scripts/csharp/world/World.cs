using System;
using Godot;
using TestGame.world.generation;

namespace TestGame.world;

public partial class World : Node
{
    private TileMap _tileMap;

    [Export] private int _worldWidth;
    [Export] private int _worldHeight;

    [Export] private Node2D _player;
    
    private WorldData _worldData;

    public override void _Ready()
    {
        _tileMap = GetNode<TileMap>("TileMap");
        var generator = GetNode<generation.WorldGenerator>("WorldGenerator");
        var settings = new WorldGenerationSettings
        {
            WorldWidth = _worldWidth,
            WorldHeight = _worldHeight,
            Seed = new Random().Next()
        };
        _worldData = generator.GenerateWorld(settings);
        
        
        for(var x = 0; x < _worldWidth ; x++)
        {
            for (var y = 0; y < _worldHeight; y++)
            {
                var coord = _worldData.BottomLeft + new Vector2I(x,-y);
                var block = _worldData.GetBlockAtWorldPosition(coord);
               

            }
        }
        
        // UpdateTilemap();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    
    }
    
    private Vector2I GetPlayerTilePosition()
    {
        return _tileMap.LocalToMap(_player.Position);
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
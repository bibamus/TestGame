using Godot;

namespace TestGame.world;

public partial class TileMapChunk : TileMap
{
    public BlockType[,] ChunkBlocks { get; set; }

    public override void _Ready()
    {
        base._Ready();
        GD.Print($"TileMapChunk {Name} ready");
        UpdateTileMap();
    }

    private void UpdateTileMap()
    {
        for (var x = 0; x < ChunkBlocks.GetLength(0); x++)
        {
            for (var y = 0; y < ChunkBlocks.GetLength(1); y++)
            {
                var block = ChunkBlocks[x, y];
                if (block == BlockType.None)
                {
                    continue;
                }

                SetCell(0, new Vector2I(x, -y), Blocks.GetBlock(block).SourceId,
                    new Vector2I(7, 1));
            }
        }
    }
    
}
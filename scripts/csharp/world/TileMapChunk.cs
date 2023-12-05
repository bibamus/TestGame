using Godot;

namespace TestGame.world;

public partial class TileMapChunk : TileMap
{
    public Block[,] Blocks { get; set; }

    public override void _Ready()
    {
        base._Ready();
        GD.Print($"TileMapChunk {Name} ready");
        UpdateTileMap();

    }

    private void UpdateTileMap()
    {
        for (var x = 0; x < Blocks.GetLength(0); x++)
        {
            for (var y = 0; y < Blocks.GetLength(1); y++)
            {
                var block = Blocks[x, y];
                if (block == null)
                {
                    continue;
                }

                SetCell(0, new Vector2I(x, -y), block.SourceId,
                    new Vector2I(7, 1));
            }
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        

    }
}
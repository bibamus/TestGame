using Godot;

namespace TestGame.world;


public partial class Block : Resource
{
    public Block(BlockType Type, int SourceId)
    {
        this.Type = Type;
        this.SourceId = SourceId;
    }

    public BlockType Type { get; init; }
    public int SourceId { get; init; }

    public void Deconstruct(out BlockType Type, out int SourceId)
    {
        Type = this.Type;
        SourceId = this.SourceId;
    }
}
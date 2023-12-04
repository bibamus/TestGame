using System;
using Godot;

namespace TestGame.world.generation;

[GlobalClass]
public partial class TestGenerationStep : GenerationStep
{
    public override WorldGenerationData Apply(WorldGenerationData data, WorldGenerationSettings settings)
    {
        var worldData = data.WorldData;

        var grad = (double)worldData.WorldWidth / (worldData.WorldHeight / 2.0);


        for (var x = 0; x < settings.WorldWidth; x++)
        {
            for (var y = 0; y < settings.WorldHeight / 2; y++)
            {
                if (Math.Abs(x - y * grad) < 5)
                {
                    worldData.Blocks[x, y] = Blocks.GetBlock(BlockType.Stone);
                }
                else
                {
                    worldData.Blocks[x, y] = Blocks.GetBlock(BlockType.Dirt);
                }
            }
        }

        for (var x = 0; x < settings.WorldWidth; x++)
        {
            worldData.Blocks[x, 0] = Blocks.GetBlock(BlockType.Stone);
            worldData.Blocks[x, settings.WorldHeight - 1] = Blocks.GetBlock(BlockType.Stone);
        }

        for (var y = 0; y < settings.WorldHeight; y++)
        {
            worldData.Blocks[0, y] = Blocks.GetBlock(BlockType.Stone);
            worldData.Blocks[settings.WorldWidth - 1, y] = Blocks.GetBlock(BlockType.Stone);
        }

        return data;
    }
}
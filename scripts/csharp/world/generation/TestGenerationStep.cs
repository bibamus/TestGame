using System;
using Godot;

namespace TestGame.world.generation;

[GlobalClass]
public partial class TestGenerationStep : GenerationStep
{
    public override WorldGenerationData Apply(WorldGenerationData data, WorldGenerationSettings settings)
    {
        var worldData2 = data.WorldData;

        var grad = (double)worldData2.Width / (worldData2.Height / 2.0);


        for (var x = 0; x < settings.WorldWidth; x++)
        {
            for (var y = 0; y < settings.WorldHeight / 2; y++)
            {
                if (Math.Abs(x - y * grad) < 5)
                {
                    worldData2.Blocks[x][ y] = BlockType.Stone;
                }
                else
                {
                    worldData2.Blocks[x][ y] = BlockType.Dirt;
                }
            }
        }

        for (var x = 0; x < settings.WorldWidth; x++)
        {
            worldData2.Blocks[x][ 0] = BlockType.Stone;
            worldData2.Blocks[x][ settings.WorldHeight - 1] = BlockType.Stone;
        }

        for (var y = 0; y < settings.WorldHeight; y++)
        {
            worldData2.Blocks[0][ y] = BlockType.Stone;
            worldData2.Blocks[settings.WorldWidth - 1][ y] = BlockType.Stone;
        }

        return data;
    }
}
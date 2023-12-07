using System;
using Godot;

namespace TestGame.world.generation;

[GlobalClass]
public partial class TestGenerationStep : GenerationStep
{
    public override WorldGenerationData Apply(WorldGenerationData data)
    {
        var worldData = data.WorldData;

        var settings = data.Settings;

        var grad = (double)worldData.Width / (worldData.Height / 2.0);


        for (var x = 0; x < settings.WorldWidth; x++)
        {
            for (var y = 0; y < settings.WorldHeight / 2; y++)
            {
                if (Math.Abs(x - y * grad) < 5)
                {
                    worldData.Blocks[x][ y] = BlockType.Stone;
                }
                else
                {
                    worldData.Blocks[x][ y] = BlockType.Dirt;
                }
            }
        }

        for (var x = 0; x < settings.WorldWidth; x++)
        {
            worldData.Blocks[x][ 0] = BlockType.Stone;
            worldData.Blocks[x][ settings.WorldHeight - 1] = BlockType.Stone;
        }

        for (var y = 0; y < settings.WorldHeight; y++)
        {
            worldData.Blocks[0][ y] = BlockType.Stone;
            worldData.Blocks[settings.WorldWidth - 1][ y] = BlockType.Stone;
        }

        return data;
    }
}
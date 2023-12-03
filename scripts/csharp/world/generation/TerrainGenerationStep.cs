using System;
using Godot;

namespace TestGame.world.generation;

[GlobalClass]
public partial class TerrainGenerationStep : GenerationStep
{
    public override WorldData Apply(WorldData worldData, WorldGenerationSettings settings)
    {
        var amplitude = 10;

        var fastNoiseLite = new FastNoiseLite
        {
            Seed = settings.Seed,
            Frequency = 0.1f,
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
            FractalGain = 0.3f
        };


        for (var x = 0; x < settings.WorldWidth; x++)
        {
            var noise = fastNoiseLite.GetNoise1D(x);
            var height = (int) Math.Floor((worldData.SurfaceLevel) + noise * amplitude);
            for (var y = 0; y < height; y++)
            {
                worldData.Blocks[x, y] = Blocks.GetBlock(BlockType.Dirt);
            }
        }

        return worldData;
    }
}
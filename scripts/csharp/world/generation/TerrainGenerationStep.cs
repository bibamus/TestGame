using System;
using Godot;

namespace TestGame.world.generation;

[GlobalClass]
public partial class TerrainGenerationStep : GenerationStep
{
    [Export] private int _amplitude = 10;

    [Export] private float _frequency = 0.1f;

    [Export] private float _fractalGain = 0.3f;

    public override WorldGenerationData Apply(WorldGenerationData data, WorldGenerationSettings settings)
    {
        var worldData = data.WorldData;

        var fastNoiseLite = new FastNoiseLite
        {
            Seed = settings.Seed,
            Frequency = _frequency,
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
            FractalGain = _fractalGain
        };

        var terrainHeights = new int[settings.WorldWidth];
        for (var x = 0; x < settings.WorldWidth; x++)
        {
            var noise = fastNoiseLite.GetNoise1D(x);
            var height = (int) Math.Floor((worldData.SurfaceLevel) + noise * _amplitude);
            terrainHeights[x] = height;
            for (var y = 0; y < height; y++)
            {
                worldData.Blocks[x, y] = Blocks.GetBlock(BlockType.Dirt);
            }
        }

        data.TerrainHeightMap = terrainHeights;

        return data;
    }
}
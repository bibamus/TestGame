﻿using Godot;

namespace TestGame.world.generation;

[GlobalClass]
public partial class CaveGenerationStep : GenerationStep
{
    [Export] private float _threshold = 0.5f;
    [Export] private float _frequency = 0.05f;

    public override WorldGenerationData Apply(WorldGenerationData data, WorldGenerationSettings settings)
    {
        var worldData = data.WorldData;

        var fastNoiseLite = new FastNoiseLite
        {
            Seed = settings.Seed,
            Frequency = _frequency,
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin
        };

        for (int x = 0; x < settings.WorldWidth; x++)
        {
            for (int y = 0; y < worldData.SurfaceLevel; y++)
            {
                var noiseValue = fastNoiseLite.GetNoise2D(x, y);
                if (noiseValue > _threshold)
                {
                    worldData.Blocks[x, y] = null;
                }
            }
        }

        return data;
    }
}
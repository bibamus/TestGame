using System;
using System.Collections.Generic;
using Godot;
using TestGame.world.generation;

namespace TestGame.world;

public partial class WorldGenerator : Node
{
    private List<GenerationStep> _steps;

    public override void _Ready()
    {
        base._Ready();
        var children = GetChildren();
        _steps = new List<GenerationStep>();
        foreach (var child in children)
        {
            if (child is GenerationStep step)
            {
                _steps.Add(step);
            }
        }
    }

    public WorldData GenerateWorld(WorldGenerationSettings settings)
    {
        var worldData = new WorldData(settings.WorldWidth, settings.WorldHeight)
        {
            SurfaceLevel = (int) Math.Floor(settings.WorldHeight * 0.75),
        };

        foreach (var generationStep in _steps)
        {
            worldData = generationStep.Apply(worldData, settings);
        }

        return worldData;
    }
}
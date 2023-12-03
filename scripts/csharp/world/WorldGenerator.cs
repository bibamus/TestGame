using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using TestGame.world.generation;

using System.Linq;

namespace TestGame.world;

public partial class WorldGenerator : Node
{
    private List<GenerationStep> _steps;

    public override void _Ready()
    {
        base._Ready();
        _steps = GetChildren()
            .Where(node => node is GenerationStep)
            .Cast<GenerationStep>()
            .ToList();
        
    }

    public WorldData GenerateWorld(WorldGenerationSettings settings)
    {
        var worldData = new WorldData(settings.WorldWidth, settings.WorldHeight)
        {
            SurfaceLevel = (int) Math.Floor(settings.WorldHeight * 0.75),
        };

        var worldGenerationData = new WorldGenerationData(worldData);

        foreach (var generationStep in _steps)
        {
            worldGenerationData = generationStep.Apply(worldGenerationData, settings);
        }

        return worldGenerationData.WorldData;
    }
}
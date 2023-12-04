using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace TestGame.world.generation;

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
        GD.Print("Generating world");
        var worldData = new WorldData(settings.WorldWidth, settings.WorldHeight)
        {
            SurfaceLevel = (int)Math.Floor(settings.WorldHeight * 0.75),
        };

        var worldGenerationData = new WorldGenerationData(worldData);

        foreach (var generationStep in _steps)
        {
            GD.Print($"Applying step {generationStep.Name}");
            worldGenerationData = generationStep.Apply(worldGenerationData, settings);
            GD.Print($"Finished step {generationStep.Name}");
        }

        GD.Print("Finished generating world");
        return worldGenerationData.WorldData;
    }
}
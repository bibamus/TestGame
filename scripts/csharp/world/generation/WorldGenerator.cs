using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

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

        var outer = new Array<Array<BlockType>>();
        for (var x = 0; x < settings.WorldWidth; x++)
        {
            var inner = new Array<BlockType>();
            for (var y = 0; y < settings.WorldHeight; y++)
            {
                inner.Add(BlockType.None);
            }

            outer.Add(inner);
        }

        var worldData2 = new WorldData()
        {
            SurfaceLevel = (int) Math.Floor(settings.WorldHeight * 0.75),
            Height = settings.WorldHeight,
            Width = settings.WorldWidth,
            Blocks = outer
        };
        var worldGenerationData = new WorldGenerationData(worldData2);

        foreach (var generationStep in _steps)
        {
            GD.Print($"Applying step {generationStep.Name}");
            worldGenerationData = generationStep.Apply(worldGenerationData, settings);
            GD.Print($"Finished step {generationStep.Name}");
        }

        GD.Print("Finished generating world");
        return worldData2;
    }
}
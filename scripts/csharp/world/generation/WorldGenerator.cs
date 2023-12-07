using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace TestGame.world.generation;

public partial class WorldGenerator : Node
{
    private IEnumerator<GenerationStep> _steps;
    private Task<WorldGenerationData> _currentTask;
    private string _currentStepName;
    private bool _finished;
    [Export] public WorldGenerationSettings Settings { get; set; }

    [Signal]
    public delegate void GenerationStepStartedEventHandler(string stepName);

    [Signal]
    public delegate void GenerationStepFinishedEventHandler(string stepName);

    [Signal]
    public delegate void WorldGeneratedEventHandler(WorldData worldData);


    public override void _Ready()
    {
        base._Ready();

        _steps = GetChildren()
            .Where(node => node is GenerationStep)
            .Cast<GenerationStep>()
            .GetEnumerator();

        _currentTask = Task.Run(() => InitWorldGenerationData(Settings));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_currentTask.IsFaulted)
        {
            GD.PrintErr("Generation step failed");
            GD.PrintErr(_currentTask.Exception);
            _steps.Dispose();
            _finished = true;
            return;
        }

        if (_finished || !_currentTask.IsCompleted)
        {
            return;
        }

        if (_currentStepName != null)
        {
            EmitSignal(SignalName.GenerationStepFinished, _currentStepName);
        }

        if (_steps.MoveNext())
        {
            var step = _steps.Current;
            _currentStepName = step.Name;
            EmitSignal(SignalName.GenerationStepStarted, _currentStepName);
            var wg = _currentTask.Result;
            _currentTask = Task.Run(() => step.Apply(wg));
        }
        else
        {
            EmitSignal(SignalName.WorldGenerated, _currentTask.Result.WorldData);
            _steps.Dispose();
            _finished = true;
        }
    }

    private static WorldGenerationData InitWorldGenerationData(WorldGenerationSettings settings)
    {
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

        var worldData = new WorldData()
        {
            SurfaceLevel = (int) Math.Floor(settings.WorldHeight * 0.75),
            Height = settings.WorldHeight,
            Width = settings.WorldWidth,
            Blocks = outer
        };
        var worldGenerationData = new WorldGenerationData(worldData, settings);
        return worldGenerationData;
    }
}
namespace TestGame.world.generation;

public class WorldGenerationData
{
    public WorldGenerationData(WorldData worldData, WorldGenerationSettings settings)
    {
        WorldData = worldData;
        Settings = settings;
    }

    public WorldData WorldData { get; }

    public int[] TerrainHeightMap { get; set; }
    
    public WorldGenerationSettings Settings { get; private set; }
}
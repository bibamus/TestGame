namespace TestGame.world.generation;

public class WorldGenerationData
{
    public WorldGenerationData(WorldData worldData)
    {
        WorldData = worldData;
    }

    public WorldData WorldData { get; }
    public int[] TerrainHeightMap { get; set; }
}
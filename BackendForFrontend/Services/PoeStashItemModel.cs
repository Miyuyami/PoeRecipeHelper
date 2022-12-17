namespace BackendForFrontend.Services;

public class PoeStashItemModel {
    public string IconUrl { get; init; }
    public int Quality { get; init; }
    public int ItemLevel { get; init; }
    public List<string> ExplicitMods { get; init; }
    public int Width { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
    public string Name { get; init; }
}

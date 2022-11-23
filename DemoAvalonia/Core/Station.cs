namespace DemoAvalonia.Core;

public class Station {
    public required string City { get; init; }
    public required string Name { get; init; }
    
    public override string ToString()
    {
        return $"{this.City} - {this.Name}";
    }
}

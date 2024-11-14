namespace Consumer.ConsoleApp;

public class IIdGenerator
{
    public Guid Id { get; set; }
}


public class IdGenerator : IIdGenerator
{
    public Guid Id { get; } = Guid.NewGuid();
}

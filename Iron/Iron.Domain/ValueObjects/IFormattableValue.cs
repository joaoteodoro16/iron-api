namespace Iron.Domain.ValueObjects;

public interface IFormattableValue
{
    string Value { get; }

    string GetFormatted();
}

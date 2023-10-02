namespace ConstrainedValues
{
    public interface IReadOnlyConstrainedValue<TValue>
    {
        TValue Value { get; }
        TValue Ceiling { get; }
    }
}

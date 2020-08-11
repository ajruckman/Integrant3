namespace Integrant.Rudiment.Inputs
{
    public interface IOption<TID>
    {
        string Key      { get; }
        TID    Value    { get; }
        string Name     { get; }
        bool   Disabled { get; }
    }
}
namespace Integrant.Rudiment.Inputs
{
    public interface IOption<TID>
    {
        TID    ID       { get; }
        string Name     { get; }
        bool   Disabled { get; }
    }
}
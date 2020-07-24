namespace Integrant.Fundament
{
    public class Layer
    {
        public Layer(Layer? parent = null)
        {
            RootLayer   = parent?.RootLayer ?? this;
            ParentLayer = parent;
        }

        public Layer RootLayer { get; }

        public Layer? ParentLayer { get; }

        public bool NeedsUpdate { get; }
    }
}
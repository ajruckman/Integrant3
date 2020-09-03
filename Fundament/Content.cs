using Microsoft.AspNetCore.Components;

namespace Integrant.Fundament
{
    public readonly struct Content
    {
        private Content(string? s)
        {
            Fragment = builder => builder.AddContent(0, s);

            #if DEBUG
            SourceValue = s;
            #endif
        }

        private Content(MarkupString s)
        {
            Fragment = builder => builder.AddContent(0, s);

            #if DEBUG
            SourceValue = s;
            #endif
        }

        private Content(RenderFragment s)
        {
            Fragment = s;

            #if DEBUG
            SourceValue = s;
            #endif
        }

        #if DEBUG
        private readonly object? SourceValue;
        #endif

        public static Content operator +(Content a, Content b)
        {
            return new RenderFragment(builder =>
            {
                builder.AddContent(0, a.Fragment);
                builder.AddContent(1, b.Fragment);
            });
        }

        public static Content operator +(Content a, string b)
        {
            return new RenderFragment(builder =>
            {
                builder.AddContent(0, a.Fragment);
                builder.AddContent(1, b);
            });
        }

        public static Content operator +(string? a, Content b)
        {
            return new RenderFragment(builder =>
            {
                builder.AddContent(0, a);
                builder.AddContent(1, b.Fragment);
            });
        }

        public readonly RenderFragment Fragment;

        public static implicit operator Content(string?        s) => new Content(s);
        public static implicit operator Content(MarkupString   s) => new Content(s);
        public static implicit operator Content(RenderFragment s) => new Content(s);
    }

    public static class ContentExtensions
    {
        public static Content Content(this RenderFragment fragment) => (Content) fragment;
    }
}
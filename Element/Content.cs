using Microsoft.AspNetCore.Components;

namespace Integrant.Element
{
    public readonly struct Content
    {
        public Content(string s)
        {
            Fragment = builder => builder.AddContent(0, s);
        }

        public Content(MarkupString s)
        {
            Fragment = builder => builder.AddContent(0, s);
        }

        public readonly RenderFragment Fragment;

        public static implicit operator Content(string       s) => new Content(s);
        public static implicit operator Content(MarkupString s) => new Content(s);
    }
}
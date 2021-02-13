using System;

namespace Integrant.Dominant
{
    public static class Extensions
    {
        // https://stackoverflow.com/a/21704965
        public static DateTime Trim(this DateTime date, long ticks) =>
            new(date.Ticks - (date.Ticks % ticks), date.Kind);
    }
}
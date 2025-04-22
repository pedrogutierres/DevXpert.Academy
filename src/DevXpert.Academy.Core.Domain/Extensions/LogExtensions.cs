using Microsoft.Extensions.Logging;

namespace Core.Domain.Extensions
{
    public static class AppLogEvents
    {
        public static EventId[] Events => [iFood];

        public static EventId iFood => new(1001, "iFood");
    }
}

using System;

namespace LibTimeCreator
{
    public static class SystemTime
    {
        public static Func<DateTimeOffset> Now = () => DateTime.Now.ToUniversalTime();
    }
}

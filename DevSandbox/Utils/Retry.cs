using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSandbox
{
    public class Retry
    {
        public static TResult Execute<TResult>(Func<TResult> operation, int times, TimeSpan delay)
        {
            var attempts = 0;
            TResult result = default(TResult);

            do
            {
                try
                {
                    attempts += 1;
                    result = operation();
                    return result;
                }
                catch (Exception ex)
                {
                    if (attempts == times) throw;
                    Task.Delay(delay).Wait();
                }
            }
            while (true);

            return result;
        }
    }
}

using System;
using System.Net;
using System.Threading.Tasks;

namespace dnstest
{
    class Program
    {
        static async Task Main()
        {
            long runCount = 0;
            int failCount = 0;
            while (true)
            {
                ++runCount;

                if (runCount % 10000 == 0)
                {
                    Console.WriteLine($"Run {runCount} - Failures: {failCount}");
                }

                try
                {
                    IPHostEntry entry = await Dns.GetHostEntryAsync("localhost");

                    if (entry is null) { failCount++; Console.WriteLine("FAIL - NULL"); continue; };

                    if (entry.HostName.Length == 0) { failCount++; Console.WriteLine("FAIL - Empty host name"); continue; }

                    if (entry.AddressList.Length == 0) { failCount++; Console.WriteLine("FAIL - Empty address list"); continue; }

                    bool allGood = true;
                    foreach (IPAddress address in entry.AddressList)
                    {
                        if (!IPAddress.IsLoopback(address))
                        {
                            Console.WriteLine("NOT LOOPBACK: " + address.ToString());
                            allGood = false;
                        }
                    }

                    if (!allGood) failCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    failCount++;
                }
            }
        }
    }
}

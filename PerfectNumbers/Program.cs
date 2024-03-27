using System.ComponentModel.Design;
using System.Numerics;


namespace PerfectNumbers
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var max = new BigInteger(50000000000);
            BigInteger chunkSize = 10000; // Adjust the chunk size as needed
            var tasks = new List<Task>();
            BigInteger start = 2;
            while (start < max)
            {
                var end = start + chunkSize - 1;
                if (end > max)
                    end = max;

                var task = ProcessChunkAsync(start, end);
                tasks.Add(task);

                start = end + 1;
            }

            Task.WaitAll(tasks.ToArray());
        }

        private static async Task ProcessChunkAsync(BigInteger start, BigInteger end)
        {
            await Task.Run(() =>
            {
                for (BigInteger number = start; number <= end; number++)
                {
                    if (IsPerfectNumber(number))
                    {
                        Console.WriteLine(number % 2 == 1
                            ? $"{number} is a spoof odd perfect number.\n"
                            : $"{number} is an even perfect number.\n");
                    }
                }
            });
        }

        private static bool IsPerfectNumber(BigInteger number)
        {
            Console.WriteLine("\r{0}   ", $"Working on number  {number}");
            BigInteger sum = 1; // Start with 1 since every number is divisible by 1
            var sqrt = (BigInteger)Math.Sqrt((double)number);


            for (BigInteger i = 2; i <= sqrt; i++)
            {
                if (number % i != 0) continue;
                sum += i;
                var div = number / i;

                if (i != div) // Add the other factor if it's not the square root
                    sum += div;
            }

            return sum == number;
        }
    }
}

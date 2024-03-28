namespace MonteCarloPiApprox
{
    internal class Program
    {
        private static readonly Random XCoord = new();
        private static readonly Random YCoord = new();
        static void Main()
        {
            double counter = 0;
            const double range = 5000;
            double printInterval = (range * .02);
            for (double i = 0; i < range; i++)
            {

                var newX = XCoord.NextDouble();
                var newY = YCoord.NextDouble();

                if (Math.Sqrt(((newX * newX) + (newY * newY))) <= 1)
                    counter++;

                if (i % printInterval == 0)
                {
                    Console.Write("\r{0}    ", $"{i / range * 100:n2}% done... drop #:{i} value: {4 * (counter / range)}");
                }

            }

            Console.Clear();
            Console.WriteLine($"Final PI Approx: {4 * (counter / range)}");
            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();
        }
    }
}

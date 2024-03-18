namespace NormalDistribution
{
    internal class Program
    {
        private static void Main()
        {
            var myGaltonBoard = new GaltonBoard(20, 1000000);
            myGaltonBoard.FlipBoard(3);
        }
    }
}

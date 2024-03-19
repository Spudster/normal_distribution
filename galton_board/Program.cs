namespace GaltonBoard
{
    internal class Program
    {
        private static void Main()
        {
            var myGaltonBoard = new classes.GaltonBoard(14, 20000);
            myGaltonBoard.FlipBoard(3);
        }
    }
}

namespace GaltonBoard
{
    internal class Program
    {
        private static void Main()
        {
            var myGaltonBoard = new GaltonBoard(14, 8000);
            myGaltonBoard.FlipBoard();
        }
    }
}

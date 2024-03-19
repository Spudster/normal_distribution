namespace GaltonBoard
{
    internal class Program
    {
        private static void Main()
        {
            var myGaltonBoard = new GaltonBoard(20, 8000);
            myGaltonBoard.FlipBoard();
        }
    }
}

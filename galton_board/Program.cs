namespace GaltonBoard
{
    internal class Program
    {
        private static void Main()
        {
            var myGaltonBoard = new classes.GaltonBoard(14, 8000);
            myGaltonBoard.FlipBoard();
        }
    }
}

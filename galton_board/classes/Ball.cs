﻿namespace GaltonBoard.classes
{
    internal class Ball
    {
        private readonly int _ballId;
        private readonly List<Coordinate> _pathCoordinatesTaken = new();
        private readonly List<Path> _pathTaken = new();

        public Ball(int ballId)
            => _ballId = ballId;

        public void AddCoordinate(Coordinate c)
            => _pathCoordinatesTaken.Add(c);

        public void AddPath(Path p)
            => _pathTaken.Add(p);

        /// <summary>
        /// Replays the path taken by the ball during its run in the virtual GaltonBoard
        /// </summary>
        public void ReplayPath()
        {
            Console.WriteLine($"Ball ID: {_ballId}");

            var lastElement = _pathCoordinatesTaken.Count - 1;
            for (var i = 0; i < _pathCoordinatesTaken.Count; i++)
            {
                var p = _pathCoordinatesTaken[i];
                if (i == lastElement)
                {
                    Console.WriteLine($"Row: {p.RowNumber} Channel#: {p.NodeNumber} Path: *LANDED*");
                    continue;
                }

                Console.WriteLine($"Row: {p.RowNumber} Col: {p.NodeNumber} Path: {_pathTaken[i]}");
            }

            var leftNum = 0;
            var rightNum = 0;

            foreach (var p in _pathTaken)
            {
                if (p == Path.Left)
                {
                    leftNum++;
                }
                else
                {
                    rightNum++;
                }
            }

            var absValue = Math.Abs(leftNum - rightNum);

            Console.WriteLine($"Paths Count: Lefts = {leftNum}, Rights = {rightNum} with L - R Difference Absolute Value {absValue}");
        }
    }
}

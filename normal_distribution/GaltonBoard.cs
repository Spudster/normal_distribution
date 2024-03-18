using System.Diagnostics;

namespace NormalDistribution
{
    internal class GaltonBoard
    {
        private readonly List<List<Node>> _board;
        private readonly int _rows;
        private readonly int _balls;
        private readonly float _ballsFloat;
        private bool _boardBuilt;
        private readonly Random _random;
        private int _intervals;

        public GaltonBoard(int rows, int balls)
        {
            _random = new Random();
            _board = new List<List<Node>>();
            _rows = (rows + 1);
            _balls = balls;
            _ballsFloat = _balls;
            BuildBoard();
        }

        private void BuildBoard()
        {
            Console.WriteLine($"*** Building Galton Board with {_rows - 1} rows and {_balls} balls");

            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < _rows; i++)
            {
                var intervalsPerRows = i + 1;
                var innerList = new List<Node>();

                for (var j = 0; j < intervalsPerRows; j++)
                {
                    innerList.Add(new Node(new Coordinate { RowNumber = i, NodeNumber = j }));
                }

                _board.Add(innerList);
            }

            sw.Stop();
            Console.WriteLine($"*** Build Completed in {sw.ElapsedMilliseconds} ms");
            _boardBuilt = true;
        }

        public void FlipBoard(int intervals = 1)
        {
            _intervals = intervals;

            for (var i = 0; i < _intervals; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"*** Flipping Board : {(i+1)}/{_intervals} times ***");
                Run();
            }
        }

        private void Run()
        {
            if (!_boardBuilt)
            {
                Console.WriteLine($"*** Galton Board has not been built yet");
                return;
            }

            var sw = new Stopwatch();
            sw.Start();

            var startNode = _board.First().First();
            var ballsProcessed = 1F;

            Parallel.For(0, _balls, _ =>
            {
                var vBall = new Ball((int)ballsProcessed);
                Console.Write("\r{0}   ", $"{((ballsProcessed / _ballsFloat) * 100):n2}% done ");
                var currentNode = new Node(startNode.GetMyCoordinate());

                for (var j = 0; j < _rows; j++)
                {
                    if (j == 0)
                        currentNode = startNode;

                    var currentRoll = RandomPath();

                    if (j == _rows - 1)
                    {
                        lock (startNode)
                        {
                            vBall.AddCoordinate(currentNode.GetMyCoordinate());
                            currentNode.AddBallToChannel(vBall);
                            currentNode.IncrementCount();
                        }
                        continue;
                    }

                    vBall.AddCoordinate(currentNode.GetMyCoordinate());

                    if (currentRoll == 0)//left
                    {
                        var leftCoordinate = currentNode.GetLeftNeighborCoordinate();
                        currentNode = GetSpecificNode(leftCoordinate.RowNumber, leftCoordinate.NodeNumber);
                        vBall.AddPath("left");
                    }
                    else // right
                    {
                        var rightCoordinate = currentNode.GetRightNeighborCoordinate();
                        currentNode = GetSpecificNode(rightCoordinate.RowNumber, rightCoordinate.NodeNumber);
                        vBall.AddPath("right");
                    }
                }

                ballsProcessed++;
            });

            sw.Stop();
            Console.WriteLine();
            Console.WriteLine("***");
            Console.WriteLine($"*** All Balls processed in {sw.ElapsedMilliseconds} ms ***");
            PrintResults();
            ResetBoard();
        }

        private void ResetBoard()
            => _board.Last().ForEach(_ => _.Reset());

        private Node GetSpecificNode(int rowNumber, int nodeNumber) 
            => _board[rowNumber][nodeNumber - 1];

        private void PrintResults()
        {
            Console.WriteLine();
            var ends = _board.Last();
            var channelNumber = 1;
            var middleChannel = (ends.Count / 2) + 1;
            foreach (var end in ends)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (channelNumber == middleChannel)
                    Console.ForegroundColor = ConsoleColor.Blue;

                var floatValue = (float)end.GetCount();
                Console.WriteLine("{0,-20} {1,-15} {2,-10}", $"ch: {channelNumber}", $"{end.GetCount()}", $"%{((floatValue / _ballsFloat) * 100):n4}");
                channelNumber++;
            }

            var channelCount = ends.Select(_ => _.GetCount()).Sum();
            Console.WriteLine($"***********************************************");
            Console.WriteLine();
            Console.WriteLine($"{channelCount} Balls in channels");


            Console.WriteLine($"*** Printing path of least Probably Channel Ball ***");

            var lestNode = ends.FirstOrDefault(_ => _.GetCount() > 0);
            if (lestNode == null) throw new Exception("Least Node Value cannot be null");

            for (var i = 1; i < ends.Count - 1; i++)
            {
                var current = ends[i];
                var next = ends[i + 1];

                if (current.GetCount() == 0)
                    continue;

                if (next.GetCount() == 0)
                    continue;

                var currCount = current.GetCount();
                var nextCount = next.GetCount();

                if (currCount < nextCount)
                {
                    if (currCount < lestNode.GetCount())
                    {
                        lestNode = current;
                        continue;
                    }
                }

                if (currCount == nextCount)
                {
                    continue;
                }

                if (currCount > nextCount)
                {
                    if (nextCount < lestNode.GetCount())
                    {
                        lestNode = next;
                    }
                }
            }

            var b = lestNode.GetBallsInChannel().First();
            b.ReplayPath();

            //Console.WriteLine($"*** Printing path of Middle Channel ***");
            //var m = ends[(ends.Count / 2)];

            //for (var i = 0; i < 3; i++)
            //{
            //    var current = m.GetBallsInChannel()[i];
            //    current.ReplayPath();
            //}

        }

        private int RandomPath() 
            => _random.Next(1, 101) <= 50 ? 0 : 1;
    }
}

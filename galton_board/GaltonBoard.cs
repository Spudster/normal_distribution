using System.Diagnostics;

namespace GaltonBoard
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

        public GaltonBoard(int rows = 14, int balls = 6000)
        {
            _random = new Random();
            _board = new List<List<Node>>();
            _rows = (rows + 1);
            _balls = balls;
            _ballsFloat = _balls;
            BuildBoard();
        }

        /// <summary>
        /// Builds Galton Board based on rows and amount of balls given as parameter values
        /// </summary>
        private void BuildBoard()
        {
            Console.WriteLine($"*** Building Galton Board with {_rows - 1} rows and {_balls} balls");

            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < _rows; i++)
            {
                var nodesPerRow = i + 1;
                var currentNodeRow = new List<Node>();

                for (var j = 0; j < nodesPerRow; j++)
                {
                    currentNodeRow.Add(new Node(j, new Coordinate { RowNumber = i, NodeNumber = j }));
                }

                _board.Add(currentNodeRow);
            }

            sw.Stop();
            Console.WriteLine($"*** Build Completed in {sw.ElapsedMilliseconds} ms");
            _boardBuilt = true;
        }

        /// <summary>
        /// Flips the board the amount of times given by 'interval' values.
        /// Acts as a different run.
        /// </summary>
        /// <param name="intervals"></param>
        public void FlipBoard(int intervals = 1)
        {
            _intervals = intervals;

            for (var i = 0; i < _intervals; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"*** Flipping Board : {(i + 1)}/{_intervals} times ***");
                RunSimulation();
            }
        }

        /// <summary>
        /// Galton board Simulation per ball
        /// </summary>
        private void RunSimulation()
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
            var channelNode = _rows - 1;

            Parallel.For(0, _balls, _ =>
            {
                Console.Write("\r{0}   ", $"{((ballsProcessed / _ballsFloat) * 100):n2}% done ");

                var vBall = new Ball((int)ballsProcessed);
                var currentNode = new Node(0, startNode.GetMyCoordinate());

                for (var j = 0; j < _rows; j++)
                {
                    if (j == 0)
                        currentNode = startNode;

                    var currentRoll = RandomPath();

                    if (j == channelNode)
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
            var channels = _board.Last();
            var channelNumber = 1;
            var middleChannel = (channels.Count / 2) + 1;
            foreach (var end in channels)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (channelNumber == middleChannel)
                    Console.ForegroundColor = ConsoleColor.Blue;

                var floatValue = (float)end.GetCount();
                Console.WriteLine("{0,-20} {1,-15} {2,-10}", $"ch: {channelNumber}", $"{end.GetCount()}", $"%{((floatValue / _ballsFloat) * 100):n4}");
                channelNumber++;
            }

            var channelCount = channels.Select(_ => _.GetCount()).Sum();
            Console.WriteLine($"***********************************************");
            Console.WriteLine();
            Console.WriteLine($"{channelCount} Balls in channels");


            Console.WriteLine($"*** Printing path of least Probably Channel Ball ***");

            PrintLeastProbablyBall(channels);

            //Console.WriteLine($"*** Printing path of Middle Channel ***");
            //var m = ends[(ends.Count / 2)];

            //for (var i = 0; i < 3; i++)
            //{
            //    var current = m.GetBallsInChannel()[i];
            //    current.ReplayPath();
            //}

        }

        private void PrintLeastProbablyBall(IReadOnlyList<Node> ends)
        {
            var channels = _board.Last();
            var middleChannel = (channels.Count / 2);
            var selectedNode = ends.FirstOrDefault(_ => _.GetCount() > 0);

            if (selectedNode == null)
                throw new Exception("Least Node Value cannot be null");

            var startIndex = selectedNode.Id;

            for (var i = startIndex; i < ends.Count - 1; i++)
            {
                var current = ends[i];

                var currentCount = current.GetCount();
                var selectedCount = selectedNode.GetCount();

                if (currentCount == 0)
                    continue;

                if (currentCount < selectedCount)
                    selectedNode = current;

                if (currentCount == selectedCount)
                {
                    var currentAbs = Math.Abs(current.Id - middleChannel);
                    var selectedAbs = Math.Abs(selectedNode.Id - middleChannel);

                    if (currentAbs > selectedAbs)
                        selectedNode = current;
                }
            }

            var b = selectedNode.GetBallsInChannel().First();
            b.ReplayPath();
        }

        private int RandomPath()
            => _random.Next(1, 101) <= 50 ? 0 : 1;
    }
}

namespace GaltonBoard
{
    internal class Node
    {
        public readonly int Id;
        private int _count;
        private List<Ball> _balls = new();
        private readonly List<Coordinate> _nodeNeighbors;
        private readonly Coordinate _nodeCoordinates;

        public Node(int id, Coordinate coordinate)
        {
            Id = id;
            _nodeCoordinates = new Coordinate { RowNumber = coordinate.RowNumber, NodeNumber = coordinate.NodeNumber + 1 };
            _nodeNeighbors = new List<Coordinate>();
            CalculateNeighborCoordinates();
        }

        public void IncrementCount()
            => _count++;

        /// <summary>
        /// End nodes act as channels where the balls land and are collected
        /// </summary>
        /// <param name="ball"></param>
        public void AddBallToChannel(Ball ball)
            => _balls.Add(ball);

        public List<Ball> GetBallsInChannel()
            => _balls;

        public void CalculateNeighborCoordinates()
        {
            _nodeNeighbors.Add(new Coordinate { RowNumber = _nodeCoordinates.RowNumber + 1, NodeNumber = _nodeCoordinates.NodeNumber });
            _nodeNeighbors.Add(new Coordinate { RowNumber = _nodeCoordinates.RowNumber + 1, NodeNumber = _nodeCoordinates.NodeNumber + 1 });
        }

        public Coordinate GetLeftNeighborCoordinate()
            => _nodeNeighbors.First();

        public Coordinate GetRightNeighborCoordinate()
            => _nodeNeighbors.Last();

        public Coordinate GetMyCoordinate()
            => _nodeCoordinates;

        public int GetCount()
            => _count;

        public void Reset()
        {
            _count = 0;
            _balls = new List<Ball>();
        }
    }
}

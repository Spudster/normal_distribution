namespace NormalDistribution
{
    internal class Node
    {
        private int _count;
        private List<Ball> _balls = new();
        private readonly List<Coordinate> _myNeighbors;
        private readonly Coordinate _myCoordinate;

        public Node(Coordinate coordinate)
        {
            _myCoordinate = new Coordinate { RowNumber = coordinate.RowNumber, NodeNumber = coordinate.NodeNumber + 1 };
            _myNeighbors = new List<Coordinate>();
            CalculateNeighbors();
        }

        public void AddToCount()
            => _count += 1;

        public void AddBallToChannel(Ball ball)
            => _balls.Add(ball);

        public List<Ball> GetBalls()
            => _balls;

        public void CalculateNeighbors()
        {
            _myNeighbors.Add(new Coordinate { RowNumber = _myCoordinate.RowNumber + 1, NodeNumber = _myCoordinate.NodeNumber });
            _myNeighbors.Add(new Coordinate { RowNumber = _myCoordinate.RowNumber + 1, NodeNumber = _myCoordinate.NodeNumber + 1 });
        }

        public Coordinate GetLeftNeighborCoordinate()
            => _myNeighbors.First();

        public Coordinate GetRightNeighborCoordinate()
            => _myNeighbors.Last();

        public Coordinate GetMyCoordinate()
            => _myCoordinate;

        public int GetCount()
            => _count;

        public void Reset()
        {
            _count = 0;
            _balls = new List<Ball>();
        }
    }
}

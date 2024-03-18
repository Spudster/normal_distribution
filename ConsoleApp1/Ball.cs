namespace NormalDistribution
{
    internal class Ball
    {
        private readonly int _ballId;
        private readonly List<Coordinate> _coordinates = new();
        private readonly List<string> _path = new();

        public Ball(int ballId)
            => _ballId = ballId;

        public void AddCoordinate(Coordinate c)
            => _coordinates.Add(c);

        public void AddPath(string p)
            => _path.Add(p);

        public void ReplayPath()
        {
            Console.WriteLine($"ID: {_ballId}");

            for (var i = 0; i < _coordinates.Count; i++)
            {
                var p = _coordinates[i];
                if (i == _coordinates.Count - 1)
                {
                    Console.WriteLine($"Row: {p.RowNumber} Channel#: {p.NodeNumber} Path: *LANDED*");
                    continue;
                }

                Console.WriteLine($"Row: {p.RowNumber} Col: {p.NodeNumber} Path: {_path[i]}");
            }

            var leftNum = 0;
            var rightNum = 0;

            foreach (var p in _path)
            {
                if (p.StartsWith("l"))
                {
                    leftNum++;
                }
                else
                {
                    rightNum++;
                }
            }

            Console.WriteLine($"Paths Count: Lefts = {leftNum}, Rights = {rightNum}");
        }
    }
}

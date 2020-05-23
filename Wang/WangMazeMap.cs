using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MapGenerator.Wang
{
    public class WangMazeMap : WangMap<WangBlobTile>
    {
        private readonly int _randomThreshold;
        private readonly bool _generateRooms;

        public WangMazeMap(int width, int height, int seed, int randomThreshold = 10, bool generateRooms = false) : base(width, height, seed)
        {
            _randomThreshold = randomThreshold;
            _generateRooms = generateRooms;
        }

        protected override WangBlobTile InvalidTile()
        {
            return WangBlobTile.Null;
        }

        protected override WangBlobTile CreateTile(Coordinate position)
        {
            return new WangBlobTile(0, false);
        }

        public override void Generate()
        {
            List<Coordinate> stack = new List<Coordinate>();

            var x = GetRandomNext(Width);
            var y = GetRandomNext(Height);

            Coordinate start = new Coordinate(x, y);
            stack.Add(start);

            RecurseGenerate(stack);
        }

        private int SelectNextIndex(ICollection visited)
        {
            if (GetRandomNext(100) < _randomThreshold)
            {
                return GetRandomNext(visited.Count);
            }

            return visited.Count - 1;
        }

        private void RecurseGenerate(List<Coordinate> stack)
        {
            if (stack.Count == 0)
            {
                return;
            }

            var startIndex = SelectNextIndex(stack);
            var startCoordinates = stack[startIndex];
            var start = GetTileAt(startCoordinates.X, startCoordinates.Y);

            var north = GetNeighborTile(startCoordinates, WangDirection.North);
            var south = GetNeighborTile(startCoordinates, WangDirection.South);
            var east = GetNeighborTile(startCoordinates, WangDirection.East);
            var west = GetNeighborTile(startCoordinates, WangDirection.West);

            var neighbors = new[] { north, south, east, west };
            neighbors = neighbors.Where(t => !t.Item1.IsNull && t.Item1.Index == 0).ToArray();

            if (neighbors.Length == 0)
            {
                stack.RemoveAt(startIndex);
                RecurseGenerate(stack);
                return;
            }

            var neighborIndex = GetRandomNext(neighbors.Length);
            var (neighbor, position, direction) = neighbors[neighborIndex];

            switch (direction)
            {
                case WangDirection.North:
                    ApplyNorthRule(start, neighbor);
                    break;
                case WangDirection.East:
                    ApplyEastRule(start, neighbor);
                    break;
                case WangDirection.South:
                    ApplySouthRule(start, neighbor);
                    break;
                case WangDirection.West:
                    ApplyWestRule(start, neighbor);
                    break;
            }

            stack.Add(position);
            RecurseGenerate(stack);
        }

        private void ApplyNorthRule(WangBlobTile start, WangBlobTile neighbor)
        {
            start.North = true;
            neighbor.South = true;

            if (!_generateRooms)
            {
                return;
            }
        }

        private void ApplyEastRule(WangBlobTile start, WangBlobTile neighbor)
        {
            start.East = true;
            neighbor.West = true;

            if (!_generateRooms)
            {
                return;
            }
        }

        private void ApplySouthRule(WangBlobTile start, WangBlobTile neighbor)
        {
            start.South = true;
            neighbor.North = true;

            if (!_generateRooms)
            {
                return;
            }
        }

        private void ApplyWestRule(WangBlobTile start, WangBlobTile neighbor)
        {
            start.West = true;
            neighbor.East = true;

            if (!_generateRooms)
            {
                return;
            }
        }
    }
}

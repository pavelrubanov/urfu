using Greedy.Architecture;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Greedy
{
    public class NotGreedyPathFinder : IPathFinder
    {
        private readonly Dictionary<Architecture.Point, Dictionary<Architecture.Point, PathWithCost>> _paths =
            new Dictionary<Architecture.Point, Dictionary<Architecture.Point, PathWithCost>>();
        private List<Architecture.Point> _bestPath = new List<Architecture.Point>();
        private int _maxChests;

        public List<Architecture.Point> FindPathToCompleteGoal(State state)
        {
            var currentPath = new List<Architecture.Point>();
            var pathFinder = new DijkstraPathFinder();
            var points = new List<Architecture.Point>(state.Chests) { state.Position };
            foreach (var point in points)
            {
                if (_paths != null && !_paths.ContainsKey(point)) _paths.Add(point, new Dictionary<Architecture.Point, PathWithCost>());
                foreach (var path in pathFinder.GetPathsByDijkstra(state, point, state.Chests))
                {
                    if (path.Start.Equals(path.End)) continue;
                    if (_paths != null) _paths[path.Start][path.End] = path;
                }
            }
            foreach (var path in _paths[state.Position])
            {
                var hashSet = new HashSet<Architecture.Point>();
                foreach (var pair in _paths[path.Key])
                    hashSet.Add(pair.Key);
                FindPath(state.Energy - path.Value.Cost, path.Key, hashSet, 1, new List<Architecture.Point> { state.Position, path.Key });
            }
            for (var i = 0; i < _bestPath.Count - 1; i++)
                currentPath = currentPath.Concat(_paths[_bestPath[i]][_bestPath[i + 1]].Path.Skip(1)).ToList();
            return currentPath;
        }

        private void FindPath(int currentEnergy, Architecture.Point currentPosition, IEnumerable<Architecture.Point> leftoverChests,
            int takenChests, List<Architecture.Point> points)
        {
            var chests = new HashSet<Architecture.Point>(leftoverChests);
            chests.Remove(currentPosition);
            foreach (var point in chests)
            {
                if (_paths[currentPosition][point].Cost > currentEnergy) continue;
                var newPath = new List<Architecture.Point>(points) { point };
                FindPath(currentEnergy - _paths[currentPosition][point].Cost, point, chests, takenChests + 1, newPath);
            }
            if (takenChests <= _maxChests) return;
            _maxChests = takenChests;
            _bestPath = points;
        }
    }
}

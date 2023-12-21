using System.Linq;
using System;

namespace Dungeon
{
    public static class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var startPoint = map.InitialPosition;
            var exitPoint = map.Exit;
            var chestsPoints = map.Chests;
            var routeWithoutChests = BfsTask.FindPaths(map, startPoint, new Point[] { exitPoint }).FirstOrDefault();

            if (routeWithoutChests == null)
                return new MoveDirection[0];

            var moveToExit = routeWithoutChests.ToList();
            moveToExit.Reverse();

            if (chestsPoints.Any(c => moveToExit.Contains(c)))
                return moveToExit.Zip(moveToExit.Skip(1), Move).ToArray();

            var routesStartToChests = BfsTask.FindPaths(map, startPoint, chestsPoints);
            var routesExitToChests = BfsTask.FindPaths(map, exitPoint, chestsPoints).DefaultIfEmpty();

            if (routesStartToChests.FirstOrDefault() == null)
                return moveToExit.Zip(moveToExit.Skip(1), Move).ToArray();

            var routesStartToExit = routesStartToChests.Join(
                routesExitToChests,
                f => f.Value,
                s => s.Value,
                (f, s) => new {
                    Length = f.Length + s.Length,
                    ListFinish = f.ToList(),
                    ListStart = s.ToList()
                }
            );

            var listsTuple = routesStartToExit.OrderBy(l => l.Length)
                .Select(v => Tuple.Create(v.ListFinish, v.ListStart)).First();

            listsTuple.Item1.Reverse();
            listsTuple.Item1.AddRange(listsTuple.Item2.Skip(1));

            return listsTuple.Item1.Zip(listsTuple.Item1.Skip(1), Move).ToArray();
        }

        private static MoveDirection Move(Point startPoint, Point finishPoint)
        {
            var direction = new Point(finishPoint.X - startPoint.X, finishPoint.Y - startPoint.Y);

            if (direction.X == 1)
                return MoveDirection.Right;

            if (direction.X == -1)
                return MoveDirection.Left;

            return direction.Y == 1 ? MoveDirection.Down : MoveDirection.Up;
        }
    }
}

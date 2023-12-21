using Greedy.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Greedy;

internal class DijkstraData
{
    public Point? ParentPoint { get; set; }
    public int TotalCost { get; set; }

}

internal static class PointExtensions
{
    public static IEnumerable<Point> GetNeighbours(this Point p)
    {
        yield return new Point(p.X + 1, p.Y);
        yield return new Point(p.X - 1, p.Y);
        yield return new Point(p.X, p.Y + 1);
        yield return new Point(p.X, p.Y - 1);
    }
}

public class DijkstraPathFinder
{

    public IEnumerable<PathWithCost> GetPathsByDijkstra(State gameState, Point startPoint, IEnumerable<Point> targetPoints)
    {
        var visitedPoints = new HashSet<Point>();
        var shortestPaths = new Dictionary<Point, DijkstraData>();
        shortestPaths[startPoint] = new DijkstraData { ParentPoint = null, TotalCost = 0 };

        while (true)
        {
            if (!targetPoints.Any())
                break;

            var currentPoint = GetPointWithShortestPath(shortestPaths, visitedPoints);
            if (currentPoint == null)
                break;

            if (targetPoints.Contains(currentPoint.Value))
            {
                targetPoints = targetPoints.Where(p => p != currentPoint);
                yield return new PathWithCost(shortestPaths[currentPoint.Value].TotalCost,
                    GetPathIterator(shortestPaths, currentPoint.Value).Reverse().ToArray());
            }

            visitedPoints.Add(currentPoint.Value);
            foreach (var neighbourPoint in currentPoint.Value.GetNeighbours().Where(n => gameState.InsideMap(n) && !gameState.IsWallAt(n)))
            {
                var newCost = shortestPaths[currentPoint.Value].TotalCost + gameState.CellCost[neighbourPoint.X, neighbourPoint.Y];
                if (!shortestPaths.ContainsKey(neighbourPoint) || shortestPaths[neighbourPoint].TotalCost > newCost)
                {
                    shortestPaths[neighbourPoint] = new DijkstraData { ParentPoint = currentPoint, TotalCost = newCost };
                }
            }
        }
    }

    private static IEnumerable<Point> GetPathIterator(Dictionary<Point, DijkstraData> shortestPaths, Point targetPoint)
    {
        for (Point? point = targetPoint; point != null; point = shortestPaths[point.Value].ParentPoint)
        {
            yield return point.Value;
        }
    }

    private static Point? GetPointWithShortestPath(Dictionary<Point, DijkstraData> shortestPaths, HashSet<Point> visitedPoints)
    {
        Point? pointWithShortestPath = null;
        int shortestPathCost = int.MaxValue;
        foreach (var point in shortestPaths.Keys.Where(p => !visitedPoints.Contains(p)))
        {
            if (shortestPaths[point].TotalCost < shortestPathCost)
            {
                pointWithShortestPath = point;
                shortestPathCost = shortestPaths[point].TotalCost;
            }
        }
        return pointWithShortestPath;
    }
}


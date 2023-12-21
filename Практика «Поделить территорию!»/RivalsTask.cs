using System.Collections.Generic;
using System.ComponentModel;

namespace Rivals;

public class RivalsTask
{
    public static IEnumerable<OwnedLocation> AssignOwners(Map map)
    {
        var players = map.Players;
        var visitedPoints = new HashSet<Point>();
        var locationQueue = new Queue<OwnedLocation>();

        for (var i = 0; i < players.Length; i++)
        {
            locationQueue.Enqueue(new OwnedLocation(i, players[i], 0));
        }

        foreach (var location in TraverseLocations(map, locationQueue, visitedPoints))
        {
            yield return location;
        }
    }

    private static IEnumerable<OwnedLocation> TraverseLocations(Map map, Queue<OwnedLocation> locationQueue, HashSet<Point> visitedPoints)
    {
        while (locationQueue.Count > 0)
        {
            var currentLocation = locationQueue.Dequeue();

            if (visitedPoints.Contains(currentLocation.Location) ||
              !map.InBounds(currentLocation.Location) ||
              map.Maze[currentLocation.Location.X, currentLocation.Location.Y] != MapCell.Empty)
            {
                continue;
            }
            visitedPoints.Add(currentLocation.Location);
            yield return currentLocation;

            AddAdjacentLocationsToQueue(locationQueue, currentLocation);
        }
    }

    private static void AddAdjacentLocationsToQueue(Queue<OwnedLocation> queue, OwnedLocation location)
    {
        for (var dy = -1; dy <= 1; dy++)
        {
            for (var dx = -1; dx <= 1; dx++)
            {
                if ((dy == 0 || dx == 0) && dy != dx)
                {
                    var adjacentLocation = new OwnedLocation(location.Owner,
                        new Point(location.Location.X + dx, location.Location.Y + dy),
                        location.Distance + 1);
                    queue.Enqueue(adjacentLocation);
                }
            }
        }
    }
}

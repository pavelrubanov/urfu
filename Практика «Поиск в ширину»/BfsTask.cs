using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map gameMap, Point startingPoint, Point[] treasureChests)
        {
            Dictionary<Point, SinglyLinkedList<Point>> mapTracking = new Dictionary<Point, SinglyLinkedList<Point>>();
            mapTracking[startingPoint] = new SinglyLinkedList<Point>(startingPoint);

            Queue<SinglyLinkedList<Point>> pointQueue = new Queue<SinglyLinkedList<Point>>();
            pointQueue.Enqueue(mapTracking[startingPoint]);

            foreach (Point chest in treasureChests)
            {
                if (mapTracking.ContainsKey(chest))
                {
                    yield return mapTracking[chest];
                    continue;
                }
                while (pointQueue.Count != 0)
                {
                    SinglyLinkedList<Point> currentNode = pointQueue.Dequeue();

                    IEnumerable<Point> adjacentNodes = Walker.PossibleDirections
                        .Select(direction => currentNode.Value + direction)
                        .Where(point => gameMap.InBounds(point) && gameMap.Dungeon[point.X, point.Y] != MapCell.Wall);

                    foreach (Point nextNode in adjacentNodes)
                    {
                        if (mapTracking.ContainsKey(nextNode)) continue;
                        mapTracking[nextNode] = new SinglyLinkedList<Point>(nextNode, currentNode);
                        pointQueue.Enqueue(mapTracking[nextNode]);
                    }

                    if (mapTracking.ContainsKey(chest))
                    {
                        yield return mapTracking[chest];
                        break;
                    }
                }
            }
        }
    }
}

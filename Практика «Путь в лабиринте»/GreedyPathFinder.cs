using System.Collections.Generic;
using System.Drawing;
using Greedy.Architecture;
using System.Linq;

namespace Greedy;

    public class GreedyPathFinder : IPathFinder
{
    public List<Architecture.Point> FindPathToCompleteGoal(State gameState)
    {
        var optimalPath = new List<Architecture.Point>();

        var startingEnergy = gameState.InitialEnergy;
        var dijkstra = new DijkstraPathFinder();
        var goalPath = new List<Architecture.Point>();

        var chests = new List<Architecture.Point> (gameState.Chests);

        if (chests.Count < gameState.Goal)
        {
            return optimalPath;
        }

        while (gameState.Scores < gameState.Goal)
        {
            var pathsToChests = dijkstra.GetPathsByDijkstra(gameState, gameState.Position, chests);

            var pathToBestChest = GetPathToBestChest(pathsToChests.ToList());

            if (pathToBestChest == null)
            {
                return optimalPath;
            }

            var energyLeft = startingEnergy - pathToBestChest.Cost;

            if (energyLeft < 0)
            {
                return optimalPath;
            }

            UpdateGameStateAndPath(gameState, goalPath, pathToBestChest);

            chests.Remove(gameState.Position);
            gameState.Scores++;
        }

        return goalPath;
    }


    private PathWithCost GetPathToBestChest(List<PathWithCost> pathsToChests)
    {
        var pathToBestChest = pathsToChests.SkipWhile(path => path.Path == new List<Architecture.Point>()).FirstOrDefault();

        return pathToBestChest;
    }

    private void UpdateGameStateAndPath(State gameState, List<Architecture.Point> goalPath, PathWithCost pathToBestChest)
    {
        goalPath.AddRange(pathToBestChest.Path.Skip(1));
        gameState.Position = pathToBestChest.Path.Last();
    }
}

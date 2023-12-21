using System;

class Program
{
    static void Main(string[] args)
    {
        TestDijkstra();
    }

    static void TestDijkstra()
    {
        int[][] graph = new int[][] {
            new int[] {0, 10, -1, 30, 100},
            new int[] {-1, 0, 50, -1, -1},
            new int[] {-1, -1, 0, -1, 10},
            new int[] {-1, -1, 20, 0, 60},
            new int[] {-1, -1, -1, -1, 0}
        };

        int expectedDistance = 60;

        int actualDistance = Dijkstra(graph);

        if (actualDistance == expectedDistance)
        {
            Console.WriteLine("Test passed.");
        }
        else
        {
            Console.WriteLine("Test failed. Expected: {0}, but got: {1}.", expectedDistance, actualDistance);
        }
    }

    static int Dijkstra(int[][] graph)
    {
        int N = graph.Length;

        int[] distances = new int[N]; // Расстояние от первого города до всех остальных
        bool[] visited = new bool[N]; // Посещенные города

        // Инициализация расстояний и пометка всех городов как непосещенных
        for (int i = 0; i < N; i++)
        {
            distances[i] = int.MaxValue;
            visited[i] = false;
        }

        distances[0] = 0; // Расстояние от первого города до него самого равно 0

        for (int i = 0; i < N - 1; i++)
        {
            int u = -1;

            // Ищем ближайший непосещенный город
            for (int j = 0; j < N; j++)
            {
                if (!visited[j] && (u == -1 || distances[j] < distances[u]))
                {
                    u = j;
                }
            }

            visited[u] = true;

            // Обновляем расстояния до соседей текущего города
            for (int v = 0; v < N; v++)
            {
                if (graph[u][v] != -1 && distances[u] + graph[u][v] < distances[v])
                {
                    distances[v] = distances[u] + graph[u][v];
                }
            }
        }

        return distances[N - 1];
    }
}

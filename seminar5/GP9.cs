using System;
using System.Collections.Generic;

class Graph
{
    private Dictionary<int, List<int>> adjacencyList;

    public Graph()
    {
        adjacencyList = new Dictionary<int, List<int>>();
    }

    public void AddVertex(int vertex)
    {
        adjacencyList[vertex] = new List<int>();
    }

    public void AddEdge(int source, int destination)
    {
        adjacencyList[source].Add(destination);
    }

    public int? FindStartVertex()
    {
        // Создание множества вершин
        HashSet<int> vertices = new HashSet<int>(adjacencyList.Keys);

        // Итерация по вершинам
        foreach (int vertex in vertices)
        {
            HashSet<int> visited = new HashSet<int>();
            DFS(vertex, visited);

            if (visited.Count == vertices.Count)
            {
                return vertex;
            }
        }

        return null;
    }

    private void DFS(int vertex, HashSet<int> visited)
    {
        // Добавление текущей вершины в множество посещённых
        visited.Add(vertex);

        // Итерация по соседним вершинам
        foreach (int neighbor in adjacencyList[vertex])
        {
            // Рекурсивный вызов для соседней вершины, если она ещё не была посещена
            if (!visited.Contains(neighbor))
            {
                DFS(neighbor, visited);
            }
        }
    }
}

class GP9
{
    public static void Main(string[] args)
    {
        // Создание графа
        Graph graph = new Graph();
        graph.AddVertex(1);
        graph.AddVertex(2);
        graph.AddVertex(3);
        graph.AddVertex(4);
        graph.AddVertex(5);
        graph.AddEdge(1, 2);
        graph.AddEdge(1, 3);
        graph.AddEdge(2, 4);
        graph.AddEdge(3, 4);
        graph.AddEdge(3, 5);
        graph.AddEdge(4, 5);

        // Поиск вершины, из которой достижимы все остальные
        int? startVertex = graph.FindStartVertex();

        if (startVertex.HasValue)
        {
            Console.WriteLine($"Вершина {startVertex.Value} достижима из всех остальных");
        }
        else
        {
            Console.WriteLine("Нет вершины, из которой достижимы все остальные");
        }
        Console.ReadLine();
    }
}

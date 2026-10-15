// Lab 5
// Student name: Christina Moolman
// Student number:29382975

using System;
using System.Collections.Generic;

Console.WriteLine("CPEN223 Lab 5");

// Testing: Write test cases that exercise all four methods.

// Test Graph 1: Connected graph
Dictionary<string, HashSet<string>> graph1 = new()
{
    ["A"] = new() { "B", "C" },
    ["B"] = new() { "A", "D" },
    ["C"] = new() { "A", "D" },
    ["D"] = new() { "B", "C", "E" },
    ["E"] = new() { "D" }
};

// Test 1: ReachableFrom
HashSet<string> reachable1 = GraphLibrary.ReachableFrom(graph1, "A");

Console.WriteLine("Test 1: ReachableFrom");
Console.WriteLine($"Expected count: 5, Actual: {reachable1.Count}");

// Test 2: CountComponents
Console.WriteLine("Test 2: CountComponents");
Console.WriteLine($"Expected: 1, Actual: {GraphLibrary.CountComponents(graph1)}");

// Test 3: ShortestDistance
Console.WriteLine("Test 3: ShortestDistance");
Console.WriteLine($"Expected: 3, Actual: {GraphLibrary.ShortestDistance(graph1, "A", "E")}");

// Test 4: Same start and target
Console.WriteLine("Test 4: Same vertex");
Console.WriteLine($"Expected: 0, Actual: {GraphLibrary.ShortestDistance(graph1, "C", "C")}");


// Test Graph 2: Disconnected graph
Dictionary<string, HashSet<string>> graph2 = new()
{
    ["A"] = new() { "B" },
    ["B"] = new() { "A", "C" },
    ["C"] = new() { "B" },
    ["D"] = new() { "E" },
    ["E"] = new() { "D" },
    ["F"] = new()
};

// Test 5: Disconnected components
Console.WriteLine("Test 5: Disconnected graph");
Console.WriteLine($"Expected: 3, Actual: {GraphLibrary.CountComponents(graph2)}");

// Test 6: Reachable vertices
HashSet<string> reachable2 = GraphLibrary.ReachableFrom(graph2, "D");
Console.WriteLine("Test 6: ReachableFrom");
Console.WriteLine($"Expected count: 2, Actual: {reachable2.Count}");
Console.WriteLine($"Contains D: {reachable2.Contains("D")}");
Console.WriteLine($"Contains E: {reachable2.Contains("E")}");

// Test 7: Unreachable target
Console.WriteLine("Test 7: Unreachable target");
Console.WriteLine($"Expected: -1, Actual: {GraphLibrary.ShortestDistance(graph2, "A", "E")}");

// Test 8: Isolated vertex
Console.WriteLine("Test 8: Isolated vertex");
Console.WriteLine($"Expected count: 1, Actual: {GraphLibrary.ReachableFrom(graph2, "F").Count}");

// Test Graph 3: Empty graph
Dictionary<string, HashSet<string>> emptyGraph = new();

Console.WriteLine("Test 9: Empty graph");
Console.WriteLine($"Expected: 0, Actual: {GraphLibrary.CountComponents(emptyGraph)}");

// Test 10: Invalid start vertex
Console.WriteLine("Test 10: Invalid start");
try
{
    GraphLibrary.ReachableFrom(graph1, "Z");
    Console.WriteLine("Expected ArgumentException, but none was thrown");
}
catch (ArgumentException)
{
    Console.WriteLine("Expected: ArgumentException, Actual: ArgumentException");
}

// end Testing code


// Do not change the program skeleton.

public static class GraphLibrary
{
    static bool IsConnected(
        IReadOnlyDictionary<string, HashSet<string>> graph,
        string start)
    {
        if (graph.Count == 0)
            return true;

        if (!graph.ContainsKey(start))
            throw new ArgumentException("start not in graph");

        HashSet<string> visited = new();
        Stack<string> work = new();
        work.Push(start);

        while (work.Count > 0)
        {
            string v = work.Pop();

            if (!visited.Add(v))
                continue;

            foreach (string n in graph[v])
                work.Push(n);
        }

        return visited.Count == graph.Count;
    }


    public static HashSet<string> ReachableFrom(
        IReadOnlyDictionary<string, HashSet<string>> graph,
        string start)
    {
        if (!graph.ContainsKey(start))
            throw new ArgumentException("start not in graph");

        HashSet<string> visited = new();
        Stack<string> work = new();

        work.Push(start);

        while (work.Count > 0)
        {
            string v = work.Pop();

            if (!visited.Add(v))
                continue;

            foreach (string n in graph[v])
            {
                work.Push(n);
            }
        }

        return visited;
    }


    public static int CountComponents(
        IReadOnlyDictionary<string, HashSet<string>> graph)
    {
        HashSet<string> visited = new();
        int components = 0;

        foreach (string vertex in graph.Keys)
        {
            if (!visited.Contains(vertex))
            {
                HashSet<string> reachable = ReachableFrom(graph, vertex);

                foreach (string v in reachable)
                {
                    visited.Add(v);
                }

                components++;
            }
        }

        return components;
    }


    public static int ShortestDistance(
        IReadOnlyDictionary<string, HashSet<string>> graph,
        string start,
        string target)
    {
        if (!graph.ContainsKey(start) || !graph.ContainsKey(target))
            throw new ArgumentException("start or target not in graph");

        if (start == target)
            return 0;

        HashSet<string> visited = new();
        Queue<string> work = new();
        Dictionary<string, int> distance = new();

        work.Enqueue(start);
        visited.Add(start);
        distance[start] = 0;

        while (work.Count > 0)
        {
            string v = work.Dequeue();

            foreach (string n in graph[v])
            {
                if (!visited.Contains(n))
                {
                    visited.Add(n);
                    distance[n] = distance[v] + 1;

                    if (n == target)
                        return distance[n];

                    work.Enqueue(n);
                }
            }
        }

        return -1;
    }
}

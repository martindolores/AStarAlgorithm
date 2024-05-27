namespace Task1_2D
{
    public class AStarAlgorithm
    {
        private readonly int _totalVertices = 9;

        public void Run(int[,] graph, int source, int target)
        {
            // Initialize the open list (priority queue) and closed list (set of visited vertices)
            var openList = new PriorityQueue<int>();
            var closedList = new HashSet<int>();

            // Initialize the cost array (g-score) and heuristic array (h-score) for each vertex
            var gScore = new int[_totalVertices];
            var hScore = new int[_totalVertices];
            // Parent array to store the parent of each vertex in the shortest path tree
            var parent = new int[_totalVertices];

            // Initialize the g-score for all vertices to infinity
            for (int i = 0; i < _totalVertices; i++)
                gScore[i] = int.MaxValue;

            // The g-score of the source vertex is 0
            gScore[source] = 0;

            // Calculate the heuristic (h-score) for each vertex (distance from the vertex to the target)
            for (int i = 0; i < _totalVertices; i++)
            {
                hScore[i] = CalculateHeuristic(i, target);
            }

            // Add the source vertex to the open list with f-score (g-score + h-score)
            openList.Enqueue(source, gScore[source] + hScore[source]);

            while (!openList.IsEmpty)
            {
                // Extract the vertex with the lowest f-score from the open list
                int currentVertex = openList.Dequeue();

                // If the current vertex is the target, we have found the shortest path
                if (currentVertex == target)
                {
                    Console.WriteLine("Shortest Path Found!");
                    PrintShortestPath(parent, source, target);
                    return;
                }

                // Add the current vertex to the closed list
                closedList.Add(currentVertex);

                // Explore neighbors of the current vertex
                for (int neighbor = 0; neighbor < _totalVertices; neighbor++)
                {
                    if (graph[currentVertex, neighbor] != 0 && !closedList.Contains(neighbor))
                    {
                        // Calculate tentative g-score for the neighbor
                        int tentativeGScore = gScore[currentVertex] + graph[currentVertex, neighbor];

                        // If the tentative g-score is better than the current g-score of the neighbor, update it
                        if (tentativeGScore < gScore[neighbor])
                        {
                            // Update parent of the neighbor
                            parent[neighbor] = currentVertex;
                            gScore[neighbor] = tentativeGScore;

                            // Add the neighbor to the open list with updated f-score
                            openList.Enqueue(neighbor, gScore[neighbor] + hScore[neighbor]);
                        }
                    }
                }
            }

            // If the target is not reachable from the source
            Console.WriteLine("No Path Found!");
        }

        // Calculate heuristic using straight-line distance (Euclidean distance) from the vertex to the target
        private int CalculateHeuristic(int vertex, int target)
        {
            // Assuming each vertex has coordinates (x, y), calculate Euclidean distance
            // For simplicity, we'll use the Manhattan distance here as a heuristic
            // The Manhattan distance is the sum of the absolute differences of the coordinates
            // Here, we assume each vertex is indexed in a 2D grid, and we calculate the Manhattan distance
            // You can replace this with a more accurate heuristic if you have the coordinates of the vertices
            int vertexX = vertex % 3; // Assuming 3 columns
            int vertexY = vertex / 3; // Assuming 3 columns
            int targetX = target % 3; // Assuming 3 columns
            int targetY = target / 3; // Assuming 3 columns

            return Math.Abs(vertexX - targetX) + Math.Abs(vertexY - targetY);
        }

        // Helper method to print the shortest path from source to target
        private void PrintShortestPath(int[] parent, int source, int target)
        {
            List<int> path = new List<int>();
            int currentVertex = target;
            while (currentVertex != source)
            {
                path.Add(currentVertex);
                currentVertex = parent[currentVertex];
            }
            path.Add(source);
            path.Reverse();

            Console.WriteLine("Shortest Path:");

            foreach (var vertex in path)
            {
                if (path[path.Count - 1] == vertex)
                {
                    Console.Write(vertex);
                    break;
                }

                Console.Write(vertex + " -> ");
            }

            Console.WriteLine();
        }
    }

    // Priority queue implementation
    public class PriorityQueue<T>
    {
        private readonly SortedDictionary<int, Queue<T>> _items = new SortedDictionary<int, Queue<T>>();

        public void Enqueue(T item, int priority)
        {
            if (!_items.ContainsKey(priority))
                _items[priority] = new Queue<T>();

            _items[priority].Enqueue(item);
        }

        public T Dequeue()
        {
            var item = _items[_items.Keys.Min()].Dequeue();
            if (_items[_items.Keys.Min()].Count == 0)
                _items.Remove(_items.Keys.Min());
            return item;
        }

        public bool IsEmpty => _items.Count == 0;
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Alogrithms.GraphTheory
{
    class MaxFlowFordFulkerson
    {
        readonly int[,] adjMatrix;
        readonly int N;
        readonly int SINK;
        readonly int SOURCE;
        public int MaxFlow;

        public MaxFlowFordFulkerson(int[,] matrix, int source, int sink)
        {
            adjMatrix = matrix;
            N = matrix.GetLength(0);
            SINK = sink;
            SOURCE = source;
            solve();
        }

        /* Breadth first search algorithm to bypass graph.
           It returns true if there is path from source to sink
           else false. Also store path.*/
        private bool BFS(int[,] matrix, int source, int sink, int[] path)
        {
            /*Find augmenting path. Agumenting path can be done
             in two ways: 
             1. Non-full forward edges
             2. Non-empty backward edges*/

            bool[] visited = new bool[N];
            visited[source] = true;

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                int node = queue.Dequeue();

                for (int next = 0; next < N; next++)
                {
                    if (matrix[node, next] > 0 && !visited[next]) //if there is available  path from node to next 
                                                                  //and if the next wasn't be visited
                    {
                        visited[next] = true;
                        queue.Enqueue(next);
                        path[next] = node;
                    }

                }

            }

            return visited[sink];
        }

        private void solve()
        {
            int[,] rMatrix = adjMatrix.Clone() as int[,];
            int[] path = new int[N];

            while (BFS(rMatrix, SOURCE, SINK, path))
            {
                // find bottleneck capacity 
                int minValue = int.MaxValue;

                for (int node = SINK; node != SOURCE; node = path[node])
                {
                    int prev = path[node];
                    minValue = Math.Min(minValue, rMatrix[prev, node]);
                }

                // update residual graph
                for (int node = SINK; node != SOURCE; node = path[node])
                {
                    int prev = path[node];
                    rMatrix[prev, node] -= minValue; // residual capacity
                    rMatrix[node, prev] += minValue; // reverse capacity
                }

                MaxFlow += minValue;
            }

        }
    }

    class MaxFlowTest
    {
        static void Main(string[] args)
        {
            int[,] graph = { {0, 16, 13, 0, 0, 0},
                            {0, 0, 10, 12, 0, 0},
                            {0, 4, 0, 0, 14, 0},
                            {0, 0, 9, 0, 0, 20},
                            {0, 0, 0, 7, 0, 4},
                            {0, 0, 0, 0, 0, 0} };
            MaxFlowFordFulkerson maxFlow = new MaxFlowFordFulkerson(graph, 0, 5);
            Console.WriteLine(maxFlow.MaxFlow);

        }
    }
}

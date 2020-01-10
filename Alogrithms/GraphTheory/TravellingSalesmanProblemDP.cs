using System;
using System.Collections.Generic;
using System.Text;

namespace Alogrithms.GraphTheory
{
    class TravellingSalesmanProblemDP
    {
        private readonly int N;
        private readonly int START_NODE;
        private readonly int FINISH_STATE;
        private int MinCost;
        private List<int> Path;
        private int[,] adjMatrix;
        private bool isComputed;

        public TravellingSalesmanProblemDP(int startNode, int[,] adjMatrix)
        {
            this.adjMatrix = adjMatrix.Clone() as int[,];
            START_NODE = startNode;
            N = adjMatrix.GetLength(0);
            
            if (N <= 2)
            {
                throw new Exception("(N <= 2) it doesn't make sense");
            }
            else if (N > 32)
            {
                throw new Exception("(N > 32) Matrix too large");
            }
            else if (START_NODE > 0 || START_NODE >= N)
            {
                throw new Exception("The start node must be (0 <= s < N)");
            }

            
            FINISH_STATE = (1 << N) - 1; //state when all bits are set to one
        }
        public int GetMinCost()
        {
            if (!isComputed)
            {
                TSP();
            }
            return MinCost;
        }

        public List<int> GetToutPath()
        {
            if (!isComputed)
            {
                TSP();
            }
            return Path;
        }

        private void TSP()
        {
            int state = 1 << START_NODE; // set index of start node bit as 1

            /* (1 << N) is 2^N since this is tree we declare the
            maximum amount of memory that can be used*/
            int[,] cost = new int[N, 1 << N]; 
            int[,] path = new int[N, 1 << N];

            MinCost = solve(START_NODE, state, cost, path);

            // restore path
            Path = new List<int>();
            int index = START_NODE;
            
            while (true)
            {
                Path.Add(index);
                int next = path[index, state];
                if (next == 0)
                {
                    break;
                }
                index = next;
                state = state | (1 << next);
            }
            Path.Add(START_NODE);

            isComputed = true;
        }
        
        private int solve(int node, int state, int[,] cost, int[,] path)
        {
            if (state == FINISH_STATE) // The tour is done 
            {
                return adjMatrix[node, START_NODE];
            }
            if (cost[node, state] != 0) // return if this state already was computed
            {
                return cost[node, state];
            }


            int minCost = int.MaxValue;
            int index = -1;
            for (int next = 0; next < N; next++)
            {
                if ((state & (1 << next)) != 0) // skip if this node has already been visited
                {
                    continue;
                }

                int newState = state | (1 << next); // bitwise operation to set next node as 1
                int newCost = adjMatrix[node, next] + solve(next, newState, cost, path); 

                if (newCost < minCost)
                {
                    minCost = newCost;
                    index = next;
                }
            }
            path[node, state] = index;
            cost[node, state] = minCost;

            return cost[node, state];
        }
    }

    class TspTest
    {
        static void Main(string[] args)
        {
            int[,] matrix = { { 0, 10, 15, 20},
                              { 10, 0, 35, 25},
                              { 15, 35, 0, 30},
                              { 20, 25, 30, 0} };
 
            TravellingSalesmanProblemDP tsp = new TravellingSalesmanProblemDP(0, matrix);
            Console.WriteLine(tsp.GetMinCost());
            List<int> Path = tsp.GetToutPath();

            foreach (int node in Path)
            {
                Console.Write("{0} ", node);
            }

        }
    }
}

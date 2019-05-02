using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;

namespace Toolbox
{
    /// <summary>
    /// A* search based off Amit Patel's implementation.
    /// </summary>
    /// <remarks>
    /// Patel's implementation is different than what you see in most algorithms and AI textbooks.
    /// https://www.redblobgames.com/pathfinding/a-star/implementation.html
    /// </remarks>
    public static class AStar
    {
        public static LinePath FindPathClosest(Tilemap map, Vector3 start, Vector3 goal)
        {
            return FindPathClosest(map, start, goal, null);
        }

        public static LinePath FindPathClosest(Tilemap map, Vector3 start, Vector3 goal, Tile self)
        {
            LinePath lp = null;

            Vector3Int s = map.WorldToCell(start);
            Vector3Int g = map.WorldToCell(goal);

            if (!map.IsCellEmpty(g))
            {
                g = ClosestCell(OpenCells(map, g, self), s, g);
            }

            List<Vector3Int> path = AStar.FindPath(new MoveGraph(map), s, g, Vector3Int.Distance);

            if (path != null)
            {
                List<Vector3> nodes = new List<Vector3>(path.Capacity);

                foreach (Vector3Int v in path)
                {
                    nodes.Add(map.GetCellCenterWorld(v));
                }

                lp = new LinePath(nodes);
            }

            return lp;
        }

        //TODO should I just make a custom traversal here instead trying to reuse BreadthFirst?
        static HashSet<Vector3Int> OpenCells(Tilemap map, Vector3Int pos, Tile self)
        {
            Dictionary<Vector3Int, int> counts = new Dictionary<Vector3Int, int>();
            counts.Add(pos, 0);

            HashSet<Vector3Int> openCells = new HashSet<Vector3Int>();

            float minDist = Mathf.Infinity;
            int minCount = int.MaxValue;

            map.BreadthFirstTraversal(pos, Utils.FourDirections, (current, next) =>
            {
                float dist = Vector3Int.Distance(pos, next);
                int count = counts[current] + 1;
                counts[next] = count;

                if ((map.IsCellEmpty(next) || map.GetTile(next) == self) && dist <= minDist)
                {
                    minDist = dist;
                    minCount = count;
                    openCells.Add(next);
                }

                return count <= minCount;
            });

            return openCells;
        }

        static Vector3Int ClosestCell(HashSet<Vector3Int> openCells, Vector3Int start, Vector3Int goal)
        {
            Vector3Int closest = goal;
            float minDist = Mathf.Infinity;

            foreach (Vector3Int c in openCells)
            {
                float dist = Vector3Int.Distance(start, c);

                if (dist < minDist)
                {
                    minDist = dist;
                    closest = c;
                }
            }

            return closest;
        }

        /// <summary>
        /// Finds a path in the tilemap using world coordinates.
        /// </summary>
        public static List<Vector3> FindPath(Tilemap map, Vector3 start, Vector3 goal)
        {
            List<Vector3> result = null;

            List<Vector3Int> path = FindPath(map, map.WorldToCell(start), map.WorldToCell(goal));

            if (path != null)
            {
                result = new List<Vector3>(path.Capacity);

                foreach (Vector3Int v in path)
                {
                    result.Add(map.GetCellCenterWorld(v));
                }
            }

            return result;
        }

        /// <summary>
        /// Finds a path in the tilemap using cell coordinates.
        /// </summary>
        public static List<Vector3Int> FindPath(Tilemap map, Vector3Int start, Vector3Int goal)
        {
            return FindPath(new FourDirectionGraph(map), start, goal, Vector3Int.Distance);
        }

        /// <summary>
        /// Finds a path in the graph using cell coordinates.
        /// </summary>
        public static List<Vector3Int> FindPath(IGraph graph, Vector3Int start, Vector3Int goal, Func<Vector3Int, Vector3Int, float> heuristic)
        {
            PriorityQueue<Vector3Int> open = new PriorityQueue<Vector3Int>();
            open.Enqueue(start, 0);

            Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
            cameFrom[start] = start;

            Dictionary<Vector3Int, float> costSoFar = new Dictionary<Vector3Int, float>();
            costSoFar[start] = 0;

            while (open.Count > 0)
            {
                Vector3Int current = open.Dequeue();

                if (current == goal)
                {
                    break;
                }

                foreach (Vector3Int next in graph.Neighbors(current))
                {
                    float newCost = costSoFar[current] + graph.Cost(current, next);

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        float priority = newCost + heuristic(next, goal);
                        open.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            List<Vector3Int> path = null;

            if (cameFrom.ContainsKey(goal))
            {
                path = new List<Vector3Int>();

                Vector3Int v = goal;

                while (v != start)
                {
                    path.Add(v);
                    v = cameFrom[v];
                }

                path.Add(start);

                path.Reverse();
            }

            return path;
        }
    }
}
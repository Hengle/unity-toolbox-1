using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Toolbox
{
    public static class TilemapExtensions
    {
        public static bool IsInBounds(this Tilemap tilemap, Vector3Int position)
        {
            return tilemap.cellBounds.Contains(position);
        }

        public static bool IsInBounds(this Tilemap tilemap, Vector3 position)
        {
            Vector3Int pos = tilemap.WorldToCell(position);
            return tilemap.cellBounds.Contains(pos);
        }

        /// <summary>
        /// Checks if the cell is in bounds and is not set with a tile.
        /// </summary>
        public static bool IsCellEmpty(this Tilemap tilemap, Vector3Int position)
        {
            return tilemap.IsInBounds(position) && tilemap.GetTile(position) == null;
        }

        public static void DebugDraw(this Tilemap tilemap, float size, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
        {
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];

                    if (tile != null)
                    {
                        Vector3Int cell = new Vector3Int(bounds.x + x, bounds.y + y, bounds.z);
                        Vector3 pos = tilemap.GetCellCenterWorld(cell);

                        Utils.DebugDrawCross(pos, size, color, duration, depthTest);
                    }
                }
            }
        }

        /// <summary>
        /// A Breadth First Search of nodes in a grid.
        /// </summary>
        /// <param name="position">Starting position of the search</param>
        /// <param name="directions">The directions the traversal can go to find connected nodes</param>
        /// <param name="isGoal">A function to decide if the goal of the search has been found</param>
        /// <param name="isConnected">A function to decide if the next node is connected to the current node</param>
        public static Vector3Int? BreadthFirstSearch(Vector3Int position, Vector3Int[] directions, Func<Vector3Int, bool> isGoal, Func<Vector3Int, Vector3Int, bool> isConnected)
        {
            Queue<Vector3Int> queue = new Queue<Vector3Int>();
            queue.Enqueue(position);

            HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

            while (queue.Count > 0)
            {
                Vector3Int node = queue.Dequeue();

                if (isGoal(node))
                {
                    return node;
                }

                foreach (Vector3Int dir in directions)
                {
                    Vector3Int next = node + dir;

                    if (isConnected(node, next) && !visited.Contains(next))
                    {
                        queue.Enqueue(next);
                    }
                }

                visited.Add(node);
            }

            return null;
        }

        /// <summary>
        /// A Breadth First Search of nodes in a grid.
        /// </summary>
        /// <param name="position">Starting position of the search</param>
        /// <param name="directions">The directions the traversal can go to find connected nodes</param>
        /// <param name="isGoal">A function to decide if the goal of the search has been found</param>
        /// <param name="isConnected">A function to decide if the next node is connected to the current node</param>
        public static Vector3Int? BreadthFirstSearch(this Tilemap tilemap, Vector3Int position, Vector3Int[] directions, Func<Vector3Int, bool> isGoal, Func<Vector3Int, Vector3Int, bool> isConnected)
        {
            return BreadthFirstSearch(position, directions, isGoal, isConnected);
        }

        /// <summary>
        /// A Breadth First Search of nodes in a grid.
        /// </summary>
        /// <param name="position">Starting position of the search</param>
        /// <param name="directions">The directions the traversal can go to find connected nodes</param>
        /// <param name="isGoal">A function to decide if the goal of the search has been found</param>
        /// <param name="isConnected">A function to decide if the next node is connected to the current node</param>
        public static Vector3? BreadthFirstSearch(this Tilemap tilemap, Vector3 position, Vector3Int[] directions, Func<Vector3Int, bool> isGoal, Func<Vector3Int, Vector3Int, bool> isConnected)
        {
            Vector3Int start = tilemap.WorldToCell(position);

            Vector3Int? resultInt = BreadthFirstSearch(start, directions, isGoal, isConnected);

            Vector3? result = null;

            if (resultInt.HasValue)
            {
                result = tilemap.GetCellCenterWorld(resultInt.Value);
            }

            return result;
        }

        /// <summary>
        /// A Breadth First Search of nodes in a grid.
        /// </summary>
        /// <param name="position">Starting position of the search</param>
        /// <param name="isGoal">A function to decide if the goal of the search has been found</param>
        /// <param name="isConnected">A function to decide if the next node is connected to the current node</param>
        public static Vector3Int? BreadthFirstSearch(this Tilemap tilemap, Vector3Int position, Func<Vector3Int, bool> isGoal, Func<Vector3Int, Vector3Int, bool> isConnected)
        {
            return BreadthFirstSearch(position, Utils.FourDirections, isGoal, isConnected);
        }

        /// <summary>
        /// A Breadth First Search of nodes in a grid.
        /// </summary>
        /// <param name="position">Starting position of the search</param>
        /// <param name="isGoal">A function to decide if the goal of the search has been found</param>
        /// <param name="isConnected">A function to decide if the next node is connected to the current node</param>
        public static Vector3? BreadthFirstSearch(this Tilemap tilemap, Vector3 position, Func<Vector3Int, bool> isGoal, Func<Vector3Int, Vector3Int, bool> isConnected)
        {
            Vector3Int start = tilemap.WorldToCell(position);

            Vector3Int? resultInt = BreadthFirstSearch(start, Utils.FourDirections, isGoal, isConnected);

            Vector3? result = null;

            if (resultInt.HasValue)
            {
                result = tilemap.GetCellCenterWorld(resultInt.Value);
            }

            return result;
        }

        /// <summary>
        /// A Breadth First Traversal of nodes in a grid.
        /// </summary>
        /// <param name="position">Starting position of the traversal</param>
        /// <param name="directions">The directions the traversal can go to find connected nodes</param>
        /// <param name="isConnected">A function to decide if the next node is connected to the current node</param>
        public static List<Vector3Int> BreadthFirstTraversal(Vector3Int position, Vector3Int[] directions, Func<Vector3Int, Vector3Int, bool> isConnected)
        {
            Queue<Vector3Int> queue = new Queue<Vector3Int>();
            queue.Enqueue(position);

            HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

            while (queue.Count > 0)
            {
                Vector3Int node = queue.Dequeue();

                foreach (Vector3Int dir in directions)
                {
                    Vector3Int next = node + dir;

                    if (isConnected(node, next) && !visited.Contains(next))
                    {
                        queue.Enqueue(next);
                    }
                }

                visited.Add(node);
            }

            return visited.ToList();
        }

        /// <summary>
        /// A Breadth First Traversal of nodes in a grid.
        /// </summary>
        /// <param name="position">Starting position of the traversal</param>
        /// <param name="directions">The directions the traversal can go to find connected nodes</param>
        /// <param name="isConnected">A function to decide if the next node is connected to the current node</param>
        public static List<Vector3Int> BreadthFirstTraversal(this Tilemap tilemap, Vector3Int position, Vector3Int[] directions, Func<Vector3Int, Vector3Int, bool> isConnected)
        {
            return BreadthFirstTraversal(position, directions, isConnected);
        }

        /// <summary>
        /// A Breadth First Traversal of nodes in a grid.
        /// </summary>
        /// <param name="position">Starting position of the traversal</param>
        /// <param name="directions">The directions the traversal can go to find connected nodes</param>
        /// <param name="isConnected">A function to decide if the next node is connected to the current node</param>
        public static List<Vector3> BreadthFirstTraversal(this Tilemap tilemap, Vector3 position, Vector3Int[] directions, Func<Vector3Int, Vector3Int, bool> isConnected)
        {
            Vector3Int start = tilemap.WorldToCell(position);

            List<Vector3Int> positions = BreadthFirstTraversal(start, directions, isConnected);

            return positions.Select(p => tilemap.GetCellCenterWorld(p)).ToList();
        }

        /// <summary>
        /// A Breadth First Traversal of nodes in a grid.
        /// </summary>
        /// <param name="position">Starting position of the traversal</param>
        /// <param name="isConnected">A function to decide if the next node is connected to the current node</param>
        public static List<Vector3Int> BreadthFirstTraversal(this Tilemap tilemap, Vector3Int position, Func<Vector3Int, Vector3Int, bool> isConnected)
        {
            return BreadthFirstTraversal(position, Utils.FourDirections, isConnected);
        }

        /// <summary>
        /// A Breadth First Traversal of nodes in a grid.
        /// </summary>
        /// <param name="position">Starting position of the traversal</param>
        /// <param name="isConnected">A function to decide if the next node is connected to the current node</param>
        public static List<Vector3> BreadthFirstTraversal(this Tilemap tilemap, Vector3 position, Func<Vector3Int, Vector3Int, bool> isConnected)
        {
            return tilemap.BreadthFirstTraversal(position, Utils.FourDirections, isConnected);
        }
    }
}

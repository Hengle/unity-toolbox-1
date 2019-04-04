using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace Toolbox
{
    /// <summary>
    /// A graph interface for use with A* Search.
    /// </summary>
    public interface IGraph
    {
        IEnumerable<Vector3Int> Neighbors(Vector3Int v);
        float Cost(Vector3Int a, Vector3Int b);
    }

    /// <summary>
    /// A graph based off a tilemap where you can move in up, down, left, and right.
    /// </summary>
    public class FourDirectionGraph : IGraph
    {
        Tilemap map;
        BoundsInt bounds;

        /// <summary>
        /// Creates a new FourDirectionalGraph using the tilemap's cell bounds
        /// as the graph bounds. If the tilemap does not have tiles on its
        /// intended boundary then the graph bounds could be smaller than it
        /// should be and you should use the other constructor to specify the
        /// graph bounds.
        /// </summary>
        public FourDirectionGraph(Tilemap map)
        {
            this.map = map;
            bounds = map.cellBounds;
        }

        public FourDirectionGraph(Tilemap map, BoundsInt bounds)
        {
            this.map = map;
            this.bounds = bounds;
        }

        public IEnumerable<Vector3Int> Neighbors(Vector3Int v)
        {
            foreach (Vector3Int dir in Utils.FourDirections)
            {
                Vector3Int next = v + dir;

                if (bounds.Contains(next) && map.GetTile(next) == null)
                {
                    yield return next;
                }
            }
        }

        public float Cost(Vector3Int a, Vector3Int b)
        {
            return Vector3Int.Distance(a, b);
        }
    }
}

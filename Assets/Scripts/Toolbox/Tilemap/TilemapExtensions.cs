using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Toolbox
{
    public static class TilemapExtensions
    {
        public static bool IsCellEmpty(this Tilemap tilemap, Vector3Int position)
        {
            return tilemap.cellBounds.Contains(position) && tilemap.GetTile(position) == null;
        }

        static readonly Vector3Int[] NEIGHBORS = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(-1, -1, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 1, 0)
        };

        public static Vector3Int? ClosestEmptyCell(this Tilemap tilemap, Vector3Int position)
        {
            Vector3Int pos = tilemap.WorldToCell(position);

            Queue<Vector3Int> cells = new Queue<Vector3Int>();
            cells.Enqueue(pos);

            HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

            while (cells.Count > 0)
            {
                Vector3Int cell = cells.Dequeue();

                if (tilemap.IsCellEmpty(cell))
                {
                    return cell;
                }

                foreach (Vector3Int dir in NEIGHBORS)
                {
                    Vector3Int next = cell + dir;

                    if (!visited.Contains(next))
                    {
                        cells.Enqueue(next);
                    }
                }

                visited.Add(cell);
            }

            return null;
        }

        public static Vector3? ClosestEmptyCell(this Tilemap tilemap, Vector3 position)
        {
            Vector3Int cellPos = tilemap.WorldToCell(position);

            Vector3Int? closestEmpty = tilemap.ClosestEmptyCell(cellPos);

            Vector3? result = null;

            if(closestEmpty.HasValue)
            {
                result = tilemap.GetCellCenterWorld(closestEmpty.Value);
            }

            return result;
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
    }
}

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

        public static Vector3Int MousePositionToCell(this Tilemap tilemap)
        {
            return tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
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

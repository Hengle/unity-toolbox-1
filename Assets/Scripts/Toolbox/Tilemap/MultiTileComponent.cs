using UnityEngine;
using UnityEngine.Tilemaps;

namespace Toolbox
{
    /// <summary>
    /// A component that also takes up multiple Tiles in a Tilemap. If the
    /// Tilemap is not specified it will default to the Tilemap in the parent
    /// Game Object.
    /// </summary>
    public class MultiTileComponent : TileComponent
    {
        public int cellWidth = 2;
        public int cellHeight = 2;

        public override void ClearTile()
        {
            Vector3Int minCell = tilemap.WorldToCell(curPos);
            minCell.x -= Mathf.FloorToInt(cellWidth / 2f);
            minCell.y -= Mathf.FloorToInt(cellHeight / 2f);

            for (int i = 0; i < cellWidth; i++)
            {
                for (int j = 0; j < cellHeight; j++)
                {
                    Vector3Int curCell = new Vector3Int(minCell.x + i, minCell.y + j, minCell.z);

                    if (tilemap.GetTile(curCell) == tile)
                    {
                        tilemap.SetTile(curCell, null);
                    }
                }
            }
        }

        /// <summary>
        /// Sets multiple cells around the given location in the tilemap to a
        /// tile representing the game object. Call this method whenever the
        /// game object moves to a new location on the tilemap.
        /// </summary>
        /// <remarks>
        /// Be very deliberate with the given position. Usually an even length
        /// should have a coordinate on the edge of a cell and an odd length
        /// should have a coordinate at the center of a cell. 
        /// </remarks>
        public override void SetTile(Vector3 newPos)
        {
            ClearTile();

            Vector3Int minCell = tilemap.WorldToCell(newPos);
            minCell.x -= Mathf.FloorToInt(cellWidth / 2f);
            minCell.y -= Mathf.FloorToInt(cellHeight / 2f);

            for (int i = 0; i < cellWidth; i++)
            {
                for (int j = 0; j < cellHeight; j++)
                {
                    tilemap.SetTile(new Vector3Int(minCell.x + i, minCell.y + j, minCell.z), tile);
                }
            }

            curPos = newPos;
        }
    }
}

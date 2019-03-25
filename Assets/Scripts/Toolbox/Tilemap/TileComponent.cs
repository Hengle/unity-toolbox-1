using UnityEngine;
using UnityEngine.Tilemaps;

namespace Toolbox
{
    /// <summary>
    /// A component that also takes up a Tile in a Tilemap. If the Tilemap is
    /// not specified it will default to the Tilemap in the parent Game Object.
    /// </summary>
    public class TileComponent : MonoBehaviour
    {
        public Tilemap tilemap;

        internal Tile tile;
        internal Vector3 curPos;
        internal bool isQuitting;

        public virtual void Awake()
        {
            if (tilemap == null)
            {
                tilemap = GetComponentInParent<Tilemap>();
            }

            tile = ScriptableObject.CreateInstance<Tile>();
            curPos = transform.position;
            SetTile(transform.position);
        }

        public virtual void ClearTile()
        {
            Vector3Int curCell = tilemap.WorldToCell(curPos);

            if (tilemap.GetTile(curCell) == tile)
            {
                tilemap.SetTile(curCell, null);
            }
        }

        /// <summary>
        /// Sets a cell at the given location in the tilemap to a tile
        /// representing the game object. Call this method whenever the game
        /// object moves to a new location on the tilemap.
        /// </summary>
        public virtual void SetTile(Vector3 newPos)
        {
            ClearTile();

            tilemap.SetTile(tilemap.WorldToCell(newPos), tile);

            curPos = newPos;
        }

        void OnApplicationQuit()
        {
            isQuitting = true;
        }

        public virtual void OnDestroy()
        {
            if (!isQuitting)
            {
                ClearTile();
            }
        }
    }
}
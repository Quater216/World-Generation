using Source;
using UnityEngine;

namespace Model
{
    public struct TileData
    {
        public Enums.Tile Tile { get; set; }
        public Vector3Int Position { get; set; }

        public TileData(Enums.Tile tile, Vector3Int position)
        {
            Tile = tile;
            Position = position;
        }
    }
}
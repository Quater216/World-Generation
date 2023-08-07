using Source;
using UnityEngine;

namespace Model
{
    public struct BiomeData
    {
        public Enums.Biome Type;
        public Enums.Tile Tile;
        public Vector3Int Position;

        public BiomeData(Enums.Biome type, Enums.Tile tile, Vector3Int position)
        {
            Type = type;
            Tile = tile;
            Position = position;
        }
    }
}
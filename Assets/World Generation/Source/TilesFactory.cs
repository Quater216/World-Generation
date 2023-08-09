using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Source
{
    public class TilesFactory : MonoBehaviour
    {
        [SerializeField] private TileBase _ground;
        [SerializeField] private TileBase _stone;
        [SerializeField] private TileBase _stone_cave;
        
        public TileBase Get(Enums.Tile type)
        {
            return type switch
            {
                Enums.Tile.Ground => _ground,
                Enums.Tile.Stone => _stone,
                Enums.Tile.Stone_Cave => _stone_cave,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
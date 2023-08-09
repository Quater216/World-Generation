using Source;
using UnityEngine;

namespace Model
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Biome")]
    public class BiomeSetting : ScriptableObject
    {
        public Enums.Biome Biome;
        public Enums.Tile DefaultTile;
    }
}
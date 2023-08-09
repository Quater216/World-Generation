using System;
using Source;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class MapLayer
    {
        [field:SerializeField] public int Height { get; set; }
        [field:SerializeField] public Enums.Tile Tile { get; set; }
        [field:SerializeField] public Enums.Layer Layer { get; set; }
    }
}
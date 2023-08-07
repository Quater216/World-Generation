using Source;
using UnityEngine;

namespace Model
{
    public struct HeatData
    {
        public Enums.HeatType Type;
        public Vector3Int Position;

        public HeatData(Enums.HeatType type, Vector3Int position)
        {
            Type = type;
            Position = position;
        }
    }
}
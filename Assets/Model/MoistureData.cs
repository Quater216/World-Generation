using Source;
using UnityEngine;

namespace Model
{
    public struct MoistureData
    {
        public Enums.MoistureType Type;
        public Vector3Int Position;

        public MoistureData(Enums.MoistureType type, Vector3Int position)
        {
            Type = type;
            Position = position;
        }
    }
}
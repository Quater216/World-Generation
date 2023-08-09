using Source;
using UnityEngine;

namespace Model
{
    public struct HeightData
    {
        public Enums.Layer Layer;
        public Vector3Int Position;

        public HeightData(Enums.Layer layer, Vector3Int position)
        {
            Layer = layer;
            Position = position;
        }
    }
}
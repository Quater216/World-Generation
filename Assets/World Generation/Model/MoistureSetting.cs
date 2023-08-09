using System;
using Source;

namespace Model
{
    [Serializable]
    public class MoistureSetting
    {
        public Enums.MoistureType Type;
        public float Value;
    }
}
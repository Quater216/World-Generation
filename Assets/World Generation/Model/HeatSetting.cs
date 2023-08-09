using System;
using Source;

namespace Model
{
    [Serializable]
    public class HeatSetting
    {
        public Enums.HeatType Type;
        public float Value;
    }
}
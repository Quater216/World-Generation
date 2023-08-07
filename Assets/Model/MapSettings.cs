using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [CreateAssetMenu(menuName = "ScriptableObjects/MapSettings")]
    public class MapSettings : ScriptableObject
    {
        [field:SerializeField] public int Width { get; set; }
        [field:SerializeField] public List<MapLayer> Layers { get; set; }
        [field:SerializeField] public float Smoothness { get; set; }
        [field:SerializeField] public List<MoistureSetting> MoistureSettings { get; set; }
        [field:SerializeField] public List<HeatSetting> HeatSettings { get; set; }
        [field:SerializeField] public BiomeTable BiomeTable { get; set; }
    }
}
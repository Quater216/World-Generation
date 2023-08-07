using System.Linq;
using Model;
using UnityEngine;

namespace Source
{
    public class PerlinNoiseBasedMapGenerator : MapGenerator
    {
        public override Map Generate(MapSettings settings)
        {
            var heightmap = GenerateHeightmap(settings);
            var moistureMap = GenerateMoistureMap(heightmap, settings);
            var heatMap = GenerateHeatMap(heightmap, settings);
            var biomeMap = GenerateBiomeMap(moistureMap, heatMap, settings);
            var map = GenerateMap(heightmap, biomeMap);
            
            return map;
        }

        private Map GenerateMap(Heightmap heightmap, BiomeMap biomeMap)
        {
            var map = new Map();

            if (heightmap.Datas.Count == biomeMap.Datas.Count)
            {
                for (int i = 0; i < heightmap.Datas.Count; i++)
                {
                    map.Tiles.Add(heightmap.Datas[i].Layer == Enums.Layer.Land
                        ? new TileData(biomeMap.Datas[i].Tile, heightmap.Datas[i].Position)
                        : new TileData(Enums.Tile.Stone, heightmap.Datas[i].Position));
                }
            }

            return map;
        }

        private BiomeMap GenerateBiomeMap(MoistureMap moistureMap, HeatMap heatMap, MapSettings settings)
        {
            var biomemap = new BiomeMap();

            if (moistureMap.Datas.Count == heatMap.Datas.Count)
            {
                for (int i = 0; i < moistureMap.Datas.Count; i++)
                {
                    var biomeData = settings.BiomeTable.Rows[(int)moistureMap.Datas[i].Type].Row[(int)heatMap.Datas[i].Type];
                    
                    Debug.Log(biomeData.Biome);
                    
                    biomemap.Datas.Add(new BiomeData(biomeData.Biome, biomeData.DefaultTile, moistureMap.Datas[i].Position));
                }
            }

            return biomemap;
        }

        private MoistureMap GenerateMoistureMap(Heightmap heightmap, MapSettings settings)
        {
            var moistureMap = new MoistureMap();

            foreach (var tileData in heightmap.Datas)
            {
                var moistureType = Enums.MoistureType.Wettest;
                var moistureMultiply = 0.75f;
                var moistureValue = Mathf.PerlinNoise(tileData.Position.x * moistureMultiply, tileData.Position.y * moistureMultiply);

                if (moistureValue < 0)
                    moistureValue *= -1;

                foreach (var moistureSetting in settings.MoistureSettings.Where(moistureSetting => moistureSetting.Value < moistureValue))
                {
                    moistureType = moistureSetting.Type;
                }
                
                var moistureData = new MoistureData(moistureType, tileData.Position);
                moistureMap.Datas.Add(moistureData);
            }

            return moistureMap;
        }
        
        private HeatMap GenerateHeatMap(Heightmap heightmap, MapSettings settings)
        {
            var heatMap = new HeatMap();
            
            foreach (var tileData in heightmap.Datas)
            {
                var heatType = Enums.HeatType.Coldest;
                var heatMultiply = 0.75f;
                var heatValue = Mathf.PerlinNoise(tileData.Position.x * heatMultiply, tileData.Position.y * heatMultiply);

                if (heatValue < 0)
                    heatValue *= -1;
                
                foreach (var heatSetting in settings.HeatSettings.Where(heatSetting => heatSetting.Value < heatValue))
                {
                    heatType = heatSetting.Type;
                }
                
                var heatData = new HeatData(heatType, tileData.Position);
                heatMap.Datas.Add(heatData);
            }

            return heatMap;
        }

        private Heightmap GenerateHeightmap(MapSettings settings)
        {
            var seed = Random.Range(0, 100000);
            var heightmap = new Heightmap();
            
            for (int x = 0; x < settings.Width; x++)
            {
                for (int layerIndex = 0; layerIndex < settings.Layers.Count; layerIndex++)
                {
                    var startY = 0;
                    var height = 0;
                    
                    if (layerIndex == 0)
                    {
                        startY = 0;
                        
                        if (settings.Layers.Count > 1)
                        {
                            height = settings.Layers[layerIndex].Height;
                        }
                        else
                        {
                            height = Mathf.RoundToInt(
                                Mathf.PerlinNoise(x / settings.Smoothness, seed) *
                                settings.Layers[layerIndex].Height / 2);
                        }
                    }
                    else
                    {
                        for (int underLayerIndex = 0; underLayerIndex < layerIndex; underLayerIndex++)
                        {
                            height = layerIndex == (settings.Layers.Count - 1)
                                ? settings.Layers[underLayerIndex].Height + Mathf.RoundToInt(
                                    Mathf.PerlinNoise(x / settings.Smoothness, seed) *
                                    settings.Layers[layerIndex].Height / 2)
                                : settings.Layers[underLayerIndex].Height + settings.Layers[layerIndex].Height;

                            startY = settings.Layers[underLayerIndex].Height;
                        }
                    }
                    
                    for (int y = startY; y < height; y++)
                    {
                        heightmap.Datas.Add(new HeightData(settings.Layers[layerIndex].Layer, new Vector3Int(x, y)));
                    }
                }
            }

            return heightmap;
        }
    }
}

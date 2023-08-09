using System;
using System.Linq;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

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
            var map = GenerateMap(heightmap, biomeMap, settings);
            
            return map;
        }

        private Map GenerateMap(Heightmap heightmap, BiomeMap biomeMap, MapSettings settings)
        {
            var map = new Map();

            foreach (var layer in heightmap.Layers)
            {
                switch (layer.Type)
                {
                    case Enums.Layer.Land:
                    {
                        if (biomeMap.Datas.Count == layer.Datas.Count)
                        {
                            for (int i = 0; i < layer.Datas.Count; i++)
                            {
                                map.Tiles.Add(new TileData(biomeMap.Datas[i].Tile, layer.Datas[i].Position));
                            }
                        }
                        break;
                    }
                    case Enums.Layer.Cave:
                    {
                        GenerateCaves(settings, layer, map);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return map;
        }

        private void GenerateCaves(MapSettings settings, LayerData layer, Map map)
        {
            for (int i = 0; i < layer.Datas.Count; i++)
            {
                var caveChance = 0.5f;
                var caveScale = 5;
                var x = (float)layer.Datas[i].Position.x / settings.Width * caveScale;
                var y = (float)layer.Datas[i].Position.y / 80 * caveScale;
                var caveValue = Mathf.PerlinNoise(x, y);

                map.Tiles.Add(caveValue > caveChance
                    ? new TileData(Enums.Tile.Stone, layer.Datas[i].Position)
                    : new TileData(Enums.Tile.Stone_Cave, layer.Datas[i].Position));
            }
        }

        private BiomeMap GenerateBiomeMap(MoistureMap moistureMap, HeatMap heatMap, MapSettings settings)
        {
            var biomemap = new BiomeMap();

            if (moistureMap.Datas.Count == heatMap.Datas.Count)
            {
                for (int i = 0; i < moistureMap.Datas.Count; i++)
                {
                    var biomeData = settings.BiomeTable.Rows[(int)moistureMap.Datas[i].Type].Row[(int)heatMap.Datas[i].Type];
                    
                    biomemap.Datas.Add(new BiomeData(biomeData.Biome, biomeData.DefaultTile, moistureMap.Datas[i].Position));
                }
            }

            return biomemap;
        }

        private MoistureMap GenerateMoistureMap(Heightmap heightmap, MapSettings settings)
        {
            var moistureMap = new MoistureMap();

            foreach (var layer in heightmap.Layers.Where(data => data.Type == Enums.Layer.Land))
            {
                foreach (var tileData in layer.Datas)
                {
                    var moistureType = Enums.MoistureType.Wettest;
                    var moistureMultiply = 5;
                    var x = (float)tileData.Position.x / settings.Width * moistureMultiply;
                    var y = (float)tileData.Position.y / 80 * moistureMultiply;
                    var moistureValue = Mathf.PerlinNoise(x, y);

                    for (int i = 0; i < settings.MoistureSettings.Count - 1; i++)
                    {
                        if (settings.MoistureSettings[i].Value <= moistureValue &&
                            moistureValue <= settings.MoistureSettings[i + 1].Value)
                        {
                            moistureType = settings.MoistureSettings[i].Type;
                        }
                    }
                
                    var moistureData = new MoistureData(moistureType, tileData.Position);
                    moistureMap.Datas.Add(moistureData);
                }
            }

            return moistureMap;
        }
        
        private HeatMap GenerateHeatMap(Heightmap heightmap, MapSettings settings)
        {
            var heatMap = new HeatMap();
            
            foreach (var layer in heightmap.Layers.Where(data => data.Type == Enums.Layer.Land))
            {
                foreach (var tileData in layer.Datas)
                {
                    var heatType = Enums.HeatType.Coldest;
                    var heatMultiply = 50;
                    var x = (float)tileData.Position.x / settings.Width * heatMultiply;
                    var y = (float)tileData.Position.y / 80 * heatMultiply;
                    var heatValue = Mathf.PerlinNoise(x, y);
                
                    for (int i = 0; i < settings.HeatSettings.Count - 1; i++)
                    {
                        if (settings.HeatSettings[i].Value <= heatValue && heatValue <= settings.HeatSettings[i + 1].Value)
                        {
                            heatType = settings.HeatSettings[i].Type;
                        }
                    }
                
                    var heatData = new HeatData(heatType, tileData.Position);
                    heatMap.Datas.Add(heatData);
                }
            }

            return heatMap;
        }

        private Heightmap GenerateHeightmap(MapSettings settings)
        {
            var seed = Random.Range(0, 100000);
            var heightmap = new Heightmap();

            for (int layerIndex = 0; layerIndex < settings.Layers.Count; layerIndex++)
            {
                var layer = new LayerData
                {
                    Type = settings.Layers[layerIndex].Layer
                };

                for (int x = 0; x < settings.Width; x++)
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
                        layer.Datas.Add(new HeightData(settings.Layers[layerIndex].Layer, new Vector3Int(x, y)));
                    }
                }
                
                heightmap.Layers.Add(layer);
            }

            return heightmap;
        }
    }
}

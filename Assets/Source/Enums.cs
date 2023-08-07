namespace Source
{
    public class Enums
    {
        public enum Tile
        {
            Ground,
            Stone
        }

        public enum Layer
        {
            Cave,
            Land
        }
        
        public enum Biome
        {
            Desert,
            Savanna,
            TropicalRainforest,
            Grassland,
            Woodland,
            SeasonalForest,
            TemperateRainforest,
            BorealForest,
            Tundra,
            Ice
        }
        
        public enum HeatType : int
        {
            Coldest = 0,
            Colder = 1,
            Cold = 2,
            Warm = 3,
            Warmer = 4,
            Warmest = 5
        }
        
        public enum MoistureType : int
        {
            Wettest = 0,
            Wetter = 1,
            Wet = 2,
            Dry = 3,
            Dryer = 4,
            Dryest = 5
        }
    }
}
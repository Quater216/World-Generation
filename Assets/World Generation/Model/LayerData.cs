using System.Collections.Generic;
using Source;

namespace Model
{
    public class LayerData
    {
        public Enums.Layer Type;
        public List<HeightData> Datas = new();
    }
}
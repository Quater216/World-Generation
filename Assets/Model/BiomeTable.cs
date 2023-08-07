using System;
using Source;

namespace Model
{
    [Serializable]
    public class BiomeTable
    {
        [Serializable]
        public struct RowData
        {
            public BiomeSetting[] Row;
        }

        public RowData[] Rows = new RowData[6];
    }
}
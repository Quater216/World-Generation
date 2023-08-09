using Model;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Source
{
    public class MapPresenter : MonoBehaviour
    {
        [SerializeField] private TilesFactory _tilesFactory;
        [SerializeField] private Tilemap _tilemap;

        public void Present(Map map)
        {
            foreach (var tile in map.Tiles)
            {
                _tilemap.SetTile(tile.Position, _tilesFactory.Get(tile.Tile));
            }
        }
    }
}
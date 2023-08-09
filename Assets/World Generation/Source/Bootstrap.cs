using Model;
using UnityEngine;

namespace Source
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private MapSettings _settings;
        [SerializeField] private MapPresenter _mapPresenter;

        private void Start()
        {
            MapGenerator generator = new PerlinNoiseBasedMapGenerator();

            var map = generator.Generate(_settings);
            _mapPresenter.Present(map);
        }
    }
}
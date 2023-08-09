using UnityEngine;

namespace Source
{
    public class NoisePresenter : MonoBehaviour
    {
        [SerializeField] private int _scale;
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        
        private void Update()
        {
            Renderer component = GetComponent<Renderer>();
            component.material.mainTexture = GenerateTexture();
        }
        
        private Texture2D GenerateTexture()
        {
            var texture = new Texture2D(_width, _height);

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var color = CalculateColor(x, y);
                    texture.SetPixel(x, y, color);
                }
            }
            
            texture.Apply();
            
            return texture;
        }

        private Color CalculateColor(int x, int y)
        {
            var xCoord = (float)x / _width * _scale; 
            var yCoord = (float)y / _height * _scale;
            
            var sample = Mathf.PerlinNoise(xCoord, yCoord);
            return new Color(sample, sample, sample);
        }
    }
}
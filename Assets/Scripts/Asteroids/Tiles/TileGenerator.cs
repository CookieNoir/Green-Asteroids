using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Asteroids.Tiles
{
    public class TileGenerator : MonoBehaviour
    {
        private static TileGenerator _instance;

        private void Awake()
        {
            _instance = this;
            _FillDictionary();
        }

        public static TileGenerator GetInstance()
        {
            return _instance;
        }

        [SerializeField] private TileGenerationMethod[] _tileGenerationMethods;
        private Dictionary<string, TileGenerationMethod> _nameMethodPairs;

        private void _FillDictionary()
        {
            _nameMethodPairs = new();
            for (int i = 0; i < _tileGenerationMethods.Length; ++i)
            {
                _nameMethodPairs.Add(_tileGenerationMethods[i].name, _tileGenerationMethods[i]);
            }
        }

        public void Generate(string generationMethodName, Transform parentTransform, Vector3 position, Random random)
        {
            if (generationMethodName != null && generationMethodName.Length > 0)
            {
                TileGenerationMethod method;
                if (_nameMethodPairs.TryGetValue(generationMethodName, out method))
                {
                    method.Generate(parentTransform, position, random);
                }
            }
        }
    }
}
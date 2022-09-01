using UnityEngine;
using Random = System.Random;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _starPrefabs;
    [SerializeField] private Vector2 _radiusRange;
    [SerializeField] private Vector2 _scaleRange;
    [SerializeField] private int _starsCount;
    [SerializeField] private int _randomSeed;

    private void Start()
    {
        SetStars();
    }

    public void SetStars()
    {
        Random random = new Random(_randomSeed);
        for (int i = 0; i < _starsCount; ++i)
        {
            float x = (float)random.NextDouble() * 2f - 1f;
            float y = (float)random.NextDouble() * 2f - 1f;
            float z = (float)random.NextDouble() * 2f - 1f;
            Vector3 direction = new Vector3(x,y,z).normalized;
            float radius = Mathf.Lerp(_radiusRange.x, _radiusRange.y, (float)random.NextDouble());
            float scale = Mathf.Lerp(_scaleRange.x, _scaleRange.y, (float)random.NextDouble());
            int prefab = random.Next(0, _starPrefabs.Length);
            GameObject star = Instantiate(_starPrefabs[prefab], gameObject.transform);
            star.transform.position = direction * radius;
            star.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            star.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
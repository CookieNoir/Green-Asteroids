using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private SphericalTileMap _sphericalTileMap;
    [SerializeField] private SphericalTileMap _gridMap;
    [SerializeField] private Transform _tileMapOrigin;
    [SerializeField] private Transform _gridMapOrigin;
    private bool _generated = false;

    public CubicGrid<ChangeableObstacle> GenerateWorld(LevelProperties levelProperties)
    {
        CubicGrid<ChangeableObstacle> sphericalGrid = new CubicGrid<ChangeableObstacle>(levelProperties.GridSize, CubicSphere.GetInstance(), false);
        bool[] mask = levelProperties.GetMask();
        for (int i = 0; i < mask.Length; ++i)
        {
            GridCell<ChangeableObstacle> cell = sphericalGrid.GetCellByIndex(i);
            ChangeableObstacle data = new ChangeableObstacle();
            data.isObstacle = mask[i];
            data.isChangeable = !mask[i];
            cell.data = data;
        }
        _sphericalTileMap.CreateTileMap(levelProperties.GridSize, mask, _tileMapOrigin, GridTransformer.SetWalls, levelProperties.RandomSeed);
        _gridMap.CreateTileMap(levelProperties.GridSize, mask, _gridMapOrigin, GridTransformer.SetGrid);
        _generated = true;
        return sphericalGrid;
    }

    public void DropWorld()
    {
        if (_generated)
        {
            foreach (Transform child in _tileMapOrigin)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in _gridMapOrigin)
            {
                Destroy(child.gameObject);
            }
            _generated = false;
        }
    }
}

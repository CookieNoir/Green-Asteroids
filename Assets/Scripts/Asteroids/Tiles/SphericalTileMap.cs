using UnityEngine;
using Asteroids.Grid;
using Random = System.Random;

namespace Asteroids.Tiles
{
    [CreateAssetMenu(fileName = "New Tile Map", menuName = "ScriptableObjects/Tile Map")]
    public class SphericalTileMap : ScriptableObject
    {
        [SerializeField] private float _height;
        [SerializeField] private GameObject[] _type0Prefabs;
        [SerializeField] private GameObject[] _type1Prefabs;
        [SerializeField] private GameObject[] _type21Prefabs;
        [SerializeField] private GameObject[] _type22Prefabs;
        [SerializeField] private GameObject[] _type3Prefabs;
        [SerializeField] private GameObject[] _type4Prefabs;
        private Random _random;

        public void CreateTileMap(int gridSize, bool[] mask, Transform origin, GridTransformer.GridTransformation gridTransformation, int seed = 0)
        {
            int doubledGridSize = gridSize + gridSize;
            CubicGrid<bool> cubicGrid = new CubicGrid<bool>(doubledGridSize, SubdividedCube.GetInstance(), true);
            gridTransformation(cubicGrid, mask);
            GridCell<bool>[] cells = new GridCell<bool>[4];
            _random = new(seed);
            for (int i = 0; i < 6; ++i)
            {
                int offseti = i * cubicGrid.CellsOnSide;
                for (int j = 0; j < doubledGridSize; ++j)
                {
                    int offsetj = offseti + j * cubicGrid.GridSize;
                    for (int k = 0; k < doubledGridSize; ++k)
                    {
                        cells[0] = cubicGrid.GetCellByIndex(offsetj + k);
                        cells[1] = cubicGrid.GetCellByIndex(offsetj + k + 1);
                        cells[2] = cubicGrid.GetCellByIndex(offsetj + cubicGrid.GridSize + k + 1);
                        cells[3] = cubicGrid.GetCellByIndex(offsetj + cubicGrid.GridSize + k);

                        int type = 0;
                        if (cells[0].data) type += 1;
                        if (cells[1].data) type += 2;
                        if (cells[3].data) type += 4;
                        if (cells[2].data) type += 8;

                        switch (type)
                        {
                            case 0: { _GenerateTile(_type0Prefabs, origin, cells[0], cells[1], cells[2], cells[3]); break; }
                            case 1: { _GenerateTile(_type1Prefabs, origin, cells[0], cells[1], cells[2], cells[3]); break; }
                            case 2: { _GenerateTile(_type1Prefabs, origin, cells[1], cells[2], cells[3], cells[0]); break; }
                            case 3: { _GenerateTile(_type21Prefabs, origin, cells[0], cells[1], cells[2], cells[3]); break; }
                            case 4: { _GenerateTile(_type1Prefabs, origin, cells[3], cells[0], cells[1], cells[2]); break; }
                            case 5: { _GenerateTile(_type21Prefabs, origin, cells[3], cells[0], cells[1], cells[2]); break; }
                            case 6: { _GenerateTile(_type22Prefabs, origin, cells[0], cells[1], cells[2], cells[3]); break; }
                            case 7: { _GenerateTile(_type3Prefabs, origin, cells[0], cells[1], cells[2], cells[3]); break; }
                            case 8: { _GenerateTile(_type1Prefabs, origin, cells[2], cells[3], cells[0], cells[1]); break; }
                            case 9: { _GenerateTile(_type22Prefabs, origin, cells[1], cells[2], cells[3], cells[0]); break; }
                            case 10: { _GenerateTile(_type21Prefabs, origin, cells[1], cells[2], cells[3], cells[0]); break; }
                            case 11: { _GenerateTile(_type3Prefabs, origin, cells[1], cells[2], cells[3], cells[0]); break; }
                            case 12: { _GenerateTile(_type21Prefabs, origin, cells[2], cells[3], cells[0], cells[1]); break; }
                            case 13: { _GenerateTile(_type3Prefabs, origin, cells[3], cells[0], cells[1], cells[2]); break; }
                            case 14: { _GenerateTile(_type3Prefabs, origin, cells[2], cells[3], cells[0], cells[1]); break; }
                            case 15: { _GenerateTile(_type4Prefabs, origin, cells[0], cells[1], cells[2], cells[3]); break; }
                        }
                    }
                }
            }
        }

        private void _GenerateTile(GameObject[] prefabs, Transform origin, GridCell<bool> point1, GridCell<bool> point2, GridCell<bool> point3, GridCell<bool> point4)
        {
            if (prefabs != null && prefabs.Length > 0)
            {
                int number = _random.Next(0, prefabs.Length);
                GameObject newTile = Instantiate(prefabs[number], Vector3.zero, Quaternion.identity);
                newTile.transform.parent = origin;
                newTile.GetComponent<SphericalTile>().Deform(point1.Position, point2.Position, point3.Position, point4.Position, _height, _random);
            }
        }
    }
}
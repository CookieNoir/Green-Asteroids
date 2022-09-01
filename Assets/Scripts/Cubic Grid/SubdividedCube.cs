using UnityEngine;

public class SubdividedCube
{
    protected static SubdividedCube instance;

    protected SubdividedCube() { }

    public static SubdividedCube GetInstance()
    {
        if (instance == null) instance = new SubdividedCube();
        return instance;
    }

    public Vector3[] GetPoints(int gridSize, bool includeBorders)
    {
        Vector3[] points;
        if (includeBorders) points = GetAllPoints(gridSize);
        else points = GetCenters(gridSize);
        return points;
    }

    private Vector3[] GetAllPoints(int gridSize)
    {
        int gridSize1 = gridSize + 1;
        int verticesLength = gridSize1 * gridSize1 * 6;
        Vector3[] points = new Vector3[verticesLength];

        int i = 0;
        for (int x = 0; x < gridSize1; x++) // xy
        {
            for (int y = 0; y < gridSize1; y++)
            {
                points[i] = GetPosition(gridSize, x, y, 0);
                i++;
            }
        }
        for (int z = 0; z < gridSize1; z++) // yz
        {
            for (int y = 0; y < gridSize1; y++)
            {
                points[i] = GetPosition(gridSize, gridSize, y, z);
                i++;
            }
        }
        for (int x = gridSize; x > -1; x--) // -xy
        {
            for (int y = 0; y < gridSize1; y++)
            {
                points[i] = GetPosition(gridSize, x, y, gridSize);
                i++;
            }
        }
        for (int z = gridSize; z > -1; z--) // -zy
        {
            for (int y = 0; y < gridSize1; y++)
            {
                points[i] = GetPosition(gridSize, 0, y, z);
                i++;
            }
        }

        for (int x = 0; x < gridSize1; x++) // top
        {
            for (int z = 0; z < gridSize1; z++)
            {
                points[i] = GetPosition(gridSize, x, gridSize, z);
                i++;
            }
        }
        for (int x = gridSize; x > -1; x--) // down
        {
            for (int z = 0; z < gridSize1; z++)
            {
                points[i] = GetPosition(gridSize, x, 0, z);
                i++;
            }
        }
        return points;
    }

    private Vector3[] GetCenters(int gridSize)
    {
        int verticesCount = gridSize * gridSize * 6;
        Vector3[] points = new Vector3[verticesCount];
        int doubledGridSize = gridSize + gridSize;
        int i = 0;
        for (int x = 0; x < gridSize; x++) // xy forward
        {
            for (int y = 0; y < gridSize; y++)
            {
                points[i] = GetPosition(doubledGridSize, 1 + 2 * x, 1 + 2 * y, 0);
                i++;
            }
        }
        for (int z = 0; z < gridSize; z++) // yz right
        {
            for (int y = 0; y < gridSize; y++)
            {
                points[i] = GetPosition(doubledGridSize, doubledGridSize, 1 + 2 * y, 1 + 2 * z);
                i++;
            }
        }
        for (int x = gridSize - 1; x > -1; x--) // -xy backward
        {
            for (int y = 0; y < gridSize; y++)
            {
                points[i] = GetPosition(doubledGridSize, 1 + 2 * x, 1 + 2 * y, doubledGridSize);
                i++;
            }
        }
        for (int z = gridSize - 1; z > -1; z--) // -zy left
        {
            for (int y = 0; y < gridSize; y++)
            {
                points[i] = GetPosition(doubledGridSize, 0, 1 + 2 * y, 1 + 2 * z);
                i++;
            }
        }

        for (int x = 0; x < gridSize; x++) // top
        {
            for (int z = 0; z < gridSize; z++)
            {
                points[i] = GetPosition(doubledGridSize, 1 + 2 * x, doubledGridSize, 1 + 2 * z);
                i++;
            }
        }
        for (int x = gridSize - 1; x > -1; x--) // bottom
        {
            for (int z = 0; z < gridSize; z++)
            {
                points[i] = GetPosition(doubledGridSize, 1 + 2 * x, 0, 1 + 2 * z);
                i++;
            }
        }
        return points;
    }

    public virtual Vector3 GetPosition(in int gridSize, in int x, in int y, in int z)
    {
        return new Vector3(x, y, z) * 2f / gridSize - Vector3.one;
    }
}
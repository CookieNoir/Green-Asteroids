using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridTransformer
{
    public delegate void GridTransformation(CubicGrid<bool> cubicGrid, bool[] mask);

    public static void SetWalls(CubicGrid<bool> cubicGrid, bool[] mask) // CubicGridSize = maskGridSize * 2 + 1
    {
        int halfSize = cubicGrid.GridSize / 2;
        int halfSide = halfSize * halfSize;
        for (int i = 0; i < 6; ++i)
        {
            int offseti = i * cubicGrid.CellsOnSide;
            for (int j = 0; j < halfSize; ++j)
            {
                int offsetj = offseti + (1 + 2 * j) * cubicGrid.GridSize;
                for (int k = 0; k < halfSize; ++k)
                {
                    cubicGrid.GetCellByIndex(offsetj + 1 + 2 * k).data = mask[halfSide * i + halfSize * j + k];
                }
            }
        }
        for (int t = 0; t < 2; ++t)
        {
            for (int i = 0; i < cubicGrid.CellsCount; ++i)
            {
                GridCell<bool> cell = cubicGrid.GetCellByIndex(i);
                if ((cell.Next(0).data && cell.Next(2).data) ||
                    (cell.Next(1).data && cell.Next(3).data)) cell.data = true;
            }
        }
    }

    public static void SetGrid(CubicGrid<bool> cubicGrid, bool[] mask) // CubicGridSize = maskGridSize * 2 + 1
    {
        int halfSize = cubicGrid.GridSize / 2;
        int halfSide = halfSize * halfSize;
        for (int i = 0; i < 6; ++i)
        {
            int offseti = i * cubicGrid.CellsOnSide;
            for (int j = 0; j < halfSize; ++j)
            {
                int offsetj = offseti + (1 + 2 * j) * cubicGrid.GridSize;
                for (int k = 0; k < halfSize; ++k)
                {
                    if (!mask[halfSide * i + halfSize * j + k])
                    {
                        for (int l = 0; l < 4; ++l)
                        {
                            GridCell<bool> cell = cubicGrid.GetCellByIndex(offsetj + 1 + 2 * k).Next(l);
                            _SetCell(cell);
                            cell = cell.Next((l + 1) % 4);
                            _SetCell(cell);
                        }
                    }
                }
            }
        }
    }

    private static void _SetCell(GridCell<bool> cell)
    {
        cell.data = true;
        GridCell<bool> nextCell;
        int[] directions = cell.GetConnectionsWithOtherSides();
        for (int c = 0; c < directions.Length; ++c)
        {
            nextCell = cell.Next(directions[c]);
            int direction = CubicGrid<bool>.ModifyDirection(directions[c], cell, nextCell);
            nextCell.Next((direction + 2) % 4).data = true;
        }
    }
}
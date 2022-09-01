using UnityEngine;

public class CubicGrid<T>
{
    public int GridSize { get; private set; }
    public int CellsOnSide { get; private set; }
    public int CellsCount { get; private set; }

    private GridCell<T>[] _cells;

    public CubicGrid(int gridSize, SubdividedCube subdivided, bool includeBorders)
    {
        _SetCells(gridSize, subdivided, includeBorders);
    }

    public GridCell<T> GetCellByIndex(int index)
    {
        if (index > -1 && index < _cells.Length) return _cells[index];
        else return null;
    }

    public static int ModifyDirection(int currentDirection, GridCell<T> prevCell, GridCell<T> currentCell)
    {
        int nextDirection = currentDirection;
        if (prevCell.SideIndex != currentCell.SideIndex)
        {
            switch (prevCell.SideIndex)
            {
                case 0:
                    {
                        switch (currentCell.SideIndex)
                        {
                            case 3: { nextDirection = 3; break; }
                            case 1: { nextDirection = 1; break; }
                            case 4: { nextDirection = 0; break; }
                            case 5: { nextDirection = 0; break; }
                        }
                        break;
                    }
                case 1:
                    {
                        switch (currentCell.SideIndex)
                        {
                            case 0: { nextDirection = 3; break; }
                            case 2: { nextDirection = 1; break; }
                            case 4: { nextDirection = 3; break; }
                            case 5: { nextDirection = 1; break; }
                        }
                        break;
                    }
                case 2:
                    {
                        switch (currentCell.SideIndex)
                        {
                            case 1: { nextDirection = 3; break; }
                            case 3: { nextDirection = 1; break; }
                            case 4: { nextDirection = 2; break; }
                            case 5: { nextDirection = 2; break; }
                        }
                        break;
                    }
                case 3:
                    {
                        switch (currentCell.SideIndex)
                        {
                            case 2: { nextDirection = 3; break; }
                            case 0: { nextDirection = 1; break; }
                            case 4: { nextDirection = 1; break; }
                            case 5: { nextDirection = 3; break; }
                        }
                        break;
                    }
                case 4:
                    {
                        nextDirection = 2;
                        break;
                    }
                case 5:
                    {
                        nextDirection = 0;
                        break;
                    }
            }
        }
        return nextDirection;
    }

    public static MovingDirections ModifyDirection(MovingDirections currentDirection, GridCell<T> prevCell, GridCell<T> currentCell)
    {
        MovingDirections nextDirection = currentDirection;
        if (prevCell.SideIndex != currentCell.SideIndex)
        {
            switch (prevCell.SideIndex)
            {
                case 0:
                    {
                        switch (currentCell.SideIndex)
                        {
                            case 3: { nextDirection = MovingDirections.Left; break; }
                            case 1: { nextDirection = MovingDirections.Right; break; }
                            case 4: { nextDirection = MovingDirections.Forward; break; }
                            case 5: { nextDirection = MovingDirections.Forward; break; }
                        }
                        break;
                    }
                case 1:
                    {
                        switch (currentCell.SideIndex)
                        {
                            case 0: { nextDirection = MovingDirections.Left; break; }
                            case 2: { nextDirection = MovingDirections.Right; break; }
                            case 4: { nextDirection = MovingDirections.Left; break; }
                            case 5: { nextDirection = MovingDirections.Right; break; }
                        }
                        break;
                    }
                case 2:
                    {
                        switch (currentCell.SideIndex)
                        {
                            case 1: { nextDirection = MovingDirections.Left; break; }
                            case 3: { nextDirection = MovingDirections.Right; break; }
                            case 4: { nextDirection = MovingDirections.Back; break; }
                            case 5: { nextDirection = MovingDirections.Back; break; }
                        }
                        break;
                    }
                case 3:
                    {
                        switch (currentCell.SideIndex)
                        {
                            case 2: { nextDirection = MovingDirections.Left; break; }
                            case 0: { nextDirection = MovingDirections.Right; break; }
                            case 4: { nextDirection = MovingDirections.Right; break; }
                            case 5: { nextDirection = MovingDirections.Left; break; }
                        }
                        break;
                    }
                case 4:
                    {
                        nextDirection = MovingDirections.Back;
                        break;
                    }
                case 5:
                    {
                        nextDirection = MovingDirections.Forward;
                        break;
                    }
            }
        }
        return nextDirection;
    }

    public GridCell<T> GetNearestCell(Vector3 point)
    {
        int nearest = 0;
        float minDistance = (point - _cells[0].Position).sqrMagnitude;
        for (int i = 1; i < _cells.Length; ++i)
        {
            float distance = (point - _cells[i].Position).sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = i;
            }
        }
        return _cells[nearest];
    }

    public void _SetCells(int gridSize, SubdividedCube subdivided, bool includeBorders)
    {
        GridSize = gridSize;
        Vector3[] points = subdivided.GetPoints(gridSize, includeBorders);
        if (includeBorders) GridSize += 1;
        CellsOnSide = GridSize * GridSize;
        _cells = new GridCell<T>[points.Length];
        CellsCount = _cells.Length;
        for (int i = 0; i < CellsCount; ++i)
        {
            _cells[i] = new GridCell<T>(i / CellsOnSide, points[i], 4);
        }

        _SetConnections(includeBorders);
    }

    private void _SetConnections(bool includeBorders)
    {
        for (int i = 0; i < 6; ++i) // inner connections
        {
            int offset_i = i * CellsOnSide;
            for (int j = 0; j < GridSize - 1; ++j)
            {
                int offset_j = j * GridSize;
                for (int k = 0; k < GridSize - 1; ++k)
                {
                    int offset_k = offset_i + offset_j + k;
                    _cells[offset_k].SetConnection(1, _cells[offset_k + GridSize]);
                    _cells[offset_k + GridSize].SetConnection(3, _cells[offset_k]);

                    _cells[offset_k].SetConnection(0, _cells[offset_k + 1]);
                    _cells[offset_k + 1].SetConnection(2, _cells[offset_k]);

                    _cells[offset_k + GridSize].SetConnection(0, _cells[offset_k + GridSize + 1]);
                    _cells[offset_k + GridSize + 1].SetConnection(2, _cells[offset_k + GridSize]);

                    _cells[offset_k + 1].SetConnection(1, _cells[offset_k + GridSize + 1]);
                    _cells[offset_k + GridSize + 1].SetConnection(3, _cells[offset_k + 1]);
                }
            }
        }
        int offset_right = CellsOnSide;
        int offset_backward = offset_right + CellsOnSide;
        int offset_left = offset_backward + CellsOnSide;
        int offset_top = offset_left + CellsOnSide;
        int offset_bottom = offset_top + CellsOnSide;
        int offset_middle = GridSize * (GridSize - 1);

        for (int i = 0; i < GridSize; ++i)
        {
            int row = GridSize * i;
            int reverseRow = GridSize * (GridSize - i - 1);
            int rowMinus1 = GridSize * (i + 1) - 1;
            int reverseRowMinus1 = GridSize * (GridSize - i) - 1;

            if (includeBorders)
            {
                // forward-right
                _cells[offset_middle + i].SetConnection(1, _cells[offset_right + i].Next(1));
                _cells[offset_right + i].SetConnection(3, _cells[offset_middle + i].Next(3));
                // right-backward
                _cells[offset_right + offset_middle + i].SetConnection(1, _cells[offset_backward + i].Next(1));
                _cells[offset_backward + i].SetConnection(3, _cells[offset_right + offset_middle + i].Next(3));
                // backward-left
                _cells[offset_backward + offset_middle + i].SetConnection(1, _cells[offset_left + i].Next(1));
                _cells[offset_left + i].SetConnection(3, _cells[offset_backward + offset_middle + i].Next(3));
                // left-forward
                _cells[offset_left + offset_middle + i].SetConnection(1, _cells[i].Next(1));
                _cells[i].SetConnection(3, _cells[offset_left + offset_middle + i].Next(3));

                // top-forward
                _cells[offset_top + row].SetConnection(2, _cells[rowMinus1].Next(2));
                _cells[rowMinus1].SetConnection(0, _cells[offset_top + row].Next(0));
                // top-right
                _cells[offset_top + offset_middle + i].SetConnection(1, _cells[offset_right + rowMinus1].Next(2));
                _cells[offset_right + rowMinus1].SetConnection(0, _cells[offset_top + offset_middle + i].Next(3));
                // top-backward
                _cells[offset_top + rowMinus1].SetConnection(0, _cells[offset_backward + reverseRowMinus1].Next(2));
                _cells[offset_backward + reverseRowMinus1].SetConnection(0, _cells[offset_top + rowMinus1].Next(2));
                // top-left
                _cells[offset_top + i].SetConnection(3, _cells[offset_left + reverseRowMinus1].Next(2));
                _cells[offset_left + reverseRowMinus1].SetConnection(0, _cells[offset_top + i].Next(1));

                // bottom-forward
                _cells[offset_bottom + row].SetConnection(2, _cells[reverseRow].Next(0));
                _cells[reverseRow].SetConnection(2, _cells[offset_bottom + row].Next(0));
                // bottom-right
                _cells[offset_bottom + i].SetConnection(3, _cells[offset_right + row].Next(0));
                _cells[offset_right + row].SetConnection(2, _cells[offset_bottom + i].Next(1));
                // bottom-backward
                _cells[offset_bottom + rowMinus1].SetConnection(0, _cells[offset_backward + row].Next(0));
                _cells[offset_backward + row].SetConnection(2, _cells[offset_bottom + rowMinus1].Next(2));
                // bottom-left
                _cells[offset_bottom + offset_middle + i].SetConnection(1, _cells[offset_left + reverseRow].Next(0));
                _cells[offset_left + reverseRow].SetConnection(2, _cells[offset_bottom + offset_middle + i].Next(3));
            }
            else
            {
                // forward-right
                _cells[offset_middle + i].SetConnection(1, _cells[offset_right + i]);
                _cells[offset_right + i].SetConnection(3, _cells[offset_middle + i]);
                // right-backward
                _cells[offset_right + offset_middle + i].SetConnection(1, _cells[offset_backward + i]);
                _cells[offset_backward + i].SetConnection(3, _cells[offset_right + offset_middle + i]);
                // backward-left
                _cells[offset_backward + offset_middle + i].SetConnection(1, _cells[offset_left + i]);
                _cells[offset_left + i].SetConnection(3, _cells[offset_backward + offset_middle + i]);
                // left-forward
                _cells[offset_left + offset_middle + i].SetConnection(1, _cells[i]);
                _cells[i].SetConnection(3, _cells[offset_left + offset_middle + i]);

                // top-forward
                _cells[offset_top + row].SetConnection(2, _cells[rowMinus1]);
                _cells[rowMinus1].SetConnection(0, _cells[offset_top + row]);
                // top-right
                _cells[offset_top + offset_middle + i].SetConnection(1, _cells[offset_right + rowMinus1]);
                _cells[offset_right + rowMinus1].SetConnection(0, _cells[offset_top + offset_middle + i]);
                // top-backward
                _cells[offset_top + rowMinus1].SetConnection(0, _cells[offset_backward + reverseRowMinus1]);
                _cells[offset_backward + reverseRowMinus1].SetConnection(0, _cells[offset_top + rowMinus1]);
                // top-left
                _cells[offset_top + i].SetConnection(3, _cells[offset_left + reverseRowMinus1]);
                _cells[offset_left + reverseRowMinus1].SetConnection(0, _cells[offset_top + i]);

                // bottom-forward
                _cells[offset_bottom + row].SetConnection(2, _cells[reverseRow]);
                _cells[reverseRow].SetConnection(2, _cells[offset_bottom + row]);
                // bottom-right
                _cells[offset_bottom + i].SetConnection(3, _cells[offset_right + row]);
                _cells[offset_right + row].SetConnection(2, _cells[offset_bottom + i]);
                // bottom-backward
                _cells[offset_bottom + rowMinus1].SetConnection(0, _cells[offset_backward + row]);
                _cells[offset_backward + row].SetConnection(2, _cells[offset_bottom + rowMinus1]);
                // bottom-left
                _cells[offset_bottom + offset_middle + i].SetConnection(1, _cells[offset_left + reverseRow]);
                _cells[offset_left + reverseRow].SetConnection(2, _cells[offset_bottom + offset_middle + i]);
            }
        }
    }
}

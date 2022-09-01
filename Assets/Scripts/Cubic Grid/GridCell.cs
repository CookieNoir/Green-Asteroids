using System;
using UnityEngine;

[Serializable]
public class GridCell<T>
{
    public int SideIndex { get; private set; }
    public Vector3 Position { get; private set; }
    [NonSerialized] private GridCell<T>[] _connections;
    public T data;

    public GridCell<T> Next(int direction)
    {
        if (direction > -1 && direction < _connections.Length) return _connections[direction];
        else return null;
    }

    public Quaternion GetLookAtRotation(int direction)
    {
        GridCell<T> next = Next(direction);
        if (next != null) return SphericalHelper.SphericalLookAt(Position, next.Position);
        else return Quaternion.identity;
    }

    public GridCell(int sideIndex, Vector3 position, int directionsCount)
    {
        SideIndex = sideIndex;
        Position = position;
        _connections = new GridCell<T>[directionsCount];
    }

    public void SetConnection(int direction, GridCell<T> cell)
    {
        if (direction > -1 && direction < _connections.Length) _connections[direction] = cell;
    }

    public int GetConnectionCount()
    {
        int result = 0;
        if (_connections != null)
        {
            for (int i = 0; i < _connections.Length; ++i)
            {
                if (Next(i) != null) result++;
            }
        }
        return result;
    }

    public int GetSharedEdgesCount()
    {
        int result = 0;
        if (_connections != null)
        {
            for (int i = 0; i < _connections.Length; ++i)
            {
                if (Next(i).SideIndex != SideIndex) result++;
            }
        }
        return result;
    }

    public int[] GetConnectionsWithOtherSides()
    {
        int sharedEdges = GetSharedEdgesCount();
        int[] connections = new int[sharedEdges];
        int c = 0;
        for (int i = 0; i < _connections.Length; ++i)
        {
            if (Next(i).SideIndex != SideIndex)
            {
                connections[c] = i;
                c++;
            }
        }
        return connections;
    }
}
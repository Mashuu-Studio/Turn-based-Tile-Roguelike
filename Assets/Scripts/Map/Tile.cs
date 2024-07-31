using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : ScriptableObject
{
    public enum TileType { NONE = 0, FLOOR, OBSTARCLE, }
    public TileType type;
    public event Action OnTileChanged;
    public string guid;
    public Tile()
    {
        type = TileType.FLOOR;
    }

    public void SetType(TileType type)
    {
        this.type = type;
    }

    public void TileChanged()
    {
        OnTileChanged?.Invoke();
    }
}
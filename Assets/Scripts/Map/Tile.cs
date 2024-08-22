using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile : Data
{
    public enum TileType { NONE = 0, FLOOR, OBSTARCLE, }
    public TileType type;
    public Tile()
    {
        type = TileType.FLOOR;
    }

    public void SetType(TileType type)
    {
        this.type = type;
        AssetDatabase.SaveAssets();
    }
}
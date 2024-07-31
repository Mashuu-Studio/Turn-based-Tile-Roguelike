using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile : ScriptableObject
{
    public enum TileType { NONE = 0, FLOOR, OBSTARCLE, }
    public TileType type;
    public string guid;
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
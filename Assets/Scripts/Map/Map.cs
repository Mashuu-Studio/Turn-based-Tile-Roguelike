using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

[CreateAssetMenu()]
public class Map : ScriptableObject
{
    public int width, height;
    public event Action OnMapChanged;
    public Tile[,] Tiles { get { return tiles; } }
    private Tile[,] tiles;

    public void LoadMap()
    {
        Map map = AssetDatabase.LoadAssetAtPath<Map>(AssetDatabase.GetAssetPath(this));

        if (map)
        {
            // 타일정보를 불러온 뒤 만일의 상황을 대비해 SetMap을 통해 지워주거나 채워줌.
            tiles = map.tiles;
            SetMap();
        }
    }

    // 맵 정보를 세팅하는 메서드
    public void SetMap()
    {
        if (tiles != null && tiles.GetLength(0) == width && tiles.GetLength(1) == height) return;

        Tile[,] newTiles = new Tile[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // 타일이 이미 있다면 기존 타일을 그대로 받아감.
                // 없다면 새롭게 생성
                if (tiles != null && i < tiles.GetLength(0) && j < tiles.GetLength(1))
                {
                    newTiles[i, j] = tiles[i, j];
                }
                else
                {
                    Tile tile = CreateInstance(typeof(Tile)) as Tile;
                    tile.name = $"({i},{j})";
                    tile.guid = GUID.Generate().ToString();
                    AssetDatabase.AddObjectToAsset(tile, this);
                    newTiles[i, j] = tile;
                }
            }
        }

        if (tiles != null)
        {
            // 기존의 타일이 더 클 경우에는 남은 부분을 지워줘야함.
            for (int i = width; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    AssetDatabase.RemoveObjectFromAsset(tiles[i, j]);
                }
            }

            for (int j = height; j < tiles.GetLength(1); j++)
            {
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    AssetDatabase.RemoveObjectFromAsset(tiles[i, j]);
                }
            }
        }
        AssetDatabase.SaveAssets();
        tiles = newTiles;
        OnMapChanged?.Invoke();
    }

    // 특정 타일을 변환하는 메서드
    public void SetTile(int x, int y, Tile.TileType type)
    {
        tiles[x, y].SetType(type);
        AssetDatabase.SaveAssets();
    }
}
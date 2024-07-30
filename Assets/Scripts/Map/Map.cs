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
            // Ÿ�������� �ҷ��� �� ������ ��Ȳ�� ����� SetMap�� ���� �����ְų� ä����.
            tiles = map.tiles;
            SetMap();
        }
    }

    // �� ������ �����ϴ� �޼���
    public void SetMap()
    {
        if (tiles != null && tiles.GetLength(0) == width && tiles.GetLength(1) == height) return;

        Tile[,] newTiles = new Tile[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Ÿ���� �̹� �ִٸ� ���� Ÿ���� �״�� �޾ư�.
                // ���ٸ� ���Ӱ� ����
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
            // ������ Ÿ���� �� Ŭ ��쿡�� ���� �κ��� ���������.
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

    // Ư�� Ÿ���� ��ȯ�ϴ� �޼���
    public void SetTile(int x, int y, Tile.TileType type)
    {
        tiles[x, y].SetType(type);
        AssetDatabase.SaveAssets();
    }
}
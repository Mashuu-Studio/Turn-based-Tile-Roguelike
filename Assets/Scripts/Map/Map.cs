using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

[CreateAssetMenu(fileName = "Map Data", menuName = "Create Data/Map")]
public class Map : ScriptableObject
{
    public string guid;
    public int width = 11, height = 7;
    public event Action OnMapChanged;
    public Tile[,] Tiles { get { return tiles; } }
    private Tile[,] tiles;
    [SerializeField] private Tile[] serializeTiles;

    private void SerializeTiles()
    {
        serializeTiles = new Tile[width * height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                serializeTiles[i * height + j] = tiles[i, j];
            }
        }
    }

    public void DeserializeTiles()
    {
        // Deserialize �� �迭�� ���ٸ� ����.
        if (serializeTiles == null || serializeTiles.Length < width * height) SetMap();

        tiles = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tiles[i, j] = serializeTiles[i * height + j];
            }
        }
    }

    public void LoadMap()
    {
        Map map = AssetDatabase.LoadAssetAtPath<Map>(AssetDatabase.GetAssetPath(this));

        if (map)
        {
            // Ÿ�������� �ҷ��� �� ������ ��Ȳ�� ����� SetMap�� ���� �����ְų� ä����.
            DeserializeTiles();
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
        tiles = newTiles;
        SerializeTiles();
        OnMapChanged?.Invoke();
        AssetDatabase.SaveAssets();
    }
}
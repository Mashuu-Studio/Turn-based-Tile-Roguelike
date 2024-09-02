using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Scripting;
using static UnityEditor.PlayerSettings;

[CreateAssetMenu(fileName = "Map Data", menuName = "Create Data/Map")]
public class Map : Data
{
    public event Action OnChanged;
    public int width = 11, height = 7;
    public Tile[,] Tiles { get { return tiles; } }
    private Tile[,] tiles;

    public List<Vector3Int> EnemiePoses { get { return enemies.Keys.ToList(); } }
    private Dictionary<Vector3Int, Unit> enemies;
    [SerializeField] private List<Vector3Int> serializeEnemyKeys;
    [SerializeField] private List<Unit> serializeEnemyValues;
    [SerializeField] private Tile[] serializeTiles;

    // 좌 우 상 하로 이루어진 4개의 비트로 문의 형태 파악
    // ex) 좌우가 열려있는 맵의 경우 1100
    // ex) 좌우상하가 전부 열려있는 경우 1111
    public int DoorInfoBit
    {
        get
        {
            if (tiles == null) Deserialize();
            int value = 0;

            value += (Tiles[0, height / 2].type == Tile.TileType.FLOOR ? 1 : 0);
            value <<= 1;
            value += (Tiles[width - 1, height / 2].type == Tile.TileType.FLOOR ? 1 : 0);
            value <<= 1;
            value += (Tiles[width / 2, height - 1].type == Tile.TileType.FLOOR ? 1 : 0);
            value <<= 1;
            value += (Tiles[width / 2, 0].type == Tile.TileType.FLOOR ? 1 : 0);

            return value;
        }
    }

    public Unit GetEnemy(Vector3Int pos)
    {
        if (enemies.ContainsKey(pos)) return enemies[pos];
        return null;
    }

    public void AddEnemy(Vector3Int pos, Unit unit)
    {
        if (enemies == null) enemies = new Dictionary<Vector3Int, Unit>();

        if (enemies.ContainsKey(pos)) enemies[pos] = unit;
        else enemies.Add(pos, unit);

        Serialize();
        AssetDatabase.SaveAssets();
    }

    private void Serialize()
    {
        serializeTiles = new Tile[width * height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                serializeTiles[i * height + j] = tiles[i, j];
            }
        }

        if (enemies != null)
        {
            serializeEnemyKeys = new List<Vector3Int>();
            serializeEnemyValues = new List<Unit>();
            foreach (var enemy in enemies)
            {
                serializeEnemyKeys.Add(enemy.Key);
                serializeEnemyValues.Add(enemy.Value);
            }
        }
    }

    public void Deserialize()
    {
        // Deserialize 할 배열이 없다면 생성.
        if (serializeTiles == null || serializeTiles.Length < width * height) SetMap();

        tiles = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tiles[i, j] = serializeTiles[i * height + j];
            }
        }

        if (serializeEnemyKeys != null)
        {
            enemies = new Dictionary<Vector3Int, Unit>();
            for (int i = 0; i < serializeEnemyKeys.Count; i++)
            {
                enemies.Add(serializeEnemyKeys[i], serializeEnemyValues[i]);
            }
        }
    }

    public void Save()
    {
        Serialize();
        AssetDatabase.SaveAssets();
    }

    public void LoadMap()
    {
        Map map = AssetDatabase.LoadAssetAtPath<Map>(AssetDatabase.GetAssetPath(this));

        if (map)
        {
            // 타일정보를 불러온 뒤 만일의 상황을 대비해 SetMap을 통해 지워주거나 채워줌.
            Deserialize();
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
        tiles = newTiles;
        OnChanged?.Invoke();
        Save();
    }
}
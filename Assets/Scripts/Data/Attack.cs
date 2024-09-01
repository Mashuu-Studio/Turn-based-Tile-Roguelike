using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Attack.SpreadInfo;


[CreateAssetMenu(fileName = "Attack Data", menuName = "Create Data/Attack")]
// ���� Ÿ�� ����.
public class Attack : Data
{
    public class SpreadInfo
    {
        // ������, �󸶰������� ������ ���� �ʿ�.
        public int range = 0;
        public int interval = 0;
        public Vector3Int pos;
        public enum SpreadType { LEFT_UP = 0, UP, RIGHT_UP, RIGHT, RIGHT_DOWN, DOWN, LEFT_DOWN, LEFT }
        public bool[] spreadTypes = new bool[Enum.GetValues(typeof(SpreadType)).Length];

        // ���� ������ Ȯ������� ����.
        public List<SpreadType> SpreadTypes
        {
            get
            {
                List<SpreadType> types = new List<SpreadType>();
                for (int i = 0; i < spreadTypes.Length; i++)
                {
                    if (spreadTypes[i]) types.Add((SpreadType)i);
                }
                return types;
            }
        }
    }

    public Dictionary<Vector3Int, SpreadInfo> spreadInfos = new Dictionary<Vector3Int, SpreadInfo>();
    public List<Vector3Int> attackRange = new List<Vector3Int>();
    public event Action OnChanged;

    public void ClearSpreadInfo(Vector3Int pos)
    {
        if (spreadInfos.ContainsKey(pos)) spreadInfos.Remove(pos);
    }

    public void SetSpreadType(Vector3Int pos, SpreadInfo.SpreadType spreadType, bool b)
    {
        // ���� ���� ������ �ƴ϶�� �߰�
        if (!spreadInfos.ContainsKey(pos)) spreadInfos.Add(pos, new SpreadInfo());
        spreadInfos[pos].spreadTypes[(int)spreadType] = b;

        // ������ ��ǻ� ����ִٸ� ����.
        if (spreadInfos[pos].SpreadTypes.Count == 0) spreadInfos.Remove(pos);

        AssetDatabase.SaveAssets();
    }

    public void SetRange(Vector3Int pos, int range)
    {
        if (!spreadInfos.ContainsKey(pos)) spreadInfos.Add(pos, new SpreadInfo());
        
        spreadInfos[pos].range = range;

        AssetDatabase.SaveAssets();
    }

    public void SetInterval(Vector3Int pos, int interval)
    {
        if (!spreadInfos.ContainsKey(pos)) spreadInfos.Add(pos, new SpreadInfo());
        
        spreadInfos[pos].interval = interval;

        AssetDatabase.SaveAssets();
    }

    public List<Vector3Int> GetRange(Vector3Int pos, Vector3Int mapSize)
    {
        List<Vector3Int> range = new List<Vector3Int>();
        // ��� ���⿡ ���ؼ� ����.
        if (spreadInfos.ContainsKey(pos)) AddRange(ref range, pos, mapSize, spreadInfos[pos]);
        return range;
    }

    private void AddRange(ref List<Vector3Int> range, Vector3Int originPos, Vector3Int mapSize, SpreadInfo info)
    {
        // ��Ī�� ������ �� ���⿡ ���ؼ� �̸� ����.
        List<SpreadInfo.SpreadType> dirs = info.SpreadTypes;

        // ��� ���⿡ ���ؼ� ����.
        foreach (var dir in dirs)
        {
            // ���� ��ġ�� �������� ���� ����.
            Vector3Int pos = originPos + info.pos;
            int r = info.range;
            if (r == 0) r = 99; // 0���� �Ǿ��ִٸ� �������� ����.

            // �ش� �������� ������ �� ����.
            // �� ������ �����ٸ� �� �̻� �ش� ���� ���� X
            while (pos.x >= 0 && pos.x < mapSize.x
                && pos.y >= 0 && pos.y < mapSize.y
                && r > 0)
            {
                int interval = info.interval;
                do
                {
                    // �� ���⺰�� ����.
                    switch (dir)
                    {
                        case SpreadInfo.SpreadType.LEFT_UP: pos += Vector3Int.left + Vector3Int.up; break;
                        case SpreadInfo.SpreadType.UP: pos += Vector3Int.up; break;
                        case SpreadInfo.SpreadType.RIGHT_UP: pos += Vector3Int.right + Vector3Int.up; break;
                        case SpreadInfo.SpreadType.RIGHT: pos += Vector3Int.right; break;
                        case SpreadInfo.SpreadType.RIGHT_DOWN: pos += Vector3Int.right + Vector3Int.down; break;
                        case SpreadInfo.SpreadType.DOWN: pos += Vector3Int.down; break;
                        case SpreadInfo.SpreadType.LEFT_DOWN: pos += Vector3Int.left + Vector3Int.down; break;
                        case SpreadInfo.SpreadType.LEFT: pos += Vector3Int.left; break;
                    }
                    r--;
                    interval--;
                } while (interval >= 0);

                // �� ������ �����ٸ� �� �̻� �ش� ���� ���� X
                if (pos.x < 0 || pos.x >= mapSize.x || pos.y < 0 || pos.y >= mapSize.y) break;

                // �̹� ������ ��ŵ.
                if (range.Contains(pos)) continue;

                // �ش� ������ �� �Ѿ�ٸ� �߰�.
                range.Add(pos);
            }
        }
    }
    public void AddRange(Vector3Int pos, bool b)
    {
        if (attackRange == null) attackRange = new List<Vector3Int>();

        if (b && !attackRange.Contains(pos)) attackRange.Add(pos);
        else if (!b && attackRange.Contains(pos)) attackRange.Remove(pos);

        AssetDatabase.SaveAssets();
    }

    public void Changed()
    {
        OnChanged?.Invoke();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Data", menuName = "Create Data/Card")]
public class Card : Data
{    public static Card None { get { return null; } }

    // ī�� ���� ���� ������ ���� �� �׷� �͵�
    // ī�� Ÿ�� ����, �Ͻ��� ���

    public enum CardType { ATTACK, DEFEND, MOVE, HEAL, }
    public CardType type;
    public int usable = -1; // -1 = ������ ���
    public float value;
    public float cost;

    public Attack style;
    public event Action OnTypeChanged;

    public void TypeChanged()
    {
        OnTypeChanged?.Invoke();
    }

}

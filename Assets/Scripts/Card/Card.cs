using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Data", menuName = "Create Data/Card")]
public class Card : Data
{    public static Card None { get { return null; } }

    // 카드 종류 공격 아이템 버프 뭐 그런 것들
    // 카드 타입 영구, 일시적 등등

    public enum CardType { ATTACK, DEFEND, MOVE, HEAL, }
    public CardType type;
    public int usable = -1; // -1 = 영구적 사용
    public float value;
    public float cost;

    public Attack style;
    public event Action OnTypeChanged;

    public void TypeChanged()
    {
        OnTypeChanged?.Invoke();
    }

}

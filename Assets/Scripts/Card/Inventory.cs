using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    public const int SIZE = 3;
    private static Card[] equips = new Card[3];
    private static Card[] inventory = new Card[SIZE];

    public static void Clear()
    {
        for (int i = 0; i < SIZE; i++)
        {
            equips[i] = Card.None;
            inventory[i] = Card.None;
        }
    }

    public static void AddCard(Card card)
    {

    }
}

using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    public const int SIZE = 3;
    private static Card[] equips = new Card[3];
    private static Card[] inventory = new Card[SIZE];
    public static int SelectedEquipSlotIndex { get { return selectedEquipSlotIndex; } }
    private static int selectedEquipSlotIndex;

    public static void Clear()
    {
        for (int i = 0; i < SIZE; i++)
        {
            equips[i] = Card.None;
            inventory[i] = Card.None;
        }
    }

    public static Card EquipCard
    {
        get
        {
            return equips[SelectedEquipSlotIndex];
        }
    }

    public static void SelectEquip(int index)
    {
        selectedEquipSlotIndex = index;
        if (selectedEquipSlotIndex < 0) selectedEquipSlotIndex = 2;
        else if (selectedEquipSlotIndex > 2) selectedEquipSlotIndex = 0;

        PlayerController.Instance.ShowPlayerRange();
    }

    public static void SetEquip(Card card, int index)
    {
        equips[index] = card;
    }

    public static void AddCard(Card card)
    {

    }
}

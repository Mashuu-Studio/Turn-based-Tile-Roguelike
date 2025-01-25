using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private RectTransform[] equipSlots;
    [SerializeField] private RectTransform selectedSlot;

    public RectTransform rectTransform { get { return (RectTransform)transform; } }

    public void SetActive(bool b)
    {
        rectTransform.anchoredPosition = new Vector2(0, b ? 0 : -160);
    }

    public void SelectSlot(int index)
    {
        selectedSlot.anchoredPosition = equipSlots[Inventory.SelectedEquipSlotIndex].anchoredPosition;
    }

    public void ScrollSlot(int dir)
    {
        Inventory.SelectEquip(Inventory.SelectedEquipSlotIndex + dir);
        SelectSlot(Inventory.SelectedEquipSlotIndex);
    }
}

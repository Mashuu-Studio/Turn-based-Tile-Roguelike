using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private RectTransform[] equipSlots;
    [SerializeField] private RectTransform selectedSlot;
    private int selectedSlotIndex = 0;

    public RectTransform rectTransform { get { return (RectTransform)transform; } }

    public void SetActive(bool b)
    {
        rectTransform.anchoredPosition = new Vector2(0, b ? 0 : -160);
    }

    public void SelectSlot(int index)
    {
        selectedSlotIndex = index;
        selectedSlot.anchoredPosition = equipSlots[selectedSlotIndex].anchoredPosition;
    }

    public void ScrollSlot(int dir)
    {
        selectedSlotIndex += dir;
        if (selectedSlotIndex < 0) selectedSlotIndex = 2;
        else if (selectedSlotIndex > 2) selectedSlotIndex = 0;

        SelectSlot(selectedSlotIndex);
    }
}

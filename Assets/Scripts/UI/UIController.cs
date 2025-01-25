using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get { return instance; } }
    private static UIController instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventory = !inventory;
            OpenInventory(inventory);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            inventoryUI.ScrollSlot(scroll > 0 ? -1 : 1);
        }

        if (Input.GetButtonDown("Equip1")) inventoryUI.SelectSlot(0);
        if (Input.GetButtonDown("Equip2")) inventoryUI.SelectSlot(1);
        if (Input.GetButtonDown("Equip3")) inventoryUI.SelectSlot(2);
    }

    [SerializeField] private InventoryUI inventoryUI;
    private bool inventory = false;

    public void OpenInventory(bool b)
    {
        inventoryUI.SetActive(b);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UnitUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI dmgText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI actionsText;
    private UnitObject unit = null;

    public void SetUnit(UnitObject unit)
    {
        gameObject.SetActive(true);
        this.unit = unit;
        portrait.sprite = SpriteManager.GetSprite(unit.Data.key.Replace("UNIT", "PORTRAIT"));
        portrait.color = Color.white;
    }

    private void Update()
    {
        if (unit == null) return;

        hpText.text = unit.Hp.ToString();
        dmgText.text = unit.Dmg.ToString();
        speedText.text = unit.Speed.ToString();
        actionsText.text = unit.Actions.ToString();

        // Á×¾úÀ» °æ¿ì
        if (unit.gameObject.activeSelf == false)
        {
            unit = null;
            portrait.color = Color.red;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameController.Instance.IsStart) return;
        if (unit != null) unit.Dragging(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (unit != null) unit.Dragging(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (unit != null) unit.Dragging(false);
    }
}

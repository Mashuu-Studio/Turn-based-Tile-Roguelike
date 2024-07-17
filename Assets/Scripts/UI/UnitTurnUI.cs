using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitTurnUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> images;
    [SerializeField] private List<Image> borderlines;
    [SerializeField] private List<Image> portraits;
    [SerializeField] private TextMeshProUGUI anotherRemainActionsText;
    private int remainActions;

    public void SetTurn(UnitObject unit)
    {
        var sprite = SpriteManager.GetSprite(unit.Data.key.Replace("UNIT", "PORTRAIT"));
        // �ൿ�� ���� �� �ϴ� ������ ��� �������� ���� ���� ǥ��Ǿ����.
        for (int i = 0; i < portraits.Count; i++)
        {
            portraits[i].sprite = sprite;
            borderlines[i].color = unit.IsEnemy ? Color.red : Color.green;
            images[i].gameObject.SetActive(i < unit.RemainAction);
        }

        // ���� ���� �ൿ ���� �������� 1 ���� �� �ڿ� Action�� ������� 
        // ���� ���� �����ָ鼭 UI�� ��������.
        remainActions = unit.RemainAction + 1;
        Action();
    }

    // �ൿ�� ������ �Ŀ� ���� ���� �ൿ�� �ִ��� bool������ return
    public bool Action()
    {
        remainActions--;
        var anotherRemainActions = remainActions - portraits.Count;
        // �ʻ�ȭ ǥ��� ������ ��ġ�� ���ڷ� ǥ��.
        // ���� ���Ǿ��ٸ� ����.
        anotherRemainActionsText.text = $"+{anotherRemainActions}";
        anotherRemainActionsText.gameObject.SetActive(anotherRemainActions > 0);
        if (remainActions > 0)
        {
            // ���� ���� �׼��� �ִٸ� �ϳ��� ������.
            images[remainActions].gameObject.SetActive(false);
            return true;
        }
        else return false;
    }
}

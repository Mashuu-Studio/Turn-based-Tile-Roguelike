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
        // 행동을 여러 번 하는 유닛의 경우 겹쳐져서 여러 개가 표기되어야함.
        for (int i = 0; i < portraits.Count; i++)
        {
            portraits[i].sprite = sprite;
            borderlines[i].color = unit.IsEnemy ? Color.red : Color.green;
            images[i].gameObject.SetActive(i < unit.RemainAction);
        }

        // 실제 남은 행동 수를 기존보다 1 높게 한 뒤에 Action을 실행시켜 
        // 원래 수로 맞춰주면서 UI를 세팅해줌.
        remainActions = unit.RemainAction + 1;
        Action();
    }

    // 행동을 진행한 후에 아직 남은 행동이 있는지 bool형으로 return
    public bool Action()
    {
        remainActions--;
        var anotherRemainActions = remainActions - portraits.Count;
        // 초상화 표기로 부족한 수치는 글자로 표기.
        // 전부 사용되었다면 없앰.
        anotherRemainActionsText.text = $"+{anotherRemainActions}";
        anotherRemainActionsText.gameObject.SetActive(anotherRemainActions > 0);
        if (remainActions > 0)
        {
            // 아직 남은 액션이 있다면 하나씩 없애줌.
            images[remainActions].gameObject.SetActive(false);
            return true;
        }
        else return false;
    }
}

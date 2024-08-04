using MapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SupportItemView : Button
{
    public bool activate;
    private int presetNumber;

    public SupportItemView(string style, int presetNumber = -1)
    {
        ClearClassList();
        AddToClassList(style);

        this.presetNumber = presetNumber;
    }

    public void Actiavte()
    {
        // 다른 서포트 아이템들. 토글 방식으로 작동.
        if (presetNumber == -1)
        {
            activate = !activate;
            style.unityBackgroundImageTintColor = activate ? TilePaletteView.SelectedColor : TilePaletteView.UnselectedColor;
        }
        // 프리셋의 경우 즉각 작동.
        else
        {
        }
    }
}

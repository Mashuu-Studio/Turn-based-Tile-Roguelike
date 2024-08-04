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
        // �ٸ� ����Ʈ �����۵�. ��� ������� �۵�.
        if (presetNumber == -1)
        {
            activate = !activate;
            style.unityBackgroundImageTintColor = activate ? TilePaletteView.SelectedColor : TilePaletteView.UnselectedColor;
        }
        // �������� ��� �ﰢ �۵�.
        else
        {
        }
    }
}

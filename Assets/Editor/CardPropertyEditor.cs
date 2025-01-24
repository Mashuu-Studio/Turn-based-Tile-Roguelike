using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Card))]
public class CardPropertyEditor : Editor
{
    private Card.CardType previousType;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var card = target as Card;

        if (previousType != card.type)
        {
            card.TypeChanged();
            previousType = card.type;
        }
    }
}

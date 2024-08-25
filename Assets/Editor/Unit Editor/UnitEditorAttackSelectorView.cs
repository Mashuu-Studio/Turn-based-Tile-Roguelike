using AttackEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnitEditor
{
    public class UnitEditorAttackSelectorView : ScrollView
    {
        public new class UxmlFactory : UxmlFactory<UnitEditorAttackSelectorView, UxmlTraits> { }

        UnitEditorAttackView attackView;

        internal void PopulateView(UnitEditorAttackView attackView)
        {
            this.attackView = attackView;
            ClearClassList();
            AddToClassList("Attack_Selector");
            // 드랍다운 세팅.

            Clear();
            Attack[] attacks = Resources.LoadAll<Attack>("Attacks");
            foreach (Attack attack in attacks)
            {
                CreateItem(attack);
            }
        }

        private void CreateItem(Attack attack)
        {
            UnitEditorAttackItem item = new UnitEditorAttackItem(attack);
            item.RegisterCallback<PointerDownEvent>(evt => attackView.DrawView(attack));
            Add(item);
        }
    }
}

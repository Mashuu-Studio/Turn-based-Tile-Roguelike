using MapEdtior;
using System.Collections;
using System.Collections.Generic;
using UnitEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class MapEditorUnitSelectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<MapEditorUnitSelectorView, UxmlTraits> { }

        MapEditorView mapEditorView;
        public Unit selectedUnit;

        internal void PopulateView(MapEditorView mapEditorView)
        {
            this.mapEditorView = mapEditorView;
            ClearClassList();
            AddToClassList("Attack_Selector");
            // 드랍다운 세팅.

            Clear();
            Unit[] units = Resources.LoadAll<Unit>("Units");
            foreach (Unit unit in units)
            {
                CreateItem(unit);
            }
        }

        private void CreateItem(Unit unit)
        {
            MapEditorUnitItem item = new MapEditorUnitItem(unit);
            item.RegisterCallback<PointerDownEvent>(evt => selectedUnit = unit);
            Add(item);
        }
    }
}
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEditor;
using UnityEngine;

public class TriDropdownWithSearchElement : TriElement
{
    private readonly TriProperty property;
    private readonly Func<TriProperty, IEnumerable<ITriDropdownItem>> getItems;
    private string selectedNameCache = "";

    public TriDropdownWithSearchElement(TriProperty property, Func<TriProperty, IEnumerable<ITriDropdownItem>> getItems)
    {
        this.property = property;
        this.getItems = getItems;
    }

    public override void OnGUI(Rect position)
    {
        var value = property.Value;
        var enumerable = getItems(property);
        selectedNameCache = enumerable.FirstOrDefault(x => Equals(x.Value, value))?.Text ?? "<None>";

        
        if (EditorGUI.DropdownButton(position, new GUIContent(selectedNameCache), FocusType.Keyboard))
        {
            Rect screenRect = GUIUtility.GUIToScreenRect(position);
            DropdownWithSearchPopup.Show(screenRect, getItems(property).ToList(), value, selected => { property.SetValue(selected); });
        }
    }

    public override float GetHeight(float width) => EditorGUIUtility.singleLineHeight;
}
#endif
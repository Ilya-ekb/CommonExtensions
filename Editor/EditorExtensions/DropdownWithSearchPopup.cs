#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEditor;
using UnityEngine;

public class DropdownWithSearchPopup : EditorWindow
{
    private List<ITriDropdownItem> items;
    private object currentValue;
    private Action<object> onSelected;
    private string search = "";
    private Vector2 scroll;
    private int selectedIndex = -1;
    private bool justOpened = true;

    public static void Show(Rect buttonRect, List<ITriDropdownItem> items, object currentValue,
        Action<object> onSelected)
    {
        var window = CreateInstance<DropdownWithSearchPopup>();
        window.items = items;
        window.currentValue = currentValue;
        window.justOpened = true;
        window.onSelected = onSelected;

        window.ShowAsDropDown(buttonRect, new Vector2(buttonRect.width, 240));
    }

    private void OnGUI()
    {
        GUI.SetNextControlName("SearchField");
        search = EditorGUILayout.TextField(search, EditorStyles.toolbarSearchField);
        EditorGUI.FocusTextInControl("SearchField");

        var filtered = string.IsNullOrEmpty(search)
            ? items
            : items.Where(x => x.Text != null && x.Text.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

        if (selectedIndex >= filtered.Count) selectedIndex = filtered.Count - 1;
        if (selectedIndex < 0 && filtered.Count > 0)
        {
            selectedIndex = filtered.FindIndex(x => Equals(x.Value, currentValue));
            if (selectedIndex < 0) selectedIndex = 0;
        }
        
        if (justOpened && selectedIndex >= 0 && filtered.Count > 0)
        {
            float itemHeight = EditorGUIUtility.singleLineHeight + 2;
            scroll.y = selectedIndex * itemHeight;
            justOpened = false;
        }

        HandleKeyboard(filtered);

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(200));
        for (int i = 0; i < filtered.Count; i++)
        {
            var item = filtered[i];
            bool isCurrent = Equals(item.Value, currentValue);
            bool isHighlighted = (i == selectedIndex);

            var prevColor = GUI.backgroundColor;
            if (isHighlighted)
            {
                GUI.backgroundColor = new Color(0.6f, 0.8f, 1f);
            }
            else if (isCurrent)
                GUI.backgroundColor = new Color(0.7f, 1f, 0.7f);

            if (GUILayout.Button(item.Text, EditorStyles.toolbarTextField, GUILayout.ExpandWidth(true)))
            {
                onSelected?.Invoke(item.Value);
                Close();
            }

            GUI.backgroundColor = prevColor;
        }

        EditorGUILayout.EndScrollView();

        if (filtered.Count == 0)
            EditorGUILayout.LabelField("No items found", EditorStyles.centeredGreyMiniLabel);
    }

    private void HandleKeyboard(List<ITriDropdownItem> filtered)
    {
        Event e = Event.current;
        if (e.type != EventType.Used || filtered.Count == 0)
            return;

        if (e.keyCode == KeyCode.DownArrow)
        {
            selectedIndex = Mathf.Min(selectedIndex + 1, filtered.Count - 1);
            e.Use();
            ScrollToSelected(filtered);
        }
        else if (e.keyCode == KeyCode.UpArrow)
        {
            selectedIndex = Mathf.Max(selectedIndex - 1, 0);
            e.Use();
            ScrollToSelected(filtered);
        }
        else if (e.keyCode is KeyCode.Return or KeyCode.KeypadEnter)
        {
            if (selectedIndex >= 0 && selectedIndex < filtered.Count)
            {
                onSelected?.Invoke(filtered[selectedIndex].Value);
                Close();
                e.Use();
            }
        }
        else if (e.keyCode == KeyCode.Escape)
        {
            Close();
            e.Use();
        }
    }

    private void ScrollToSelected(List<ITriDropdownItem> filtered)
    {
        float itemHeight = EditorGUIUtility.singleLineHeight + 2;
        float viewHeight = 200;
        float top = selectedIndex * itemHeight;
        float bottom = top + itemHeight;

        if (scroll.y > top)
            scroll.y = top;
        else if (scroll.y + viewHeight < bottom)
            scroll.y = bottom - viewHeight;
    }
}
#endif
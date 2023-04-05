using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonTapColorChange : MonoBehaviour
{
    [SerializeField] private UI_Manager ui_Manager = null;
    private Button btn = null;
    private Color defaultNomalColor;

    private void Awake()
    {
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TapColorChange);
        defaultNomalColor = btn.colors.normalColor;
    }

    private void TapColorChange()
    {
        if (ui_Manager.IsSameContent())
        {
            ColorBlock cb = btn.colors;
            cb.normalColor = defaultNomalColor;
            cb.selectedColor = defaultNomalColor;
            btn.colors = cb;
        }
        else
        {
            ColorBlock cb = btn.colors;
            cb.normalColor = Color.grey;
            cb.selectedColor = Color.grey;
            btn.colors = cb;
        }

    }


}

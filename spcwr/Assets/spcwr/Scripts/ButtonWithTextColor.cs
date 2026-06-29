using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithTextColor : Button
{
    public TMP_Text targetText;
    public ColorBlock textColorBlock;

    // Update is called once per frame
    void Update()
    {
        switch (currentSelectionState)
        {
            case SelectionState.Normal:
                targetText.color = textColorBlock.normalColor;
                break;
            case SelectionState.Highlighted:
                targetText.color = textColorBlock.highlightedColor;
                break;
            case SelectionState.Pressed:
                targetText.color = textColorBlock.pressedColor;
                break;
            case SelectionState.Selected:
                targetText.color = textColorBlock.selectedColor;
                break;
            case SelectionState.Disabled:
                targetText.color = textColorBlock.disabledColor;
                break;
            default:
                break;
        }
    }
}
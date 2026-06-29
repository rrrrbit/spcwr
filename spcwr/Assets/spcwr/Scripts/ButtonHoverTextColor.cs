using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverTextColor : MonoBehaviour
{
    public Color normalColor;
    public Color highlightedColor;
    public Color pressedColor;
    public Color selectedColor;
    public Color disabledColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var color = normalColor;
        color = GetComponent<Button>().interactable ? normalColor : disabledColor;
        //color = GetComponent<Button>().spriteState
    }
}

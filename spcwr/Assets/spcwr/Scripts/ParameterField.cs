using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
public class ParameterField : MonoBehaviour
{
    public TMP_Text text;
    public TMP_InputField inputField;

    public Button leftButton;
    public Button rightButton;

    UnityAction<string> showBtnsAction;
    UnityAction<string> hideBtnsAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        showBtnsAction += delegate { leftButton.interactable = true; rightButton.interactable = true; };
        hideBtnsAction += delegate { leftButton.interactable = false; rightButton.interactable = false; };

        inputField.onSelect.AddListener(showBtnsAction);
        inputField.onDeselect.AddListener(hideBtnsAction);
    }
}

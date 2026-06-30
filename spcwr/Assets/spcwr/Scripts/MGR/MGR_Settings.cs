using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class MGR_Settings : MonoBehaviour
{
    public ParameterField paramFieldPrefab;
    public RectTransform paramsContainer;

    public ParameterField[] paramFields;

    public InputSystemUIInputModule inputModule;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SVC_Data data = SVC.Get<SVC_Data>();

        paramFields = new ParameterField[data.gameParameters.Length];

        for (int i = 0; i < paramFields.Length; i++)
        {
            var thisField = Instantiate(paramFieldPrefab, paramsContainer);
            thisField.text.text = (data.gameParameters[i].name + " ").PadRight(27, '-');
            thisField.inputField.text = data.gameParameters[i].value.ToString();
            paramFields[i] = thisField;
        }
    }

    // Update is called once per frame
    void Update()
    {

        var upDown = inputModule.move.ToInputAction().ReadValue<Vector2>().y;
        Selectable currentSelectable = null;
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            currentSelectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            print("found selectable");
        }

        if (currentSelectable != null && currentSelectable.TryGetComponent<TMP_InputField>(out _))
        {
            print("selectable is input field");
            if (upDown < 0)
            {
                EventSystem.current.SetSelectedGameObject(currentSelectable.FindSelectableOnDown().gameObject);
                print("go down");
            }
            else if (upDown > 0)
            {
                EventSystem.current.SetSelectedGameObject(currentSelectable.FindSelectableOnUp().gameObject);
                print("go up");


            }
        }
        
    }
}

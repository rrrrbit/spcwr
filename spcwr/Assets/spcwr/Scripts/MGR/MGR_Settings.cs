using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
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
        print(inputModule.move.ToInputAction().ReadValue<Vector2>());
        //FUCKKKKK UNITY'S UIIIIIIII
    }
}

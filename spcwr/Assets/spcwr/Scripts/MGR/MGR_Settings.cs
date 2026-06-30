using RBitUtils;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class MGR_Settings : MonoBehaviour
{
    public ParameterField paramFieldPrefab;
    public RectTransform paramsContainer;

    public ParameterField[] paramFields;

    public InputSystemUIInputModule inputModule;

    public ScrollRect scrollRect;

    [SerializeField] float navigateTime;
    Vector2 navigateTimer;
    
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
            thisField.step = data.gameParameters[i].step;
            paramFields[i] = thisField;
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("mainMenu");
    }

    public void Proceed() // KRIS!!!!!!!!!!!!!!!!!!!!!!!
    {
        SceneManager.LoadScene("game");

    }

    // Update is called once per frame
    void Update()
    {
        navigateTimer = Vector2.Max(navigateTimer - Vector2.one * Time.deltaTime, Vector2.zero);
        Vector2 navigate = inputModule.move.ToInputAction().ReadValue<Vector2>();
        Selectable currentSelectable = null;
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            currentSelectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        }

        if (currentSelectable != null && currentSelectable.TryGetComponent<TMP_InputField>(out var input))
        {
            if (navigateTimer.x == 0)
            {
                if (navigate.x < 0)
                {
                    input.GetComponentInParent<ParameterField>().StepNeg();
                    print("go left");
                    navigateTimer.x = navigateTime;
                }
                else if (navigate.x > 0)
                {
                    input.GetComponentInParent<ParameterField>().StepPos();

                    print("go right");
                    navigateTimer.x = navigateTime;
                }
            }

            if (navigateTimer.y == 0)
            {
                if (navigate.y < 0)
                {
                    EventSystem.current.SetSelectedGameObject(currentSelectable.FindSelectableOnDown().gameObject);
                    scrollRect.ScrollTo((RectTransform)EventSystem.current.currentSelectedGameObject.transform, 32);
                    print("go down");
                    navigateTimer.y = navigateTime;
                }
                else if (navigate.y > 0)
                {
                    EventSystem.current.SetSelectedGameObject(currentSelectable.FindSelectableOnUp().gameObject);
                    scrollRect.ScrollTo((RectTransform)EventSystem.current.currentSelectedGameObject.transform, 32);

                    print("go up");
                    navigateTimer.y = navigateTime;
                }
            }
            
            
        }
        
    }
}

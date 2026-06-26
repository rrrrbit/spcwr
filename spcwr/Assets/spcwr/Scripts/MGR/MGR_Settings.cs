using UnityEngine;

public class MGR_Settings : MonoBehaviour
{
    public ParameterField paramFieldPrefab;
    public RectTransform paramsContainer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SVC_Data data = SVC.Get<SVC_Data>();

        foreach (var item in data.gameParameters)
        {
            var thisField = Instantiate(paramFieldPrefab, paramsContainer);
            thisField.text.text = (item.name+" ").PadRight(27, '-');
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

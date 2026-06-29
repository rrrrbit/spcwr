using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ButtonWithTextColor))]
public class ButtonWithTextColorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

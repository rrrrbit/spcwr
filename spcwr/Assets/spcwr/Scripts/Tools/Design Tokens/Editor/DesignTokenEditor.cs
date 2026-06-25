using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DesignToken<>), true)]
public class EDITOR_DesignToken : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Reload"))
        {
            ((IDesignToken)target).InvokeReload();
        }
    }
}

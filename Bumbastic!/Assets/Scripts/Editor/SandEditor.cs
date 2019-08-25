using UnityEditor;

[CustomEditor(typeof(Sand))]
public class SandEditor : Editor
{
    SerializedProperty changeParticlesColor;

    SerializedProperty desertColor;
    SerializedProperty winterColor;

    private void OnEnable()
    {
        changeParticlesColor = serializedObject.FindProperty("changeParticlesColor");

        desertColor = serializedObject.FindProperty("desertColor");
        winterColor = serializedObject.FindProperty("winterColor");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        changeParticlesColor.boolValue = EditorGUILayout.Toggle("Change particles color", changeParticlesColor.boolValue);

        if (changeParticlesColor.boolValue)
        {
            EditorGUI.indentLevel = 1;
            desertColor.colorValue = EditorGUILayout.ColorField("Desert Color", desertColor.colorValue);
            winterColor.colorValue = EditorGUILayout.ColorField("Winter Color", winterColor.colorValue);
            EditorGUI.indentLevel = 0;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

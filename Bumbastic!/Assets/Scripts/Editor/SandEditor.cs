using UnityEditor;

[CustomEditor(typeof(Sand))]
public class SandEditor : Editor
{
    SerializedProperty changeParticlesColor;

    SerializedProperty desertColor;
    SerializedProperty winterColor;

    Sand m_Sand;

    private void OnEnable()
    {
        m_Sand = (Sand)target;

        changeParticlesColor = serializedObject.FindProperty("changeParticlesColor");

        desertColor = serializedObject.FindProperty("desertColor");
        winterColor = serializedObject.FindProperty("winterColor");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        changeParticlesColor.boolValue = EditorGUILayout.Toggle("Change particles color", m_Sand.ChangeParticlesColor);

        if (changeParticlesColor.boolValue)
        {
            EditorGUI.indentLevel = 1;
            desertColor.colorValue = EditorGUILayout.ColorField("Desert Color", m_Sand.DesertColor);
            winterColor.colorValue = EditorGUILayout.ColorField("Winter Color", m_Sand.WinterColor);
            EditorGUI.indentLevel = 0;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

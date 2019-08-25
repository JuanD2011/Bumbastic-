using UnityEditor;

[CustomEditor(typeof(Velocity))]
public class VelocityEditor : Editor
{
    public override void OnInspectorGUI() { }
}

[CustomEditor(typeof(Shield))]
public class ShieldEditor : Editor
{
    public override void OnInspectorGUI() { }
}

[CustomEditor(typeof(Magnet))]
public class MagnetEditor : Editor
{
    SerializedProperty lerpDuration;

    Magnet m_Magnet;

    private void OnEnable()
    {
        m_Magnet = (Magnet)target;

        lerpDuration = serializedObject.FindProperty("lerpDuration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        lerpDuration.floatValue = EditorGUILayout.FloatField("Lerp duration", m_Magnet.LerpDuration);
        serializedObject.ApplyModifiedProperties();
    }
}

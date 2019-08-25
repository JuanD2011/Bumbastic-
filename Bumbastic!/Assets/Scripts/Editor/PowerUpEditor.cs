using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Velocity))]
public class VelocityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((Velocity)target), typeof(Velocity), false);
        GUI.enabled = true;
    }
}

[CustomEditor(typeof(Shield))]
public class ShieldEditor : Editor
{
    SerializedProperty bounceForce, timeToLerpBomb;

    private void OnEnable()
    {
        bounceForce = serializedObject.FindProperty("bounceForce");
        timeToLerpBomb = serializedObject.FindProperty("timeToLerpBomb");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((Shield)target), typeof(Shield), false);
        GUI.enabled = true;
        bounceForce.floatValue = EditorGUILayout.FloatField(new GUIContent("Bounce force", "When any player touches the shield"), bounceForce.floatValue);
        timeToLerpBomb.floatValue = EditorGUILayout.FloatField(new GUIContent("Lerp duration", "Lerp time when the bomb bounces to the nearest player"), timeToLerpBomb.floatValue);
        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(Magnet))]
public class MagnetEditor : Editor
{
    SerializedProperty lerpDuration;

    private void OnEnable()
    {
        lerpDuration = serializedObject.FindProperty("lerpDuration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((Magnet)target), typeof(Magnet), false);
        GUI.enabled = true;
        lerpDuration.floatValue = EditorGUILayout.FloatField("Lerp duration", lerpDuration.floatValue);
        serializedObject.ApplyModifiedProperties();
    }
}

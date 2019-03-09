using UnityEngine;

[CreateAssetMenu(fileName ="Settings",menuName ="Settings")]
public class Settings : ScriptableObject
{
    [Header("Configuration Settings")]
    #region ConfigurationSettings
    public bool isMusicActive;
    public bool isSfxActive;
    #endregion
}

using UnityEngine;

[CreateAssetMenu(fileName ="Settings",menuName ="Settings")]
public class Settings : ScriptableObject
{
    [Header("Configuration Settings")]
    #region ConfigurationSettings
    public bool isMusicActive;
    public bool isSfxActive;

    public float musicSlider;
    public float sFxSlider;

    public void MusicSlider(float _musicVol) { musicSlider = _musicVol; }
    public void SfxSlider(float _sFXVol) { sFxSlider = _sFXVol; }

    public byte languageID = 0;
    #endregion

    public Color[] playersColor = { Color.red, Color.green, Color.blue, Color.yellow };
}

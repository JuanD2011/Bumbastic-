using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CanvasBillboard : MonoBehaviour
{
    [SerializeField] Settings settings = null;

    Player player = null;
    ThrowerPlayer throwerPlayer = null;

    TextMeshProUGUI[] playersText;
    Image playerColor = null;
    Slider dashCountSlider = null;

    float valueToIncrease = 0f;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        playersText = GetComponentsInChildren<TextMeshProUGUI>();
        playerColor = GetComponentInChildren<Image>();
        dashCountSlider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        playersText[0].text = Translation.Fields[string.Format("P{0}", player.Id + 1)];
        playerColor.color = settings.playersColor[player.Id];
        playersText[1].text = string.Format("{0}", player.PrefabName);

        if (dashCountSlider != null)
        {
            valueToIncrease = dashCountSlider.maxValue / GameManager.maximunDashLevel; 
        }

        GameManager.Manager.OnCorrectPassBomb += UpdateDashCounter;
        if (throwerPlayer != null)
        {
            throwerPlayer.OnDashExecuted += UpdateDashCounter; 
        }
    }

    private void UpdateDashCounter(Player _player)
    {
        if (_player != player || !gameObject.activeInHierarchy) return;

        StartCoroutine(LerpSlider());

        //if (_player.DashCount == GameManager.numberToReachDash)
        //    uIParticleSystem.gameObject.SetActive(true);
        //else if (_player.DashCount == 0)
        //    uIParticleSystem.gameObject.SetActive(false);
    }

    IEnumerator LerpSlider()
    {
        float time = 0.15f, elapsedTime = 0f;
        float initValue = dashCountSlider.value;

        while (dashCountSlider.value != valueToIncrease * throwerPlayer.DashCount)
        {
            dashCountSlider.value = Mathf.Lerp(initValue, valueToIncrease * throwerPlayer.DashCount, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                                                       Camera.main.transform.rotation * Vector3.up);
        else Debug.LogWarning("Set to the camera the MainCamera tag");
    }
}

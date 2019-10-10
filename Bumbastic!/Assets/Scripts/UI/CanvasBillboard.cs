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

    Transform camTransform = null;
    Quaternion originalRotation = Quaternion.identity;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        throwerPlayer = GetComponentInParent<ThrowerPlayer>();
        playersText = GetComponentsInChildren<TextMeshProUGUI>();
        playerColor = GetComponentInChildren<Image>();
        dashCountSlider = GetComponentInChildren<Slider>();

        camTransform = Camera.main.transform;
    }

    private void Start()
    {
        originalRotation = transform.localRotation;

        playersText[0].text = Translation.Fields[string.Format("P{0}", player.Id + 1)];
        if (!GameModeDataBase.IsCurrentBasesGame())
        {
            playerColor.color = settings.playersColor[player.Id];
        }
        else
        {
            for (int i = 0; i < BasesGameManager.basesGame.Teams[0].Members.Count; i++)
            {
                if (player == BasesGameManager.basesGame.Teams[0].Members[i])
                {
                    playerColor.color = BasesGameManager.basesGame.Teams[0].TeamColor;
                }
                else if(player == BasesGameManager.basesGame.Teams[1].Members[i])
                {
                    playerColor.color = BasesGameManager.basesGame.Teams[1].TeamColor;
                }
            }
        }

        playersText[1].text = string.Format("{0}", player.PrefabName);

        if (dashCountSlider != null) valueToIncrease = dashCountSlider.maxValue / throwerPlayer.MaximunDashLevel;

        GameManager.Manager.OnCorrectPassBomb += UpdateDashCounter;

        if (throwerPlayer != null) throwerPlayer.OnDashExecuted += UpdateDashCounter;
    }

    private void Update()
    {
        transform.rotation = camTransform.rotation * originalRotation;
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
}

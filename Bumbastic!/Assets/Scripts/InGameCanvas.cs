using UnityEngine;
using TMPro;
using System.Collections;

public class InGameCanvas : MonoBehaviour
{
    Animator m_Animator;
    [SerializeField] TextMeshProUGUI textWinner;

    public delegate IEnumerator DelLoadString(string _scene);
    public static DelLoadString OnLoadScene;

    bool isEndPanelActive = false;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        GameManager.manager.OnGameOver += SetEndAnimation;
        Player.OnStartInGame += CanLoadScene;
    }

    private void CanLoadScene()
    {
        if (isEndPanelActive)
        {
            m_Animator.SetTrigger("loadingScreen");
        }
    }

    #region Animation events
    public void OnLoadScreenComplete()
    {
        StartCoroutine(OnLoadScene?.Invoke("Menu"));//Lvl Manager hears it.
    }
    #endregion

    private void SetEndAnimation()
    {
        isEndPanelActive = true;
        m_Animator.SetBool("isGameOver", true);
        textWinner.text = string.Format("P{0}", GameManager.manager.Players[0].Id + 1);
    }
}

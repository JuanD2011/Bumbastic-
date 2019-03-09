using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Animator canvasAnimator;
    [SerializeField] Settings settings;

    public delegate void DelMenu();
    public static DelMenu OnLoadData;

    public void CanLoadData()
    {
        OnLoadData?.Invoke();//Memento hears it.
    }

    public void IsSearchingGame(bool _bool)
    {
        canvasAnimator.SetBool("Play",_bool);
    }

    public void ConfigurationPanel(bool _bool)
    {
        canvasAnimator.SetBool("Menu_Options", _bool);
    }

    public void CreditsPanel(bool _bool)
    {
        canvasAnimator.SetBool("Credits", _bool);
    }

    public void QuitPanel(bool _bool)
    {
        canvasAnimator.SetBool("QuitPanel", _bool);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvasAnimator.GetCurrentAnimatorStateInfo(0).IsName("Menu_Principal"))
            {
                QuitPanel(true);
            }
        }
    }
}
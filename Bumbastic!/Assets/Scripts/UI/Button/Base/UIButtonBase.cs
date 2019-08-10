using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UIButtonBase : MonoBehaviour
{
    protected bool interactuable = false;

    Button m_Button;

    public Button Button { get => m_Button; set => m_Button = value; }

    protected virtual void Awake()
    {
        Button = GetComponent<Button>();
    }

    protected virtual void Start()
    {
        PlayerMenu.OnAcceptButton += OnButtonClicked; 
    }

    protected virtual void OnButtonClicked(byte _id)
    {
        if (EventSystem.current == null) return;
        if (EventSystem.current.currentSelectedGameObject != gameObject || !gameObject.activeInHierarchy)
            interactuable = false;
        else interactuable = true;
    }

    protected void ClickSound(bool _default)
    {
        if (_default)
        {
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.buttonDefault, 0.6f);
        }
        else
        {
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.buttonBack, 0.5f);
        }
    }
}

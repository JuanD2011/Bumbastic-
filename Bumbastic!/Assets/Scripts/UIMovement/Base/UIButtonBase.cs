using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIButtonBase : MonoBehaviour
{
    private void Start()
    {
        PlayerMenu.OnAcceptButton += VerifyButtonClicked;
    }

    private void VerifyButtonClicked(byte _id)
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject && gameObject.activeInHierarchy)
        {
            OnButtonClicked();
        }
    }

    protected abstract void OnButtonClicked();

    protected void ClickSound(bool _default)
    {
        if (_default)
        {
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.buttonDefault, 0.6f);
        }
        else
        {
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.buttonBack, 0.6f);
        }
    }
}

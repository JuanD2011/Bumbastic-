using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIButtonBase : MonoBehaviour
{
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            PlayerMenu.OnAcceptButton += OnButtonClicked;
        }
    }

    protected abstract void OnButtonClicked(byte _id);

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

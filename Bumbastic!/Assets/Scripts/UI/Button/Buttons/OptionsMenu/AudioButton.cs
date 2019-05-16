public class AudioButton : UIButtonBase
{
    AudioMute audioMute;

    protected override void Awake()
    {
        base.Awake();
        audioMute = GetComponent<AudioMute>();
    }

    protected override void OnButtonClicked(byte _id)
    {
        base.OnButtonClicked(_id);

        if (interactuable)
        {
            audioMute.MuteAudios();
            ClickSound(true); 
        }
    }
}

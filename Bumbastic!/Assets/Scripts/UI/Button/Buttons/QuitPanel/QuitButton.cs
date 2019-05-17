using UnityEngine;

public class QuitButton : UIButtonBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnButtonClicked(byte _id)
    {
        base.OnButtonClicked(_id);

        if (interactuable)
        {
            Application.Quit();
            ClickSound(true); 
        }
    }
}

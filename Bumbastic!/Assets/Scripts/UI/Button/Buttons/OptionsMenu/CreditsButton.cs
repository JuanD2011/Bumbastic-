public class CreditsButton : UIButtonBase
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
            MenuManager.menu.menuCanvas.CreditsPanel(true);
            ClickSound(true); 
        }
    }
}

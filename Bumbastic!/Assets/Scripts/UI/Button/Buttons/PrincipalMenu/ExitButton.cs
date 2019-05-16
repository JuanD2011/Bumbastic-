public class ExitButton : UIButtonBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClicked(byte _id)
    {
        base.OnButtonClicked(_id);

        if (interactuable)
        {
            MenuManager.menu.menuCanvas.QuitPanel(true);
            ClickSound(true); 
        }
    }
}

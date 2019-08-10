public class BackButton : UIButtonBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClicked(byte _id)
    {
        base.OnButtonClicked(_id);

        if (interactuable)
        {
            MenuManager.menu.menuCanvas.BackButton(_id);
            ClickSound(false); 
        }
    }
}

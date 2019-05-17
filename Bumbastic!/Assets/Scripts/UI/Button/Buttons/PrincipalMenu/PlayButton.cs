public class PlayButton : UIButtonBase
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
            MenuManager.menu.menuCanvas.MatchmakingPanel(true);
            ClickSound(true); 
        }
    }
}

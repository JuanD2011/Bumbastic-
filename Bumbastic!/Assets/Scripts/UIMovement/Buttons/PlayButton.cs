public class PlayButton : UIButtonBase
{
    protected override void OnButtonClicked(byte _id)
    {
        MenuManager.menu.menuCanvas.MatchmakingPanel(true);
        ClickSound(true);
    }
}

public class PlayButton : UIButtonBase
{
    protected override void OnButtonClicked()
    {
        MenuManager.menu.menuCanvas.MatchmakingPanel(true);
        ClickSound(true);
    }
}

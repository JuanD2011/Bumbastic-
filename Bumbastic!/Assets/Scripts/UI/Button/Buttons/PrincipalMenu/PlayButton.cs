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
            if (MenuManager.menu.Players.Count > 1)
            {
                MenuManager.menu.menuCanvas.MatchmakingPanel(true);
                ClickSound(true);
            }
            else
            {
                print("You need more players");
            }
        }
    }
}

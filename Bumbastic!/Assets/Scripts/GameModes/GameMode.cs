[System.Serializable]
public class GameMode
{
    public string name;
    public string description;
    public int id;

    public GameMode(string _name, string _description, int _id)
    {
        name = _name;
        description = _description;
        id = _id;
    }
}

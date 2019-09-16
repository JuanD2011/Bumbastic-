using UnityEngine;

[CreateAssetMenu(menuName = "World props", fileName = "World props")]
public class WorldProps : ScriptableObject
{
    [SerializeField] Props[] props = null;
    [SerializeField] Props[] basesProps = null;

    public Props[] Props { get => props; private set => props = value; }
    public Props[] BasesProps { get => basesProps; set => basesProps = value; }

    public Props CurrentProps { get; set; } = null;
}

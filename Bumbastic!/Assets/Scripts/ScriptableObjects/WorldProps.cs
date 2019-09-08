using UnityEngine;

[CreateAssetMenu(menuName = "World props", fileName = "World props")]
public class WorldProps : ScriptableObject
{
    [SerializeField] GameObject[] propsDesert = null, propsModuleWinter = null, propsBeach = null;
    [SerializeField] GameObject[] basesProps = null;

    public GameObject[] PropsDesert { get => propsDesert; private set => propsDesert = value; }
    public GameObject[] PropsModuleWinter { get => propsModuleWinter; private set => propsModuleWinter = value; }
    public GameObject[] PropsBeach { get => propsBeach; private set => propsBeach = value; }
    public GameObject[] BasesProps { get => basesProps; set => basesProps = value; }
}

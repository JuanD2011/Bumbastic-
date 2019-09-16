using UnityEngine;

[System.Serializable]
public class Props
{
    [SerializeField] EnumEnviroment m_Type = EnumEnviroment.Desert;
    [SerializeField] GameObject m_Props = null;

    public GameObject PropsGO { get => m_Props; private set => m_Props = value; }
    public EnumEnviroment Type { get => m_Type; private set => m_Type = value; }
}

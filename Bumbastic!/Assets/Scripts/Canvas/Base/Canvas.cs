using System.Collections;
using UnityEngine;

public abstract class Canvas : MonoBehaviour
{
    public delegate IEnumerator DelLoadString(string _scene);
    public static DelLoadString OnLoadScene;

    protected Animator m_Animator;

    [SerializeField] protected string[] animatorStateNames;

    protected virtual void Awake()
    {
        OnLoadScene = null;
    }

    protected virtual void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
}

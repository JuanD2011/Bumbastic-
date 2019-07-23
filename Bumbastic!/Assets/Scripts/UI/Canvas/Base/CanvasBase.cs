using System.Collections;
using UnityEngine;

public abstract class CanvasBase : MonoBehaviour
{
    public delegate IEnumerator DelLoadString(string _scene);
    public static DelLoadString OnLoadScene;

    protected Animator m_Animator;

    [SerializeField] protected string[] animatorStateNames;

    protected virtual void Awake()
    {
        OnLoadScene = null;
        m_Animator = GetComponent<Animator>();
    }
}

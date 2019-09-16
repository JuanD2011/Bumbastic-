using UnityEngine;

public class Prop : MonoBehaviour
{
    float distance = 10;
    Transform[] modules;

    GameObject parent;

    private void Awake()
    {
        modules = GameManager.Manager.floor.GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        for (int i = 1; i < modules.Length; i++)
        {
            if (Vector3.Distance(transform.position, modules[i].transform.position) < distance && modules[i].gameObject.tag == "Floor")
            {
                distance = (Vector3.Distance(transform.position, modules[i].transform.position));
                parent = modules[i].gameObject;
            }
        }

        transform.parent = parent.transform;
    }
}

using UnityEngine;

public class Props : MonoBehaviour
{
    float distance = 10;
    Transform[] modules;

    GameObject padre;

    void Start()
    {
        modules = GameManager.manager.floor.GetComponentsInChildren<Transform>();

        for (int i = 1; i < modules.Length; i++)
        {
            if (Vector3.Distance(transform.position, modules[i].transform.position) < distance && modules[i].gameObject.tag=="Floor")
            {
                distance = (Vector3.Distance(transform.position, modules[i].transform.position));
                padre = modules[i].gameObject;
            }
        }

        transform.parent = padre.transform;
    }
}

using System.Collections;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] bool drop = true;

    [SerializeField]
    GameObject[] propsModule;

	[SerializeField]
	Transform[] colliders;

	[SerializeField]
	float dropTime, dropInterval, anticipationTime;

	[SerializeField]
	Gradient colorAnticipation;
	
	[SerializeField]
	float floorOffset = 0.07f;
	
	[SerializeField]
	Vector3 propsPos = new Vector3(0, 0.5f, 0);
	
    Rigidbody[] modules;
	Rings[] rings;

	int nRings = 0;
	int c = 0;
	int anticipationRing;
	float time = 0;

	bool anticipation = false;
    bool canDrop = false;

    private void Awake()
    {
        SpawnProps(propsModule.Length);
    }

    void Start()
    {
        if (drop)
        {
            Init();
            Bomb.OnExplode += MapDrop; 
        }
        else
        {
            foreach(Transform collider in colliders)
            {
                collider.gameObject.SetActive(false);
            }
        }
    }

    private void Init()
    {
        modules = GetComponentsInChildren<Rigidbody>();

        while (modules.Length >= (Mathf.Pow((c + 2), 2)))
        {
            c += 2;
        }

        nRings = (c / 2);

        rings = new Rings[nRings];

        for (int i = 0; i < rings.Length; i++)
        {
            rings[i].module = new Rigidbody[(int)(Mathf.Pow(((i * 2) + 2), 2) - Mathf.Pow((i * 2), 2))];
            rings[i].renderers = new Renderer[rings[i].module.Length];

            for (int j = 0; j < Mathf.Pow(((i * 2) + 2), 2) - Mathf.Pow((i * 2), 2); j++)
            {
                if (i > 0)
                {
                    rings[i].module[j] = modules[j + (int)(Mathf.Pow((((i - 1) * 2) + 2), 2))];
                    rings[i].renderers[j] = rings[i].module[j].gameObject.GetComponent<Renderer>();
                }

                else
                {
                    rings[i].module[j] = modules[j];
                    rings[i].renderers[j] = rings[i].module[j].gameObject.GetComponent<Renderer>();
                }
            }
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].localPosition = colliders[i].forward * nRings * 4f;
        }

        for (int i = 0; i < modules.Length; i++)
        {
            modules[i].transform.position += new Vector3(0, Random.Range(0f, floorOffset), 0);
        }
    }

    private void SpawnProps(int _length)
    {
        Instantiate(propsModule[Random.Range(0, _length)], propsPos, Quaternion.identity);
    }

    private void MapDrop()
    {
        if (!canDrop)
        {
            if (nRings > 2)
            {
                StartCoroutine(Anticipation(nRings - 1));
            }
            canDrop = true;
        }
    }

    void Update()
    {
        if (anticipation)
        {

            time += Time.deltaTime;

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].localPosition = Vector3.MoveTowards(colliders[i].localPosition, colliders[i].forward * nRings * 4f, (anticipationTime/2) * Time.deltaTime);
            }

            for (int i = 0; i < rings[anticipationRing].module.Length; i++)
            {
				rings[anticipationRing].renderers[i].material.color= colorAnticipation.Evaluate(time);
			}

            if (time >= 1)
            {
                time = 0;
            }
        }
    }

    IEnumerator Anticipation(int ring)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(5f);

        yield return waitForSeconds;

        nRings -= 1;
        anticipationRing = ring;
        anticipation = true;

        yield return new WaitForSeconds(anticipationTime);

        StartCoroutine(Drop(ring));

        //if (nRings > 2)
        //{
        //    StartCoroutine(Anticipation(nRings - 1));
        //}
    }

    IEnumerator Drop(int ring)
    {
        for (int i = rings[ring].module.Length -1 ; i >= 0 ; i--)
        {
            rings[ring].module[i].useGravity = true;

            Collider[] props;
            props = rings[ring].module[i].gameObject.GetComponentsInChildren<Collider>();
            for (int j = 0; j < props.Length; j++)
            {
                props[j].enabled = false;
            }

            rings[ring].module[i].constraints = RigidbodyConstraints.None;

            StartCoroutine(Desactivate(rings[ring].module[i]));
            yield return new WaitForSeconds(dropInterval);
            if (i==0)
            {
                anticipation = false;
            }
        }
    }

    IEnumerator Desactivate (Rigidbody module)
    {
        yield return new WaitForSeconds(dropTime);
        module.useGravity = false;
        module.gameObject.SetActive(false);
    }
}

[System.Serializable]
public struct Rings
{
    public Rigidbody[] module;
    public Renderer[] renderers;
}

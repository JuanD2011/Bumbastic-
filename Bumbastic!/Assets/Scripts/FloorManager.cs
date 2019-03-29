using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class FloorManager : MonoBehaviour
{
	Rigidbody[] modules;

	Rings[] rings;

	[SerializeField]
	Transform[] colliders;

	int nRings = 0;
	int c = 0;
	int anticipationRing;

	bool anticipation = false;

	float time = 0;

	[SerializeField]
	float dropTime, dropInterval, anticipationTime;

	[SerializeField]
	Gradient colorAnticipation;

    bool canDrop = false;

    void Start()
    {
        modules = GetComponentsInChildren<Rigidbody>();

        while (modules.Length >= (Mathf.Pow((c + 2), 2)))
        {
            c += 2;
        }
        nRings = (c/ 2);

        rings = new Rings[nRings];

        for (int i = 0; i < rings.Length; i++)
        {
            rings[i].module = new Rigidbody[(int)(Mathf.Pow(((i * 2) + 2), 2) - Mathf.Pow((i * 2), 2))];

            for (int j = 0; j < Mathf.Pow(((i*2)+2),2)- Mathf.Pow((i*2),2); j++)
            {
                if (i>0)
                {
                    rings[i].module[j] = modules[j + (int)(Mathf.Pow((((i-1) * 2) + 2), 2))];
                }
                else
                {
                    rings[i].module[j] = modules[j];  
                }

            }
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].localPosition = colliders[i].forward*nRings;
        }

        for (int i = 0; i < modules.Length; i++)
        {
            modules[i].transform.position += new Vector3(0, Random.Range(0f,0.07f), 0);
        }

        GameManager.manager.Director.stopped += MapDrop;
    }

    private void MapDrop(PlayableDirector obj)
    {
        if (!canDrop)
        {
            if (nRings > 1)
            {
                StartCoroutine(Anticipation(nRings - 1));
                nRings -= 1;
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
                colliders[i].localPosition = Vector3.MoveTowards(colliders[i].localPosition, colliders[i].forward * nRings, Time.deltaTime/anticipationTime);
            }

            for (int i = 0; i < rings[anticipationRing].module.Length; i++)
            {
				rings[anticipationRing].module[i].GetComponent<Renderer>().material.shader = Shader.Find("HDRP/Lit");
				rings[anticipationRing].module[i].GetComponent<Renderer>().material.SetColor("_BaseColor", colorAnticipation.Evaluate(time));
			}

            if (time >= 1)
            {
                time = 0;
            }
        }
    }

    IEnumerator Anticipation(int ring)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(10f);

        yield return waitForSeconds;

        anticipationRing = ring;
        anticipation = true;

        yield return new WaitForSeconds(anticipationTime);

        StartCoroutine(Drop(ring));

        if (nRings > 1)
        {
            StartCoroutine(Anticipation(nRings - 1));
            nRings -= 1;
        }
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
}

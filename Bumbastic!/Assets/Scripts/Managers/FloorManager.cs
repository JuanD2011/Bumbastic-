using System.Collections;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField]
    Material plane = null;

    [SerializeField] GameObject[] propsModule = null, propsModuleWinter = null;
    
    [SerializeField]
	Transform[] colliders = new Transform[0];

	[SerializeField]
	float dropTime = 0f, dropInterval = 0f, anticipationTime = 0f;

	[SerializeField]
	Gradient colorAnticipation = null;
	
	[SerializeField]
	private float floorOffset = 0.07f;
	
	[SerializeField]
	Vector3 propsPos = new Vector3(0, 0.5f, 0);
	
    Rigidbody[] modules;
	Rings[] rings;

	int nRings = 0;
	int c = 0;
	int anticipationRing;
	float time = 0;

	bool anticipation = false;

    void Start()
    {
        SetEnviroment();

        modules = GetComponentsInChildren<Rigidbody>();

        while (modules.Length >= (Mathf.Pow((c + 2), 2)))
        {
            c += 2;
        }

        nRings = (c / 2);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].localPosition = colliders[i].forward * nRings * 4f;
        }

        if (GameModeDataBase.IsCurrentHotPotato())
        {
            InitRings();
            Bomb.OnExplode += MapDrop;
        }
    }

    private void SetEnviroment()
    {
        switch (GameManager.Manager.Enviroment) 
        {
            case EnumEnviroment.Desert:
                plane.SetFloat("_SnowThr", 0f);
                Instantiate(propsModule[Random.Range(0, propsModule.Length)], propsPos, Quaternion.identity);
                break;
            case EnumEnviroment.Winter:
                plane.SetFloat("_SnowThr", 1f);
                Instantiate(propsModuleWinter[Random.Range(0, propsModuleWinter.Length)], propsPos, Quaternion.identity);
                break;
            default:
                plane.SetFloat("_SnowThr", 0f);
                Instantiate(propsModule[Random.Range(0, propsModule.Length)], propsPos, Quaternion.identity);
                break;
        }
    }

    private void InitRings()
    {
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

        for (int i = 0; i < modules.Length; i++)
        {
            modules[i].transform.position += new Vector3(0, Random.Range(0f, floorOffset), 0);
        }
    }

    private void MapDrop()
    {
        if (nRings > 2)
        {
            StartCoroutine(Anticipation(nRings - 1));
        }
    }

    void Update()
    {
        if (GameModeDataBase.IsCurrentHotPotato() || GameModeDataBase.IsCurrentFreeForAll())
        {
            if (HotPotatoManager.HotPotato.Bomb.transform.parent == null)
            {
                plane.SetVector("_Target", HotPotatoManager.HotPotato.Bomb.transform.position);
            }
        }

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
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.anticipation, 0.7f);
                
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
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.anticipation, 0.7f);

        yield return new WaitForSeconds(anticipationTime);

        StartCoroutine(Drop(ring));
    }

    IEnumerator Drop(int ring)
    {
        CameraShake.instance.OnStartShake?.Invoke(1f, 0.5f);//Starts Camera shake.

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
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.dropModule, 0.6f);

            StartCoroutine(Desactivate(rings[ring].module[i]));
            yield return new WaitForSeconds(dropInterval);
            if (i==0)
            {
                anticipation = false;
            }
        }

        CameraShake.instance.OnStopShake?.Invoke();//Stops Camera shake
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

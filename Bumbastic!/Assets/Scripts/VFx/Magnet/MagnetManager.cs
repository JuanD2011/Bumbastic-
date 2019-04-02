using System.Collections;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{
    LineRenderer wave;

    Vector3[] wavePositions = new Vector3[2];

    [SerializeField] PathParticles pathParticles;
    [SerializeField] Color ringColor;

    [SerializeField] ParticleSystem ringParticles;
    ParticleSystem.MainModule ringMain;

    GameObject bomb;
    Transform posBomb;

    [SerializeField] float lerpDuration = 2f, ringsFadeOut = 1f;

    public delegate void DelMagnetManager();
    public DelMagnetManager OnLerpComplete;

    void Start()
    {
        wave = GetComponentInChildren<LineRenderer>();
        ringMain = ringParticles.main;
        wave.gameObject.SetActive(true);

        if (GameManager.manager.Bomb != null)
        {
            bomb = GameManager.manager.Bomb.gameObject;
            posBomb = GameManager.manager.Bomb.transform;
            StartCoroutine(LerpBomb());
        }
    }

    public IEnumerator LerpBomb()
    {
        wavePositions[0] = transform.position;
        wavePositions[1] = bomb.transform.position;
        wave.SetPositions(wavePositions);

        ringMain.startColor = ringColor;
        pathParticles.gameObject.SetActive(true);
        float distance = Mathf.Abs(Vector3.Distance(transform.position, bomb.transform.position));
        float t = 0;
        yield return new WaitWhile(() => pathParticles.activateParticles == false);
        wave.enabled = true;

        while (distance >= 2f)
        {
            distance = Mathf.Abs(Vector3.Distance(transform.position, bomb.transform.position));
            wavePositions[0] = transform.position;
            wavePositions[1] = bomb.transform.position;
            wave.SetPositions(wavePositions);

            bomb.transform.position = Vector3.Lerp(bomb.transform.position, transform.position, t/ lerpDuration);

            t += Time.deltaTime;
            yield return null;
        }

        pathParticles.gameObject.SetActive(false);
        wave.enabled = false;     
        t = 0;

        while(t <= ringsFadeOut)
        {
            ringMain.startColor = Color.Lerp(ringMain.startColor.colorMax, Color.clear, t / ringsFadeOut);
            bomb.transform.position = Vector3.Lerp(bomb.transform.position, transform.position, t / ringsFadeOut);
            t += Time.deltaTime;
            yield return null;
        }

        wave.gameObject.SetActive(false);
        OnLerpComplete?.Invoke();
    }
}

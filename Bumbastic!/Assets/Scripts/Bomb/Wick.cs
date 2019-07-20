using System.Collections;
using UnityEngine;

public class Wick : MonoBehaviour
{
    [SerializeField] Transform particles = null;
    [SerializeField] Transform[] points = new Transform[0];

    private Bomb bomb;

    private Renderer m_Renderer;

    private void Start()
    {
        bomb = GetComponentInParent<Bomb>();
        m_Renderer = GetComponent<Renderer>();

        ResetWick();

        Bomb.OnExplode += ResetWick;
    }

    private void ResetWick()
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(WickMovement());
    }

    IEnumerator WickMovement()
    {
        int currentPoint = 0;
        float timePerPoint;
        float elapsedTime = 0f;
        particles.position = points[0].position;
        m_Renderer.material.SetFloat("_Factor", 0f);
        yield return new WaitUntil(() => bomb.Timer != 0f);
        yield return new WaitUntil(() => !bomb.Exploded);
        timePerPoint = bomb.Timer / (points.Length - 1);

        while (!bomb.Exploded)
        {
            if (bomb.CanCount)
            {
                float distance = Vector3.Distance(points[currentPoint].position, particles.position);
                particles.position = Vector3.MoveTowards(particles.position, points[currentPoint].position, (Time.deltaTime * distance) / timePerPoint);
                m_Renderer.material.SetFloat("_Factor", elapsedTime / bomb.Timer);

                if (distance < 0.1f && currentPoint < points.Length - 1)
                {
                    currentPoint++;
                }
                elapsedTime += Time.deltaTime; 
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.red;
            Vector3 position = points[i].position;
            Gizmos.DrawSphere(position, 0.05f);

            if (i > 0)
            {
                Vector3 previous = points[i - 1].position;
                Gizmos.DrawLine(previous, position);
            }
        }
    }
}

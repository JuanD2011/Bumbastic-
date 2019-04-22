using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Wagon : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float velocity = 15f;
    [SerializeField] float timeToRestart = 7f, timeToStart = 5f;

    Rigidbody m_Rigidbody;
    Vector3 initPos; 

    private void Start()
    {
        initPos = transform.position;
        m_Rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(InitWagon());
    }

    private IEnumerator InitWagon()
    {
        WaitForSeconds waitToStart = new WaitForSeconds(timeToStart);
        WaitForSeconds waitToRestart = new WaitForSeconds(timeToRestart);

        yield return waitToStart;

        if (m_Rigidbody.velocity == Vector3.zero)
        {
            m_Rigidbody.AddForce(transform.forward * velocity, ForceMode.Impulse); 
        }

        yield return waitToRestart;
        transform.position = initPos;
        StartCoroutine(InitWagon());
    }

    private void OnTriggerEnter(Collider other)
    {
        Player playerCollisioned = other.GetComponent<Player>();

        if (playerCollisioned != null)
        {
            playerCollisioned.Rigidbody.AddForce(Vector3.Cross(transform.forward, playerCollisioned.transform.up) * pushForce, ForceMode.Impulse);
        }
    }
}

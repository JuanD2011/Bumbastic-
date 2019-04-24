using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Wagon : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float velocity = 15f;
    [SerializeField] float timeToRestart = 7f, timeToStart = 5f;
    [SerializeField] float timeToLerpPosition = 0.3f;

    Rigidbody m_Rigidbody;

    Collider[] colliders;

    private void Start()
    {
        if (GameModeDataBase.IsCurrentFreeForAll())
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            colliders = GetComponentsInChildren<Collider>();

            foreach (Collider collider in colliders)
            {
                collider.gameObject.transform.parent = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player playerCollisioned = other.GetComponentInParent<Player>();

        if (playerCollisioned != null)
        {
            if (m_Rigidbody.velocity != Vector3.zero)
            {
                if (playerCollisioned.transform.position.z > transform.position.z)
                {
                    playerCollisioned.Rigidbody.AddForce(Quaternion.AngleAxis(60, Vector3.right) * Vector3.forward * pushForce, ForceMode.Impulse);
                }
                else
                {
                    playerCollisioned.Rigidbody.AddForce(Quaternion.AngleAxis(60, Vector3.right) * -Vector3.forward * pushForce, ForceMode.Impulse);
                }
            }
        }

        if (GameModeDataBase.IsCurrentFreeForAll())
        {
            if (other.tag == "Wagon")
            {
                StartCoroutine(LerpPosition(timeToLerpPosition, transform.position, other.transform.position));
                transform.rotation = other.transform.rotation;
                m_Rigidbody.velocity = Vector3.zero;
            }
        }
    }

    IEnumerator LerpPosition(float _timeToLerp, Vector3 _lastPosition, Vector3 _target)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _timeToLerp)
        {
            transform.position = Vector3.Lerp(_lastPosition, _target, elapsedTime / _timeToLerp);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(InitWagon());
    }

    private IEnumerator InitWagon()
    {
        WaitForSeconds waitToStart = new WaitForSeconds(timeToStart);

        yield return waitToStart;

        if (m_Rigidbody.velocity == Vector3.zero)
        {
            m_Rigidbody.AddForce(transform.forward * velocity, ForceMode.Impulse);
        }
    }
}

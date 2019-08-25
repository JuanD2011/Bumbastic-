using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Wagon : MonoBehaviour, IBounce
{
    [SerializeField] float pushForce = 2f;
    [SerializeField] float velocity = 15f;
    [SerializeField] float timeToStart = 5f;
    [SerializeField] float timeToLerpPosition = 0.3f;
    [SerializeField] float maxVelocity = 10f;

    [SerializeField] ParticleSystem crashParticleSystem = null;

    bool clampVelocity = false;
    float sqrMaxVelocity = 0f;

    bool canStun = false;

    Rigidbody m_Rigidbody;

    Collider[] colliders;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        if (GameModeDataBase.IsCurrentFreeForAll())
        {
            colliders = GetComponentsInChildren<Collider>();

            foreach (Collider collider in colliders)
            {
                collider.gameObject.transform.parent = null;
            }
        }
    }

    private void Start()
    {
        sqrMaxVelocity = SqrMaxVelocity();
    }

    private void FixedUpdate()
    {
        if (clampVelocity)
        {
            Vector3 rbVel = m_Rigidbody.velocity;

            if (rbVel.sqrMagnitude > sqrMaxVelocity || rbVel.sqrMagnitude <= sqrMaxVelocity * 0.9f)
            {
                m_Rigidbody.velocity = rbVel.normalized * maxVelocity; 
            }
        }   
    }

    private float SqrMaxVelocity() { return maxVelocity * maxVelocity; }

    private void OnTriggerEnter(Collider other)
    {
        if (GameModeDataBase.IsCurrentFreeForAll())
        {
            if (other.tag == "Wagon")
            {
                clampVelocity = false;
                canStun = false;
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.rollingWagon, 0.7f, false);
                transform.rotation = other.transform.rotation;
                m_Rigidbody.velocity = Vector3.zero;
                StartCoroutine(LerpPosition(timeToLerpPosition, transform.position, other.transform.position));
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
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(timeToStart);
        m_Rigidbody.constraints = RigidbodyConstraints.None;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        
        if (m_Rigidbody.velocity == Vector3.zero)
        {
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.rollingWagon, 1f, true);
            m_Rigidbody.AddForce(transform.forward * velocity, ForceMode.Impulse);
            clampVelocity = true;
            canStun = true;
        }
    }

    public void Bounce(GameObject _bounceable, Collision _collision)
    {
        if (!GameModeDataBase.IsCurrentFreeForAll() || _collision == null) return;

        Player player = _bounceable.GetComponentInParent<Player>();

        if (player != null && player.CanMove)
        {
            if (canStun)
            {
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.wagonHit, 1f);
                StartCoroutine(player.Stun(true, 2.2f));
                player.Rigidbody.AddForce(Quaternion.AngleAxis(60, Vector3.right) * -Vector3.forward * pushForce, ForceMode.Impulse);
                ContactPoint contactPoint = _collision.GetContact(0);
                crashParticleSystem.transform.position = contactPoint.point;
                crashParticleSystem.Play();
            }
        }
    }
}

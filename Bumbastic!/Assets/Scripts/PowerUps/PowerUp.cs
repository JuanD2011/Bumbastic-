using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    private float duration = 0f;
    protected Player m_player = null;
    [SerializeField] Animator p_Animator = null;
    private Animator m_Animator = null;

    [SerializeField] GameObject speedUp = null, magnet = null, shield = null;

    [SerializeField] ParticleSystem openBoxParticleSystem = null;

    public float Duration { get => duration; protected set => duration = value; }
    public BoxCollider P_Collider { get; set; }
    public Transform Box { get; set; } = null;

    private void Awake()
    {
        P_Collider = GetComponentInParent<BoxCollider>();
        m_Animator = GetComponent<Animator>();

        Box = transform.parent;
    }

    private void Start()
    {
        Invoke("Dropped", 2f);
    }

    public void Dropped()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBoxDropped, 0.6f);
        Box.eulerAngles = Vector3.zero;
        Box.SetParent(null);

        p_Animator.SetTrigger("Open");
        openBoxParticleSystem.Play();
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBoxOpened, 0.5f);
        Invoke("AfterBoxOpens", 0.5f);
    }

    public void AfterBoxOpens()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBubble, 1f);
        P_Collider.enabled = false;
        m_Animator.SetTrigger("GetBigger");
    }

    protected void GetPlayer()
    {
        m_player = GetComponentInParent<Player>();
    }

    public virtual void PickPowerUp(Player _player)
    {
        int randomPU = Random.Range(0, 3);

        if (_player.HasBomb)
        {
            Instantiate(speedUp, _player.transform.position, Quaternion.identity, _player.transform);
            Box.gameObject.SetActive(false);
            return;
        }

        switch (randomPU)
        {
            case 0:
                Instantiate(speedUp, _player.transform.position, Quaternion.identity, _player.transform);
                break;
            case 1:
                Instantiate(magnet, _player.transform.position, Quaternion.identity, _player.transform);
                break;
            case 2:
                Instantiate(shield, _player.transform.position + new Vector3(0f, 1.51f, 0f), Quaternion.identity, _player.transform);
                break;
            default:
                break;
        }
        Box.gameObject.SetActive(false);
    }
}

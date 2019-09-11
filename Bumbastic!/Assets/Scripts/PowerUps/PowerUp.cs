using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    [SerializeField] Animator p_Animator = null;
    [SerializeField] Explosion p_Explosion = null;

    private float duration = 0f;
    protected ThrowerPlayer m_player = null;

    private Animator m_Animator = null;

    [SerializeField] GameObject speedUp = null, magnet = null, shield = null, tangle = null;

    [SerializeField] ParticleSystem openBoxParticleSystem = null;

    public float Duration { get => duration; protected set => duration = value; }
    public Transform Box { get; set; } = null;

    private void Awake()
    {
        p_Explosion = GetComponentInParent<Explosion>();
        m_Animator = GetComponent<Animator>();

        Box = transform.parent;
    }

    private void Start()
    {
        Invoke("InitFirstTime", 4f);
        p_Explosion.OnBoxExplode += OnBoxExplode;
    }

    private void InitFirstTime()
    {
        p_Animator.SetTrigger("Open");
    }

    public void Dropped()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBoxDropped, 0.6f);
        Box.eulerAngles = Vector3.zero;
        Box.SetParent(null);
        p_Explosion.Explode();
    }

    private void OnBoxExplode()
    {
        openBoxParticleSystem.Play();
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBoxOpened, 0.5f);
        Invoke("AfterBoxOpens", 0.5f);
    }

    public void AfterBoxOpens()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBubble, 1f);
        m_Animator.SetTrigger("GetBigger");
        p_Animator.SetTrigger("Dissipate");
    }

    protected void GetPlayer()
    {
        m_player = GetComponentInParent<ThrowerPlayer>();
    }

    public virtual void PickPowerUp(ThrowerPlayer _player)
    {
        int randomPU = Random.Range(0, 4);

        if (_player.HasBomb)
        {
            TanglePlayers(_player as Player);
            //Instantiate(speedUp, _player.transform.position, Quaternion.identity, _player.transform);
            //Box.gameObject.SetActive(false);
            //return;
        }

        switch (randomPU)
        {
            case 0:
            case 1:
            case 2:
            //case 0:
            //    Instantiate(speedUp, _player.transform.position, Quaternion.identity, _player.transform);
            //    break;
            //case 1:
            //    Instantiate(magnet, _player.transform.position, Quaternion.identity, _player.transform);
            //    break;
            //case 2:
            //    Instantiate(shield, _player.transform.position + new Vector3(0f, 1.51f, 0f), Quaternion.identity, _player.transform);
            //    break;
            case 3:
                TanglePlayers(_player as Player);
                break;
            default:
                break;
        }
        Box.gameObject.SetActive(false);
    }

    private void TanglePlayers(Player _player)
    {
        foreach (Player player in GameManager.Manager.Players)
        {
            if (player != _player)
            {
                Instantiate(tangle, _player.transform.position, Quaternion.identity, player.transform);
            }
        }
    }
}

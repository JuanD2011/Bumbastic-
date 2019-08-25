using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour, IPowerUp
{
    private float duration = 0f;
    protected Player m_player = null;
    private Animator p_Animator = null;

    [SerializeField] GameObject speedUp, magnet, shield;

    ParticleSystem openBoxParticleSystem = null;

    public float Duration { get => duration; protected set => duration = value; }
    public Collider P_Collider { get; set; }
    public Transform Box { get; set; } = null;

    private void Awake()
    {
        P_Collider = GetComponentInParent<Collider>();
        p_Animator = GetComponentInParent<Animator>();
        openBoxParticleSystem = GetComponentInChildren<ParticleSystem>();

        Box = transform.parent;
    }

    public void Dropped()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBoxDropped, 0.6f);
        Box.eulerAngles = Vector3.zero;
        Box.SetParent(null);

        p_Animator.SetTrigger("Open");
        StartCoroutine(SyncOpenBox());
    }

    IEnumerator SyncOpenBox()
    {
        yield return new WaitUntil(() => p_Animator.GetCurrentAnimatorStateInfo(0).IsName("Opened"));
        AnimatorStateInfo animatorStateInfo = p_Animator.GetCurrentAnimatorStateInfo(0);

        openBoxParticleSystem.Play();
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBoxOpened, 1f);

        yield return new WaitUntil(() => animatorStateInfo.normalizedTime >= 0.9f);
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBubble, 1f);
        P_Collider.enabled = false;
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
                Instantiate(magnet, _player.transform.position, Quaternion.identity, _player.transform);
                break;
            case 1:
                Instantiate(magnet, _player.transform.position, Quaternion.identity, _player.transform);
                break;
            case 2:
                Instantiate(magnet, _player.transform.position, Quaternion.identity, _player.transform);
                //Instantiate(shield, _player.transform.position + new Vector3(0f, 1.51f, 0f), Quaternion.identity, _player.transform);
                break;
            default:
                break;
        }
        Box.gameObject.SetActive(false);
    }
}

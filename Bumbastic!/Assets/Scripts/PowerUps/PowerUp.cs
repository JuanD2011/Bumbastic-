using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour, IPowerUp
{
    private float duration = 0f;
    protected Player m_player = null;
    private Animator p_Animator = null;


    public float Duration { get => duration; protected set => duration = value; }
    public Collider P_Collider { get; set; }
    public Transform Box { get; set; } = null;

    private void Awake()
    {
        P_Collider = GetComponentInParent<Collider>();
        p_Animator = GetComponentInParent<Animator>();
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

        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.PowerUpBoxOpened, 1f);
        yield return new WaitUntil(() => animatorStateInfo.normalizedTime >= 0.9f);
        P_Collider.enabled = false;
    }

    protected void GetPlayer()
    {
        m_player = GetComponent<Player>();
    }

    public virtual void PickPowerUp(Player _player)
    {
        int randomPU = Random.Range(0, 3);

        if (_player.HasBomb)
        {
            _player.gameObject.AddComponent<Velocity>();
            Box.gameObject.SetActive(false);
            return;
        }

        switch (randomPU)
        {
            case 0:
                _player.gameObject.AddComponent<Velocity>();
                break;
            case 1:
                _player.gameObject.AddComponent<Magnet>();
                break;
            case 2:
                _player.gameObject.AddComponent<Magnet>();
                break;
            case 3:
                break;
            default:
                break;
        }
        Box.gameObject.SetActive(false);
    }
}

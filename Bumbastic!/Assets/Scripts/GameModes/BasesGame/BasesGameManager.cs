using System.Collections.Generic;
using UnityEngine;

public class BasesGameManager : GameManager
{
    public static BasesGameManager basesGame;

    public Base[] bases;

    protected override void Awake()
    {
        if (basesGame == null) basesGame = this;
        else Destroy(this);

        base.Awake();

        SetBasesColor();
    }

    protected override void SpawnPlayers()
    {
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            Player player = Instantiate(PlayerPrefab).GetComponent<Player>();
            Players.Add(player);
            player.Avatar = InGame.playerSettings[i].avatar;
            player.PrefabName = InGame.playerSettings[i].name;
            player.Id = (byte)i;
            player.SpawnPoint = SpawnPoints[i].transform.position;
            player.transform.position = player.SpawnPoint;
            player.Initialize();
        }
    }

    private void SetBasesColor()
    {
        //TODO implement color to the "Hair" of the base.
    }

    public override void PassBomb(Player _receiver)
    {
        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;

        foreach (Renderer renderer in _receiver.AvatarSkinnedMeshRenderers)
        {
            renderer.material.shader = bombHolderShader;
        }

        //Bomb.RigidBody.velocity = Vector2.zero;
        //Bomb.RigidBody.isKinematic = true;
        //Bomb.Collider.enabled = false;
        //Bomb.transform.position = _receiver.Catapult.position;
        //Bomb.transform.SetParent(_receiver.Catapult.transform);
        StartCoroutine(_receiver.Rumble(0.2f, 0.2f, 0.2f));

        float probTosound = Random.Range(0f, 1f);

        if (probTosound < 0.33f)
        {
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cTransmitter, 1f);
        }
    }

    public override void PassBomb(Player _receiver, Player _transmitter)
    {
    }

    protected override void GiveBombs()
    {
        return;
    }
}

using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class TargetGroup : MonoBehaviour
{
    CinemachineTargetGroup cinemachineTargetGroup;

    private void Start()
    {
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();

        if (GameManager.Manager.Players.Count != 0)
        {
            cinemachineTargetGroup.m_Targets = new CinemachineTargetGroup.Target[GameManager.Manager.Players.Count];

            for (int i = 0; i < GameManager.Manager.Players.Count; i++)
            {
                cinemachineTargetGroup.m_Targets[i].target = GameManager.Manager.Players[i].transform;
            }
        }
        else
        {
            Debug.LogError("No targets found");
            return;
        } 
    }
}
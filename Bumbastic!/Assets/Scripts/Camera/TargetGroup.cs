using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class TargetGroup : MonoBehaviour
{
    CinemachineTargetGroup cinemachineTargetGroup;

    private void Start()
    {
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();

        if (GameManager.manager.Players.Count != 0)
        {
            cinemachineTargetGroup.m_Targets = new CinemachineTargetGroup.Target[GameManager.manager.Players.Count];

            for (int i = 0; i < GameManager.manager.Players.Count; i++)
            {
                cinemachineTargetGroup.m_Targets[i].target = GameManager.manager.Players[i].transform;
            }
        }
        else
        {
            Debug.LogError("No targets found");
            return;
        } 
    }
}
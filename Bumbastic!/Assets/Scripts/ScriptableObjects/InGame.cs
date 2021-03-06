﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InGame",menuName = "InGame")]
public class InGame : ScriptableObject
{
    public const byte maxScore = 3;
    public Transform posDropPU;

    public static List<PlayerSettings> playerSettings = new List<PlayerSettings>();

    Vector3 crowPos;

    public Vector3 CrowPos { get => crowPos; set => crowPos = value; }
}

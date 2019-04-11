﻿using System.Collections;
using UnityEngine;

public class Shield : PowerUp
{
    protected override void Start()
    {
        base.Start();
        Duration = 5f;
        StartCoroutine(Execute());
    }

    IEnumerator Execute()
    {
        WaitForSeconds wait = new WaitForSeconds(Duration);
        yield return wait;
        Destroy(this);
    }
}

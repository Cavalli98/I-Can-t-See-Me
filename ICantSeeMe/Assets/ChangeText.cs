using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeText : Triggerable
{
    public bool oneTime = true;
    private bool _stop = false;

    public override void activate()
    {
        if (_stop) return;
        if (oneTime) _stop = true;

        foreach(Transform child in transform)
            child.gameObject.SetActive(!child.gameObject.activeSelf);
    }
}
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : Trigger
{
    public int hours;
    public int minutes;
    public GameObject LockPanel;
    private int code;
    
    private void Awake()
    {
        code = int.Parse(hours.ToString() + minutes.ToString());
    }
}

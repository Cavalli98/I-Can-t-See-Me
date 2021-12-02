using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public static Ghost instance;
    // Player to follow
    public Transform _localPlayer = null;
    private Transform _tr;

    private void Awake()
    {
        instance = this;
        _tr = GetComponent<Transform>();
    }
}

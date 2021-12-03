using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public static Ghost instance;

    private void Awake()
    {
        instance = this;
    }
}

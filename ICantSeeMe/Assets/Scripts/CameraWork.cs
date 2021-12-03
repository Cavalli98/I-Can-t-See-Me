using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    private Transform _camtr;

    private void Awake()
    {
        _camtr = Camera.main.transform;
        if (PhotonNetwork.IsMasterClient)
        {
            _camtr.position = _camtr.position + new Vector3(26.6f, 0f, 0f);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

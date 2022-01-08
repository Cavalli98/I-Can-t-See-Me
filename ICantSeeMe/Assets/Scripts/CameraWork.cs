using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    private Transform _camtr;
    public static CameraWork instance;
    public Vector2 player1;
    public Vector2 player2;

    private void Awake()
    {
        instance = this;
        _camtr = Camera.main.transform;
        if (PhotonNetwork.IsMasterClient)
        {
            _camtr.position = new Vector3(player1.x, player1.y, _camtr.position.z);
        }
        else
        {
            _camtr.position = new Vector3(player2.x, player2.y, _camtr.position.z);
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

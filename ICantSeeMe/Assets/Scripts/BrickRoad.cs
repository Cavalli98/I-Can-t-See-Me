using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BrickRoad : Triggerable
{
    public bool isUp;
    public float startY;
    public float endY;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    [PunRPC]
    public override void activate()
    {
        if (isUp)
        {
            while (transform.position.y > endY)
            {
                transform.Translate(0f, -0.0005f, 0f);
            }
            isUp = false;
        }
        else if (!isUp)
        {
            while (transform.position.y < endY)
            {
                transform.Translate(0f, 0.0005f, 0f);
            }
            isUp = true;
        }
    }
}

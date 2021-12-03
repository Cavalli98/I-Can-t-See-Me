using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BrickRoad : Triggerable
{
    public bool isUp;
    public float startY;
    public float endY;
    private float _diff;

    // Start is called before the first frame update
    void Start()
    {
        _diff = Mathf.Abs(startY - endY);
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
            transform.Translate(0f, -_diff * Time.deltaTime, 0f);
            isUp = false;
        }
        else if (!isUp)
        {
            transform.Translate(0f, _diff * Time.deltaTime, 0f);
            isUp = true;
        }
    }
}

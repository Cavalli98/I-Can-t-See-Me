using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingStack : MonoBehaviour
{
    private float startingX = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        startingX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * (transform.position.x - startingX)*(-100));
        print(transform.rotation.z);
    }
}

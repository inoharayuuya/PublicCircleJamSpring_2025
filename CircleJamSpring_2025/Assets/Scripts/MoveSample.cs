using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSample : MonoBehaviour
{
    //ˆÚ“®‘¬“x
    Vector3 speed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            speed.y = 1f;
        } 
        else if (Input.GetKey(KeyCode.S))
        {
            speed.y = -1f;
        }
        else { speed.y = 0; }

        if (Input.GetKey(KeyCode.D))
        {
            speed.x = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            speed.x = -1f;
        }
        else { speed.x = 0; }

        transform.position += speed * Time.deltaTime;
    }
}

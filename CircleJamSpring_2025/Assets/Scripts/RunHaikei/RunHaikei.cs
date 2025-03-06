using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RunHaikei : MonoBehaviour
{
    public float speed = 3;
    public float distance;
    public float limu = 3;
    public float time;
    public float acceleration = 3;
    public bool hit_check;
    public int delay = 1000;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        MoveHaikei();
    }

    void Init()
    {
        distance = 0;
        time = 0;
        hit_check = false;
    }

    async void MoveHaikei()
    {
        if (hit_check == true)
        {
            await Task.Delay(delay);
            hit_check = false;
        }
        else
        {
            var pos = new Vector3(-speed * Time.deltaTime, 0, 0);
            transform.position += pos;
            distance -= pos.x;
            time += Time.deltaTime;
            if (time >= limu)
            {
                speed += acceleration;
                time = 0;
            }
            if (transform.position.x < -17.7)
            {
                transform.position = new Vector3(17.7f, 0, 0);
            }
        }
    }
}
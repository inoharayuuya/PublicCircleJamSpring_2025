using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStageMoob : MonoBehaviour
{
    public RunHaikei runHaikei;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        runHaikei = GameObject.Find("Square (1)").GetComponent<RunHaikei>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = runHaikei.speed;
        var pos = new Vector3(-speed * Time.deltaTime, 0, 0);
        transform.position += pos;
        if(transform.position.x <= -15)
        {
            Destroy(this.gameObject);
        }
    }
}

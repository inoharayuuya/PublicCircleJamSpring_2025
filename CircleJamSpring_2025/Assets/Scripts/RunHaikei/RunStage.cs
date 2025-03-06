using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunStage : MonoBehaviour
{
    public GameObject[] prefabs;
    public int num;
    // Start is called before the first frame update
    void Start()
    {
        RandomPrefabs();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void RandomPrefabs()
    {
        num = Random.Range(0, prefabs.Length-1);
        Instantiate(prefabs[num], prefabs[num].transform.position + new Vector3(17, 0, 0), Quaternion.identity);
    }
}

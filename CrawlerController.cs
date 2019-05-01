using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerController : MonoBehaviour {

    private int x = 0;

	// Use this for initialization
	void Start () {
        x = Random.Range(-13, 13);
        transform.position = new Vector3(x, -12, 5);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

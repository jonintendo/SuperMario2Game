using UnityEngine;
using System.Collections;

public class Cogumelo : MonoBehaviour {

    int direction = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.position += direction*this.transform.forward / 70;
	}


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        direction =  -1 * direction;
    }

}

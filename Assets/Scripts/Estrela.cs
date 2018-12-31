using UnityEngine;
using System.Collections;

public class Estrela : MonoBehaviour {

	// Use this for initialization

    
	void Start () {
        GetComponent<Rigidbody>().AddForce(new Vector3(-1,1,0) * 400);	
	}
	
	// Update is called once per frame
	void Update () {
      //  this.transform.position +=  this.transform.forward / 70;
	}

    void OnCollisionEnter(Collision collision)
    {

        if (!collision.transform.tag.Contains("layer"))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * 400);	

        }
    }
}

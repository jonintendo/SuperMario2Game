 
using UnityEngine;
using System.Collections;

public class PesOnLine : MonoBehaviour {


    public GameObject myPlayer;

	void Start () {
        myPlayer = this.gameObject.transform.parent.gameObject;
	}
	
	
	void Update () {
	
	}


    void OnTriggerEnter(Collider collision)
    {
        if (!collision.tag.Equals("enemy"))
        {
            Debug.Log(collision.gameObject.name);
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        //if (collision.gameObject.tag == "chao")
        //{

        //  PlayerAction.nochao = true;
        //    // Debug.Log("colidiu com o chao");

        //}


        //Debug.Log(collision.gameObject.name);
        //if (!collision.transform.tag.Equals("Bullet"))
        //    Destroy(collision.gameObject);
        if (collision.gameObject.tag.Equals("enemy"))
        {

            PlayerActionOnLine playeraction = myPlayer.GetComponent<PlayerActionOnLine>();
            playeraction.SetPlacar(100,0,null);
            Debug.Log("colidiu com inmigo");
        }

    }

}

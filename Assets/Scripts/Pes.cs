using UnityEngine;
using System.Collections;

public class Pes : MonoBehaviour {


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

            PlayerAction playeraction = myPlayer.GetComponent<PlayerAction>();
            playeraction.coins += 100;
            Debug.Log("colidiu com inmigo");
        }

    }

}

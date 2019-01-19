using UnityEngine;
using System.Collections;

public class Enemys : MonoBehaviour
{


    public AudioClip smashenemy;
    Quaternion rot;
    Vector3 behind;


    float timer = 0;

    void Start()
    {

        behind = -transform.forward;
    }


    void Update()
    {
        timer += Time.deltaTime;

      
        float damping = 0.5f;

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);

        if (timer > 50)
        {
            timer = 0;
            behind = -1 * behind;

        }
        rot = Quaternion.LookRotation(behind);

        transform.position += this.transform.forward / 200;


    }

    void OnTriggerEnter(Collider collision)
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);

        switch (collision.transform.tag)
        {
            case "Pes":
                GetComponent<AudioSource>().PlayOneShot(smashenemy);
               // gameObject.GetComponent<Collider>().enabled = false;
                Destroy(gameObject);
                break;
            case "Fireball":
                GetComponent<AudioSource>().PlayOneShot(smashenemy);
                Destroy(gameObject, 0.50f);
                break;
            case "estrela":
              
                Debug.Log("matou com estrela");
                GetComponent<AudioSource>().PlayOneShot(smashenemy);
                Destroy(gameObject);
                break;
            default:
                behind = -1 * behind;
                break;

        }


    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.tag);
    }




}

using UnityEngine;
using System.Collections;

public class testemovemario : MonoBehaviour {

    //Animator anim;
    //int jumpHash = Animator.StringToHash("ArmatureAction.003");
   // int runStateHash = Animator.StringToHash("Base Layer.Run");



    public Texture marioFogo;
    public Texture marioMimic;
    public Texture mario;
    public Texture marioLuigi;
    public Texture marioWario;

    public Material mat;



    public Animation ff;
    public CharacterController tt;
    float speedV;
    float speedH;



    bool FireBall = false;

    void Start()
    {


        ff = GetComponentsInChildren<Animation>()[0];
         tt = GetComponentInParent<CharacterController>();
        
    }


    void Update()
    {
        if (FireBall)
        {
            ff.Play("Armature|MyFireBall", PlayMode.StopAll);
            FireBall = false;
        }


        var VV=new Vector3( tt.velocity.x,0,tt.velocity.z);
        speedV = VV.magnitude;

        var VH = new Vector3(0,tt.velocity.y, 0);
        speedH = VH.magnitude;




        if (speedH > 0.5f && !ff.IsPlaying("Armature|MyFireBall"))
        {

            ff.Play("Armature|MyJump", PlayMode.StopAll);
        }else

            if (speedV > 0.5f && !ff.IsPlaying("Armature|MyFireBall"))
        {
            ff.Play("Armature|MyWalk", PlayMode.StopAll);
        }


       

        if (Input.GetKeyDown(KeyCode.Space) )
        {
           // ff.Play("Armature|MyWalk", PlayMode.StopAll);
           //ff.Play("Armature|MyJump",PlayMode.StopAll);
           // ff.Play("Armature|MyFireBall", PlayMode.StopAll);

           mat.mainTexture = marioLuigi;

            Debug.Log(ff.name);
            //anim.SetTrigger(jumpHash);
        }

        if (Input.GetMouseButton(0) )
        { //Tiro
            FireBall = true;
        }


        Debug.Log(speedV+"  "+speedH);
    }
}

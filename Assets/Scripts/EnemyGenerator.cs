using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;

    // MenuRede menu;

    float timer;
    float timeEnemyAppear;
  
    void Start()
    {
        timer = 0.0f;
        timeEnemyAppear = 10.0f;
        //menu =  GetComponentInParent<MenuRede>();
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if (Network.isServer )
        {
            if (timer > timeEnemyAppear)
            {
                NewEnemies();
                timer = 0;
            }
        }
    }

    public void NewEnemies()
    {
        var entradasInimigos = GameObject.FindGameObjectsWithTag("EntradaFaseInimigo");

        foreach (var entradaInimigos in entradasInimigos)
        {

            Instantiate(Enemy1, entradaInimigos.transform.position, Quaternion.identity);
        }

    }

}

using UnityEngine;
using System.Collections;

public class PlacarOnLine : MonoBehaviour
{

    GUIText coinst;
    GUIText lifet;

    void Awake()
    {
        coinst = GetComponentsInChildren<GUIText>()[0];

        lifet = GetComponentsInChildren<GUIText>()[1];
    }

    void Update()
    {
        //Debug.Log(coinst.text + ")" + lifet.text);

    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {

        
        Vector3 syncPosition = Vector3.zero;
        int syncCoin = 0;
        int syncLife = 0;

        if (stream.isWriting)
        {
            syncPosition = transform.position;
            syncCoin = int.Parse(coinst.text);
            syncLife = int.Parse(lifet.text);

            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncCoin);
            stream.Serialize(ref syncLife);

        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncCoin);
            stream.Serialize(ref syncLife);

            transform.position = syncPosition;
            coinst.text = syncCoin.ToString();
            lifet.text = syncLife.ToString();

        }
    }




}

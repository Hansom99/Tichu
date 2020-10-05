using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Tichu;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    public static GameObject myPlayer;

    public Transform[] spawnPoint;

    public int myNumber;

    public PhotonView PV;

    public Card[] allCards = new Card[56];


    // Start is called before the first frame update
    void Start()
    {
        getNumber();
        Debug.Log(myNumber);

        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate


            myPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint[myNumber].position, spawnPoint[myNumber].rotation, 0);
            myPlayer.GetComponent<PlayerManager>().myNumber = myNumber;
        }
        if (PhotonNetwork.IsMasterClient) 
        {
            int[] cardValue = new int[56];
            
            for (int i = 0; i < allCards.Length; i++)
            {
                allCards[i] = new TichuCard(i + 2);
            }
            RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
            allCards = allCards.OrderBy(x => GetNextInt32(rnd)).ToArray();
            for (int i = 0; i < allCards.Length; i++) cardValue[i] = allCards[i].value;
            

            photonView.RPC("SetCards", RpcTarget.All, cardValue);
        }



    }


    [PunRPC]
    void SetCards(int[] cards)
    {
        Debug.Log(cards.Length);


        for (int i = 0; i < allCards.Length; i++)
        {
            allCards[i] = new TichuCard(cards[i]);
        }
        Card[] tmp = new Card[14];
        Array.Copy(allCards, myNumber*14,tmp ,0, 14);

        myPlayer.GetComponent<PlayerManager>().myCards = new List<Card>(tmp);

    }


    static int GetNextInt32(RNGCryptoServiceProvider rnd)
    {
        byte[] randomInt = new byte[4];
        rnd.GetBytes(randomInt);
        return Convert.ToInt32(randomInt[0]);
    }

    void getNumber()
    {
        for(int i = 0;i< PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].IsLocal)
            {
                myNumber = i;
                break;
            }
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

using Photon.Pun;
using Photon.Realtime;
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

    public Transform table;

    public int myNumber;

    public PhotonView PV;

    public Card[] allCards = new Card[56];

    public PlayerManager myPlayerManager;

    public Player currentPlayer;

    public Player belongsTo;


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
            myPlayerManager = myPlayer.GetComponent<PlayerManager>();
        }
        if (PhotonNetwork.IsMasterClient) 
        {
            int[] cardValue = new int[56];
            
            for (int i = 0; i < allCards.Length; i++)
            {
                allCards[i] = new TichuCard(i);
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
        //Card[] tmp = new Card[14];
        //Array.Copy(allCards, myNumber*14,tmp ,0, 14);

        //myPlayer.GetComponent<PlayerManager>().myNumber = myNumber;
        //myPlayer.GetComponent<PlayerManager>().myCards = new List<Card>(tmp);
        

        Debug.Log("Cards Set:" + allCards.Length);

        photonView.RPC("sncCards", RpcTarget.All, myPlayer.GetPhotonView().ViewID, myNumber);

    }

    [PunRPC]
    void sncCards(int id, int playerNumber)
    {
        //while (allCards.Length == 0) ;

        //this.myNumber = myNumber;
        Card[] tmp = new Card[14];
        Array.Copy(allCards, playerNumber * 14, tmp, 0, 14);
        foreach(Card c in tmp)
        {
            if(c.value == 53 && PhotonNetwork.IsMasterClient)
            {
                setCurrentPlayer(0,0);
                break;
            }
        }
        GameObject player = PhotonView.Find(id).gameObject;
        player.GetComponent<PlayerManager>().myNumber = myNumber;
        player.GetComponent<PlayerManager>().gameManager = this;
        player.GetComponent<PlayerManager>().myCards = new List<Card>(tmp);


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

    public void setCurrentPlayer(int number,int belongs)
    {
        photonView.RPC("SetCurrentPlayer", RpcTarget.All, number,belongs);

        
    }

    [PunRPC]
    void SetCurrentPlayer(int number,int belongs)
    {
        Debug.Log(number + " " + belongs);
        currentPlayer = PhotonNetwork.PlayerList[number];

        if(PhotonNetwork.IsMasterClient && belongs == -1)
        {
            //Round over
            if (belongsTo == currentPlayer) endRound();
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            setBelongsTo(belongs);
        }
        

    }
    void endRound()
    {
        Card[] stich = new Card[table.transform.childCount];

        for (int i = 0; i < table.transform.childCount; i++)
        {
            GameObject card = table.transform.GetChild(i).gameObject;
            stich[i] = card.GetComponent<CardBehaviour>().card;
            PhotonNetwork.Destroy(card);
        }
        Debug.LogError(stich.ToStringFull());
    }

    [PunRPC]
    void EndRound()
    {
        
        
    }

    public void setBelongsTo(int number)
    {
        photonView.RPC("SetBelongsTo", RpcTarget.All, number);
    }

    [PunRPC]
    void SetBelongsTo(int number)
    {
        currentPlayer = PhotonNetwork.PlayerList[number];
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

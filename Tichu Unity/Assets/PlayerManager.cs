using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Tichu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject camera;

    public Transform cardHolder;

    public List<Card> myCards;

    public Button playButton;

    public int myNumber;

    public GameManager gameManager;

    public 

    List<GameObject> cardObjects = new List<GameObject>();

    bool cardsSet = false;

    public List<GameObject> selected = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            //camera.transform.GetChild(0).transform.parent = transform;
            Destroy(camera);
        }

        else
        {
            playButton = GameObject.Find("UI").GetComponent<UI>().playButton;
            playButton.onClick.AddListener(play);
        }
    }

    

    public void select(GameObject card)
    {
        if (!photonView.IsMine) return;
        selected.Add(card);
    }

    public void unselect(GameObject card)
    {
        if (!photonView.IsMine) return;
        selected.Remove(card);
    }
    public void play()
    {
        Debug.Log("play");
        if (!photonView.IsMine || !gameManager.currentPlayer.IsLocal) return;

        

        //check Kombo
       


        //Pass
        if (selected.Count == 0) {
            gameManager.setCurrentPlayer((myNumber + 1) % 4,-1);
            return;
        }

        

        List<int> ids = new List<int>();
        
        for(int i = 0; i < selected.Count; i++)
        {
            selected[i].GetComponent<CardBehaviour>().setClickable(false);
            ids.Add(selected[i].GetPhotonView().ViewID);
            myCards.Remove(selected[i].GetComponent<CardBehaviour>().card);
            cardObjects.Remove(selected[i]);
        }
        selected.Clear();

        if(myCards.Count == 0)
        {
            gameManager.ranking.Add(myNumber);
            photonView.RPC("RpcRanking", RpcTarget.All, gameManager.ranking.ToArray());
        }

        photonView.RPC("RpcPlay", RpcTarget.All,ids.ToArray(),myNumber);
        gameManager.setCurrentPlayer((myNumber + 1) % 4, myNumber);
    }

    [PunRPC]
    void RpcRanking(int[] ranking)
    {
        gameManager.ranking.Clear();
        foreach (int p in ranking) gameManager.ranking.Add(p);

        Debug.LogError(ranking.IntToString());
    }

    [PunRPC]
    void RpcPlay(int[] ids, int playernumber)
    {
        for(int i = 0; i < ids.Length; i++)
        {
            GameObject card = PhotonView.Find(ids[i]).gameObject;
            if (PhotonNetwork.IsMasterClient)
            {
                card.GetPhotonView().TransferOwnership(PhotonNetwork.MasterClient);
            }
            card.transform.parent = gameManager.table;
            card.transform.position = gameManager.table.position;
            card.transform.rotation = Quaternion.identity;
            card.transform.Rotate(gameManager.spawnPoint[myNumber].rotation.x, 0, 0);
            card.transform.position += card.transform.up*(gameManager.table.childCount) * 0.01f- card.transform.forward*i*0.5f;
            



        }
        Debug.Log("New Card on Table");
    }


    // Update is called once per frame
    void Update()
    {
        if (myCards == null) return;

        if (!cardsSet && photonView.IsMine)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < myCards.Count; i++)
            {
                GameObject card = (PhotonNetwork.Instantiate("CardPrefab", cardHolder.position + new Vector3(0.5f * i, 0.01f * i, 0), cardHolder.rotation));
                ids.Add(card.GetPhotonView().ViewID);
                
                //90 91 RPC du spasst!
            }
            cardsSet = true;
            photonView.RPC("createCards", RpcTarget.All, ids.ToArray());
        }

    }

    [PunRPC]
    private void createCards(int[] ids)
    {
        foreach(int id in ids)
        {
            cardObjects.Add( PhotonView.Find(id).gameObject);
        }
        for (int i = 0; i < myCards.Count; i++)
        {
            cardObjects[i].transform.rotation = cardHolder.rotation;
            cardObjects[i].transform.position = cardHolder.position + cardHolder.transform.right * 0.5f * i + cardHolder.transform.up*i*0.01f; 
            cardObjects[i].GetComponent<CardBehaviour>().setCard(myCards[i]);
            cardObjects[i].transform.parent = cardHolder.transform;
        }
        cardsSet = true;
        cardHolder.transform.Translate(-(0.5f * myCards.Count) / 2, 0, 0);
        
    }
}

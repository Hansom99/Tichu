using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Tichu;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject camera;

    public Transform cardHolder;

    public List<Card> myCards;

    public Button playButton;

    public int myNumber;

    List<GameObject> cardObjects = new List<GameObject>();

    bool cardsSet = false;

    List<GameObject> selected = new List<GameObject>();

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
        if (!photonView.IsMine) return;
        //check Kombo
        if (selected.Count == 0) return;

        
        for(int i = 0; i < selected.Count; i++)
        {
            selected[i].transform.rotation = Quaternion.Euler(0, 90*myNumber, 0);
            selected[i].transform.position = Vector3.zero + new Vector3(0, i * 0.01f, -i * 0.5f);
            selected[i].transform.parent = null;
            myCards.Remove(selected[i].GetComponent<CardBehaviour>().card);
            cardObjects.Remove(selected[i]);
        }
        selected.Clear();
        photonView.RPC("RpcPlay", RpcTarget.All);
    }

    [PunRPC]
    void RpcPlay()
    {
        Debug.Log("New Card on Table");
    }


    // Update is called once per frame
    void Update()
    {
        if (myCards == null) return;

        if (!cardsSet && photonView.IsMine)
        {
            for(int i = 0; i < myCards.Count; i++)
            {
                cardObjects.Add(PhotonNetwork.Instantiate("CardPrefab", cardHolder.position+new Vector3(0.5f*i,0.01f*i,0), cardHolder.rotation));
                cardObjects[i].GetComponent<CardBehaviour>().setCard(myCards[i]);
                cardObjects[i].transform.parent =  cardHolder.transform;
                //90 91 RPC du spasst!
            }
            cardsSet = true;
            cardHolder.transform.Translate(-(0.5f * myCards.Count) / 2, 0, 0);
        }

    }
}

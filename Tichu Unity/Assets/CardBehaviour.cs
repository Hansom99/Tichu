using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Tichu;
using UnityEngine;

public class CardBehaviour : MonoBehaviourPunCallbacks
{
    public Material[] materials;
    public Card card;

    bool selected = false;

    bool clickable = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCard(Card card)
    {
        this.card = card;
        if (card.value >= materials.Length) Debug.Log(card.name + card.value);
        else GetComponent<Renderer>().material = materials[card.value];
    }

    private void OnMouseUpAsButton()
    {
        if (!clickable) return;
        if (selected)
        {
            transform.Translate(0, 0, -1);
            selected = false;
            transform.root.GetComponent<PlayerManager>().unselect(this.gameObject);
        }
        else
        {
            transform.Translate(0, 0, 1);
            selected = true;
            transform.root.GetComponent<PlayerManager>().select(this.gameObject);
        }
        
    }

    public void setClickable(bool clickable)
    {
        this.clickable = clickable;
        photonView.RPC("RpcSetClickable", RpcTarget.All, clickable);
    }

    [PunRPC]
    void RpcSetClickable(bool clickable)
    {
        this.clickable = clickable;
    }

}

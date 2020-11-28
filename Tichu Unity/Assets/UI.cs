using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public Button playButton;
    public Text playButtonText;
    public GameObject gameManagerObject;
    GameManager gameManager;

    public Text NameA, NameB, CurrentPlayer, PointsA, PointsB;

    public Slider sliderA, sliderB;


    private void Start()
    {
        
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }
    private void Update()
    {
        if (gameManager.myPlayerManager.selected.Count == 0)
        {
            playButtonText.text = "Pass";
        }
        else playButtonText.text = "Play Cards";

        //NameA.text = "TeamA\n" + PhotonNetwork.PlayerList[0].NickName + "\n" + PhotonNetwork.PlayerList[2].NickName;
       // NameB.text = "TeamB\n" + PhotonNetwork.PlayerList[1]?.NickName + "\n" + PhotonNetwork.PlayerList[3]?.NickName;
        CurrentPlayer.text = "Waiting for:\n" + gameManager.currentPlayer.NickName;

        PointsA.text = gameManager.teamPoint[0].ToString();
        PointsB.text = gameManager.teamPoint[1].ToString();

        sliderA.value = gameManager.teamPoint[0];
        sliderB.value = gameManager.teamPoint[1];

    }

}

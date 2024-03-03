using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Movement movement;
    public GameObject playerCamera;    
    public GameObject[] cameraBrain;


    private void Start()
    {
        if (photonView.IsMine)
        {
            movement.enabled = true;
            playerCamera.SetActive(true);
            foreach (GameObject brain in cameraBrain)
            {
                brain.SetActive(true);
            }
        }
    }
}

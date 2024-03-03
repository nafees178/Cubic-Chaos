using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviourPun
{
    public Transform moveLocation;
    public bool playerTrigger = false;
    public Animator anim;
    Vector3 orignalPos;
    public float speed = 0.15f;
    private void Start()
    {
        orignalPos = transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!playerTrigger)
        {
            other.transform.SetParent(transform);
        }
        if (playerTrigger)
        {
            if (other.tag == "Player")
            {
                anim.Play("MoveAnim");
                Debug.Log("moving");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!playerTrigger)
        {
            other.transform.SetParent(null);
        }
    }
    private void Update()
    {
        if (!playerTrigger)
        {
            if (photonView.IsMine || !PhotonNetwork.IsConnected)
            {
                // Only move the obstacle if it's the owner or if not in a networked game
                photonView.RPC("MoveObstacle", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void MoveObstacle()
    {
        // Move the obstacle back and forth between pointA and pointB
        transform.position = Vector3.Lerp(orignalPos, moveLocation.position, Mathf.PingPong(Time.time * speed, 1.0f));
    }

}


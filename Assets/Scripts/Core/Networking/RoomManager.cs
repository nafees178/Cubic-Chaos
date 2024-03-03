using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject player;
    public Transform[] spawnpoint;
    [SerializeField] int pointCount;


    public void Start()
    {
        pointCount = Random.Range(0, spawnpoint.Length);
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnpoint[pointCount].position, Quaternion.identity);
        _player.transform.SetPositionAndRotation(new Vector3(Random.Range(0,5f),transform.position.y,transform.position.z), Quaternion.identity); 
    }
}

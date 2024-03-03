using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Finishlvl : MonoBehaviourPunCallbacks
{
    private List<Collider> playersInsideTrigger = new List<Collider>();
    public Animator finishLvlAnim;
    GameObject[] finishCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playersInsideTrigger.Contains(other))
            {
                playersInsideTrigger.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInsideTrigger.Remove(other);
        }
    }

    private void CheckPlayersInsideTrigger()
    {
        if (PhotonNetwork.InRoom)
        {
            int playersInRoom = PhotonNetwork.CurrentRoom.PlayerCount;

            if (playersInsideTrigger.Count == playersInRoom)
            {
                
                StartCoroutine(finishLvl());


            }

        }
    }

    // You can call CheckPlayersInsideTrigger() when needed, for example, in an Update method or when a specific event occurs.
    void Update()
    {
        CheckPlayersInsideTrigger();

    }
    IEnumerator finishLvl()
    {
        finishCam = GameObject.FindGameObjectsWithTag("LvlFinishCamera");
        foreach (GameObject cam in finishCam)
        {
            cam.GetComponent<CinemachineVirtualCamera>().Priority = 15;
        }
        yield return new WaitForSeconds(2);
        Debug.Log("Switched View");
        finishLvlAnim.Play("DoorOpen");
        Debug.Log("LevelFinish"); 
        yield return new WaitForSeconds(3);

        // Load the next level
        LoadNextLevel();
    }


    void LoadNextLevel()
    {
        // Destroy existing players
        //foreach (var player in playersInsideTrigger)
        //{
        //PhotonView pv = player.GetComponent<PhotonView>();
        //pv.RPC("DestroyPlayer", RpcTarget.All);
        //}

        // Load the next level

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}

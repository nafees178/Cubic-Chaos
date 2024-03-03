using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{

    Vector3 spawnpos;
    Vector3 startpos = new Vector3(0,1,0);

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "respawn")
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        spawnpos = GetComponent<CheckPoint>().currentCheckPoint;
        Debug.Log(spawnpos);
        transform.position = spawnpos;
        if (spawnpos == startpos)
        {
            gameObject.GetComponent<Movement>().gravity = 15;
            gameObject.GetComponent<Movement>().jumpForce = 8;
            gameObject.GetComponent<Movement>().playerCam.Priority = 10;
            gameObject.GetComponent<Movement>().isFrozen = false;
            gameObject.GetComponent<Movement>().justFrozen = false;

        }
    }

}

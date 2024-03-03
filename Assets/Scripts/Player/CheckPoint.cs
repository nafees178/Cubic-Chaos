using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Vector3 currentCheckPoint = new Vector3(0,1,0);

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CheckPoint")
        {
            currentCheckPoint = other.transform.position;
        }
    }
}

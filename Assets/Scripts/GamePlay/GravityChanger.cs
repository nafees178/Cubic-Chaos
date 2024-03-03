using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Movement>().gravity *= -1;
            other.GetComponent<Movement>().jumpForce *= -1;
            other.GetComponent<Movement>().changePriority();
        }
    }
}

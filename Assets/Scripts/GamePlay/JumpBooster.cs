using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBooster : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (other.GetComponent<Movement>().jumpForce > 0)
            {
                other.GetComponent<Movement>().jumpForce = 20f;
            }else if(other.GetComponent<Movement>().jumpForce < 0)
            {
                other.GetComponent<Movement>().jumpForce = -20f;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<Movement>().jumpForce > 0)
            {
                other.GetComponent<Movement>().jumpForce = 8f;
            }
            else if (other.GetComponent<Movement>().jumpForce < 0)
            {
                other.GetComponent<Movement>().jumpForce = -8f;
            }
            
        }
    }
}

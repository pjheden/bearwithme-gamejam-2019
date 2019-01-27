using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Set player safe
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            pc.SetIsSafe(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Remove player safe
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            pc.SetIsSafe(false);
        }
    }
}

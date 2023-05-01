using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeliveryPoint : MonoBehaviour {

    private BoxCollider2D bc;
    public bool isTriggered = false;
    public bool isTriggeredByBox = false;


    void OnTriggerEnter2D(Collider2D other) {

        isTriggered = true;
        if (other.GetComponent<Box>() != null) {
            isTriggeredByBox = true;

            // This should probably be handled in Game.cs but I don't know how to easily access it from here.
            SceneManager.LoadScene("Victory", LoadSceneMode.Single);
        }
    }


    void OnTriggerExit2D(Collider2D other) {

        isTriggered = false;
        if (other.GetComponent<Box>() != null) {
            isTriggeredByBox = false;
        }
    }
}

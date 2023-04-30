using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{

    private BoxCollider2D bc;
    public bool isTriggered = false;
    public bool isTriggeredByBox = false;

    // Start is called before the first frame update
    void Start() {
      // bc = GetComponent<BoxCollider2D>;
    }

    // Update is called once per frame
    void Update() {

    }


    void OnTriggerEnter2D(Collider2D other) {

      isTriggered = true;
      if (other.GetComponent<Box>() != null) {
        isTriggeredByBox = true;

        //DisplayVictoryScene(); // this method exists in Game.cs - how to run it from here?
      }
    }


    void OnTriggerExit2D(Collider2D other) {

      isTriggered = false;
      if (other.GetComponent<Box>() != null) {
        isTriggeredByBox = false;
      }
    }
}

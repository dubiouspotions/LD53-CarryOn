using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDetection : MonoBehaviour
{
    public bool isDetected = false;   
    public float DetectionTimer = 1.0f;
    private float DetectionCountdown = 0f;

    SpriteRenderer sprite;
    Light light;

    public GameObject Player;

    public LineRenderer LineRenderer; // draws the laz0r

    // Start is called before the first frame update
    void Start() {
        DetectionCountdown = DetectionTimer;
        sprite = GetComponent<SpriteRenderer>();
        light = GetComponentsInChildren<Light>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (isDetected) {
          if (DetectionCountdown <= 0 ) {
            // PLAYER HAS BEEN DETECTED & COUNTDOWN IS AT 0, FIRE BEAM
            sprite.color = new Color(0, 1, 0, 0f);
            light.color = new Color(0, 1, 0, 1f);
            //transform.localScale = new Vector3(0.05f, transform.localScale.y, transform.localScale.z);


            LineRenderer.startColor = new Color(0, 1, 0, 1);
            LineRenderer.endColor = new Color(0, 1, 0, 0.8f);
            LineRenderer.startWidth = 0.2f;
            LineRenderer.endWidth = 0.2f;
            LineRenderer.SetPosition(0, transform.position);
            
            LineRenderer.SetPosition(1, Player.transform.position);




          } else {
            DetectionCountdown -= Time.deltaTime;
          }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other) {
      
      if (other.GetComponentInParent<Player>() != null) {
        isDetected = true;
        sprite.color = new Color(1, 0, 0, 0.25f);
        light.color = new Color(1, 0, 0, 0.25f);
      }
    }

    void OnTriggerExit2D(Collider2D other) {
      if (other.GetComponentInParent<Player>() != null) {
        // Player has escaped detection, reset the timer
        isDetected = false;
        DetectionCountdown = DetectionTimer;
        sprite.color = new Color(1, 1, 0, 0.2f);
        light.color = new Color(1, 1, 0, 0.2f);
      }
    }
}

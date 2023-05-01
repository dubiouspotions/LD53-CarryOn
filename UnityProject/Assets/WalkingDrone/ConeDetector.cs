using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDetector : MonoBehaviour
{
    SpriteRenderer sprite;
    public bool isDetected = false;   
    public float DetectionTimer = 2.0f;
    private float DetectionCountdown = 0f;
    public Player Player;
    
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1, 1, 0, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDetected) {
          if (DetectionCountdown <= 0 ) {
            sprite.color = new Color(0, 1, 0, 0f);
            
            Player.KillSlowly();

          } else {
            DetectionCountdown -= Time.deltaTime;
          }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
      
      if (other.GetComponentInParent<Player>() != null) {
        isDetected = true;
        DetectionCountdown = DetectionTimer;
        sprite.color = new Color(1, 0, 0, 0.25f);
        Player= other.GetComponentInParent<Player>();
      }
    }

    void OnTriggerExit2D(Collider2D other) {
      if (other.GetComponentInParent<Player>() != null) {
        // Player has escaped detection, reset the timer
        isDetected = false;
        sprite.color = new Color(1, 1, 0, 0.2f);
      }
    }
}

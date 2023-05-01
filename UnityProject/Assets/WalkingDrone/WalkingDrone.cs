using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingDrone : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator animator;
    float time;
    float dir = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var isDetected = GetComponentInChildren<ConeDetector>().isDetected;
        if (!isDetected) {
            rb.velocity = new Vector2(5 * dir, 0);
        } else {
            rb.velocity = new Vector2(0, 0);
        }
        
        time += Time.deltaTime;
        if (time > 5) {
            time = 0;
            dir = -dir;
            var trans = GetComponent<Transform>();
            var scale = trans.localScale;
            scale.x = -scale.x;
            trans.localScale = scale;
        }
    }
}

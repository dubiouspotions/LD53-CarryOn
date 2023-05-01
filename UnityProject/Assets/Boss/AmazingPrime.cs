using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmazingPrime : MonoBehaviour {

    Animator animator => GetComponent<Animator>();

    public bool IsSafe;
    bool _IsSafe;

    public float NextTurnTime;
    public float WaitTime = 3.0f;

    private void Start() {
        NextTurnTime = Time.time + WaitTime;
    }
    void Update() {
        if (Time.time > NextTurnTime) {
            IsSafe = !IsSafe;
            NextTurnTime = Time.time + WaitTime;
        }
        animator.SetBool("Safe", IsSafe);
    }

    private void OnValidate() {
        Update();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        print("Prime got a present: " + other.gameObject.name);
        if (other.gameObject.name.Contains("HeroBox")) {
            animator.SetTrigger("Died");
        }
    }
}

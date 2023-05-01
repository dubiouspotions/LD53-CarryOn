using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmazingPrime : MonoBehaviour {

    Animator animator => GetComponent<Animator>();

    public bool IsSafe;
    bool _IsSafe;

    public bool IsDead;
    bool _IsDead;

    public float NextTurnTime;
    public float WaitTime = 3.0f;

    private void Start() {
        NextTurnTime = Time.time + WaitTime;
    }
    void Update() {
        if (!IsDead && Time.time > NextTurnTime) {
            IsSafe = !IsSafe;
            NextTurnTime = Time.time + WaitTime;
        }
        animator.SetBool("Safe", IsSafe);
        if (IsDead != _IsDead) {
            _IsDead = IsDead;
            if (IsDead) Death();
        }
    }

    private void OnValidate() {
        Update();
    }

    void Death() {
        Destroy(GameObject.Find("Boxes"));

        GameObject.Find("Player").GetComponent<Player>().PrepareToWin();

        transform.Find("Cone").GetComponent<SpriteRenderer>().enabled = false;
        animator.SetTrigger("Died");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        print("Prime got a present: " + other.gameObject.name);
        if (other.gameObject.name.Contains("HeroBox")) {
            Destroy(other.gameObject);
            Death();
        }
    }

    private void DeathComplete() {
        GameObject.Find("Player").GetComponent<Player>().SamusForvandlaMig();
    }
}

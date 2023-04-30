using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Rigidbody2D rb;



    void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        UpdateSpriteScale();
        rb.centerOfMass.Set(rb.centerOfMass.x, rb.centerOfMass.y + bc.size.y / 3);
    }

    void Update() {
      

    }

    private void OnValidate() {
        UpdateSpriteScale();
    }


    private void UpdateSpriteScale() {
        if (sr == null || bc == null || rb == null) {
            sr = GetComponentInChildren<SpriteRenderer>();
            bc = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }
        sr.size = bc.size;
        sr.transform.localScale = new Vector3(bc.size.x / sr.sprite.bounds.size.x, bc.size.y / sr.sprite.bounds.size.y, 1f);
        GetComponentInChildren<TextMesh>().text = rb.mass.ToString("N0") + "Kg";
    }

    private void OnDrawGizmosSelected() {
        UpdateSpriteScale();
    }
}

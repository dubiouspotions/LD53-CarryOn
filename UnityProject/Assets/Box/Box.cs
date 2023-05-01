using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    public Transform CenterOfMass;



    void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        UpdateSpriteScale();
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

        var centerOfMass = rb.centerOfMass;
        if (CenterOfMass != null) {
            centerOfMass = CenterOfMass.position - transform.position;
        }

        rb.centerOfMass = centerOfMass;
    }

    private void OnDrawGizmosSelected() {
        UpdateSpriteScale();
        Gizmos.DrawSphere(rb.position + rb.centerOfMass, .1f);
    }


    public void SetCollisionEnabled(bool enable) {
        foreach (var collider in GetComponentsInChildren<Collider2D>()) {
            collider.enabled = enable;
        }
        foreach (var body in GetComponentsInChildren<Rigidbody2D>()) {
            body.simulated = enable;
        }
    }
}

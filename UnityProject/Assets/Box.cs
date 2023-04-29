using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    private SpriteRenderer sr;
    private BoxCollider2D bc;


    void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        UpdateSpriteScale();
    }

    private void OnValidate() {
        UpdateSpriteScale();
    }


    private void UpdateSpriteScale() {
        if (sr == null || bc == null) {
            sr = GetComponentInChildren<SpriteRenderer>();
            bc = GetComponent<BoxCollider2D>();
        }
        sr.size = bc.size;
        sr.transform.localScale = new Vector3(bc.size.x / sr.sprite.bounds.size.x, bc.size.y / sr.sprite.bounds.size.y, 1f);

    }

    private void OnDrawGizmosSelected() {
        UpdateSpriteScale();
    }
}

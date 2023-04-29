using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public float speed = 50f;

    Vector2 GetMovement() {
        Vector2 move = new Vector2();
        if (Input.GetKey(KeyCode.RightArrow)) {
            move.x += 1f;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            move.x -= 1f;
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            move.y += 1;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            move.y -= 1;
        }
        return move;
    }

    private void Update() {
        var move = GetMovement() * speed * Time.deltaTime;

        rb.AddForce(move);

        // transform.position += new Vector3(move.x, move.y, 0f);
    }
}

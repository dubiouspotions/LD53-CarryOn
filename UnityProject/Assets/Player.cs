using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    public Transform Graphics;


    public float MoveSpeed = 50f;
    public float JumpForce = 5f;
    public Transform GroundCheckObject;
    public float GroundCheckRadius = 0.1f;
    public LayerMask GroundLayer;
    public bool isGrounded;

    public Transform Arms;
    public float ArmRadius = 0.25f;
    public LayerMask BoxesLayer;

    GameObject carriedBox;
    Vector3 boxGrabOffset;
    Vector3 armInitialPos;

    public SpriteRenderer SpriteRenderer;


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        armInitialPos = Arms.position - Arms.parent.position;
    }

    float Deadzoned(float value, float absmax) {
        if (Mathf.Abs(value) < value) return 0;
        return value;
    }

    private void Update() {

        // ---------- Movement
        //check if player is on the ground
        isGrounded = Physics2D.OverlapCircle(GroundCheckObject.position, GroundCheckRadius, GroundLayer);

        //jump into the air
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb.velocity += new Vector2(0, JumpForce);
        }

        // move horizontally
        rb.velocity = new Vector2(Deadzoned(Input.GetAxisRaw("Horizontal"), 0.1f) * MoveSpeed, Deadzoned(rb.velocity.y, 0.1f));


        if (Graphics != null) {
            Graphics.transform.eulerAngles = Vector3.zero;
        }

        // are we moving?
        var vx = Deadzoned(rb.velocity.x, 0.1f);
        if (Mathf.Abs(vx) > 0) {
            // are we facing where we think?
            var facingLeft = vx < 0;
            var s = Graphics.transform.localScale;
            s.x = facingLeft ? -1 : 1;
            Graphics.transform.localScale = s;
        }

        // --------Box carrying

        if (Input.GetKeyDown(KeyCode.E)) {
            if (carriedBox == null) {
                var boxCollider = Physics2D.OverlapCircle(Arms.position, ArmRadius, BoxesLayer);
                if (boxCollider != null) {
                    carriedBox = boxCollider.gameObject;
                    boxGrabOffset = carriedBox.transform.position - transform.position;
                    boxGrabOffset.y += 0.2f;
                    carriedBox.GetComponent<Rigidbody2D>().gravityScale = 0;
                }
            } else {
                carriedBox.GetComponent<Rigidbody2D>().gravityScale = 1;
                carriedBox = null;
            }
        }

    }

    private void FixedUpdate() {
        if (carriedBox != null && Arms != null) {
            carriedBox.transform.position = transform.position + boxGrabOffset;
        }
    }

    private void OnDrawGizmos() {
        if (GroundCheckObject != null)
            Gizmos.DrawSphere(GroundCheckObject.position, GroundCheckRadius);
        if (Arms != null)
            Gizmos.DrawSphere(Arms.position, ArmRadius);
    }
}

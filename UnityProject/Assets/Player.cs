using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    public Transform Graphics;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

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

    private void Update() {
        //check if player is on the ground
        isGrounded = Physics2D.OverlapCircle(GroundCheckObject.position, GroundCheckRadius, GroundLayer);

        //jump into the air
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb.velocity += new Vector2(0, JumpForce);
        }

        // move horizontally
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, rb.velocity.y);


        if (Graphics != null) {
            Graphics.transform.eulerAngles = Vector3.zero;
        }



        if (Input.GetKeyDown(KeyCode.E)) {
            if (carriedBox == null) {
                var boxCollider = Physics2D.OverlapCircle(Arms.position, ArmRadius, BoxesLayer);
                if (boxCollider != null) {
                    carriedBox = boxCollider.gameObject;
                    boxGrabOffset = carriedBox.transform.position - Arms.position;
                    boxGrabOffset.y += 0.2f;
                }
            } else {
                carriedBox = null;
            }
        }
        if (carriedBox != null && Arms != null) {
            carriedBox.transform.position = Arms.position + boxGrabOffset;
        }
    }

    private void FixedUpdate() {

    }

    private void OnDrawGizmos() {
        if (GroundCheckObject != null)
            Gizmos.DrawSphere(GroundCheckObject.position, GroundCheckRadius);
        if (Arms != null)
            Gizmos.DrawSphere(Arms.position, ArmRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    public Transform Graphics;


    public float MoveSpeed = 50f;
    public float JumpForce = 30f;


    public Transform GroundCheckObject;
    public float GroundCheckRadius = 0.1f;
    public LayerMask GroundLayer;
    public bool isGrounded;
    public List<Collider2D> colliderList;
    private ContactFilter2D colliderContactFilter = new ContactFilter2D();


    public float hangTime = 0.2f;
    private float hangCounter;

    public Transform Arms;
    public float ArmRadius = 0.25f;

    public Transform Feet;

    public LayerMask BoxesLayer;

    public GameObject carriedBox;
    public Vector3 boxGrabOffset;

    public Transform ThrowTarget;
    public float ThrowTargetRadius;

    public SpriteRenderer SpriteRenderer;
    /// <summary>
    ///  How fast a picked up box rotates to straight
    /// </summary>
    public float PickupStraighteningSpeed = 30f;
    /// <summary>
    /// How fast a box is picked up
    /// </summary>
    public float PickupSpeed = 30f;
    public Vector2 throwForce = new Vector2(30, 30);

    public Animator Animator;
    public bool IsDucking;


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        colliderList = new List<Collider2D>();
        colliderContactFilter.SetLayerMask(GroundLayer);
    }

    float Deadzoned(float value, float absmax) {
        if (Mathf.Abs(value) < value) return 0;
        return value;
    }

    private void Update() {

        // ---------- Movement
        //check if player is on the ground -- Replaced by Box collider triggers above
        //isGrounded = Physics2D.OverlapCircle(GroundCheckObject.position, GroundCheckRadius, GroundLayer);

        colliderList.Clear();
        if (Physics2D.OverlapCollider(GroundCheckObject.GetComponent<BoxCollider2D>(), colliderContactFilter, colliderList) > 0) {
          isGrounded = true;
        } else {
          isGrounded = false;
        }

        if (isGrounded) {
          hangCounter = hangTime;
        } else {
          hangCounter -= Time.deltaTime;
        }

        Animator = GetComponentInChildren<Animator>();

        //jump into the air
        if (hangCounter > 0f && Input.GetButtonDown("Jump")) {
            rb.velocity += new Vector2(0, JumpForce);
        }

        // Allow jumps to semi-interrupted if the jump button is released early mid-jump
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) {
          rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
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

        if (Animator != null) {
            Animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            Animator.SetBool("Jumping", !isGrounded);
            Animator.SetFloat("VerticalSpeed", rb.velocity.y);
            Animator.SetBool("HasBox", carriedBox != null);
            Animator.SetBool("Ducking", IsDucking);
        }

        // --------Ducking
        IsDucking = Input.GetKey(KeyCode.S);

        // --------Box carrying

        var throwBox = Input.GetKeyDown(KeyCode.E);
        var grabBox = Input.GetKeyDown(KeyCode.S);

        if (grabBox || throwBox) {
            if (carriedBox == null) {
                var boxCollider = Physics2D.OverlapCircle(Arms.position, ArmRadius, BoxesLayer);
                if (boxCollider != null) {
                    carriedBox = boxCollider.gameObject;
                    boxGrabOffset = carriedBox.transform.position - Arms.position;
                    boxGrabOffset.y += 0.2f;
                    carriedBox.GetComponent<Rigidbody2D>().gravityScale = 0;
                    carriedBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), boxCollider, true);
                }
            } else if (carriedBox) {
                carriedBox.GetComponent<Rigidbody2D>().gravityScale = 1;
                carriedBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), carriedBox.GetComponentInChildren<Collider2D>(), false);

                if (throwBox) {

                    carriedBox.GetComponent<Rigidbody2D>().AddForce(throwForce, ForceMode2D.Force);
                }
                carriedBox = null;

                var targetBox = Physics2D.OverlapCircle(ThrowTarget.position, ThrowTargetRadius, LayerMask.NameToLayer("Boxes")) as BoxCollider2D;
                if (targetBox != null) {
                    var targetPos = targetBox.transform.position;
                    targetPos.y += targetBox.size.y / 2;
                    Gizmos.DrawSphere(targetBox.transform.position, 0.2f);
                }
            }
        }

    }

    private void FixedUpdate() {

        UpdateCarriedBox();
    }

    void UpdateCarriedBox() {
        if (carriedBox != null && Arms != null) {
            var box = carriedBox.GetComponentInChildren<BoxCollider2D>();
            var boxSize = box.size / 2f;
            print(boxGrabOffset.y + " " + boxSize.y);
            if (boxGrabOffset.y < boxSize.y)
                boxGrabOffset.y += PickupSpeed * Time.deltaTime;
            carriedBox.transform.position = Arms.position + boxGrabOffset;

            var rot = carriedBox.transform.rotation.eulerAngles.z;
            var newRot = Quaternion.Euler(0, 0, Mathf.MoveTowardsAngle(
                rot, (int)(rot / 90) * 90, PickupStraighteningSpeed * Time.deltaTime
            ));
        }
    }

    private void OnDrawGizmos() {
        UpdateCarriedBox();
        if (GroundCheckObject != null)
            Gizmos.DrawSphere(GroundCheckObject.position, GroundCheckRadius);
        if (Arms != null)
            Gizmos.DrawSphere(Arms.position, ArmRadius);
        if (ThrowTarget != null)
            Gizmos.DrawSphere(ThrowTarget.position, ThrowTargetRadius);
    }
}

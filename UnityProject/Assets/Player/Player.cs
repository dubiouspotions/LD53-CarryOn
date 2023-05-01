using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    public Transform Graphics;


    public float MoveSpeed = 50f;
    public float MoveForce = 50 * 100f;
    public float JumpForce = 30f;
    // How fast to stop without input
    public float MovementDampening = 0.2f;


    public Transform GroundCheckObject;
    public float GroundCheckRadius = 0.1f;
    public LayerMask GroundLayer;
    public bool isGrounded;
    public List<Collider2D> colliderList;
    private ContactFilter2D colliderContactFilter = new ContactFilter2D();


    public float hangTime = 0.2f;
    private float hangCounter;

    public Transform ArmsStandingPosition; // Just a positioning handle
    public Transform ArmsDuckingPosition; // Just a positioning handle
    public Transform ArmsUpPosition; // Just a positioning handle
    public Transform Hand; //Moves between above positions and actually holds the box
    public float ArmRange = 1;

    public Vector2 ThrowForce = new Vector2(1000, 1000);

    public Transform Feet;

    public LayerMask BoxesLayer;

    public GameObject carriedBox;

    public SpriteRenderer SpriteRenderer;
    /// <summary>
    ///  How fast a picked up box rotates to straight
    /// </summary>
    public float PickupStraighteningSpeed = 30f;
    /// <summary>
    /// How fast a box is picked up
    /// </summary>
    public float PickupSpeed = 30f;

    public Animator Animator;
    public bool IsDucking;
    public bool IsHandsup;
    public bool IsCarrying => carriedBox != null;
    public bool IsFacingLeft;
    public bool IsDead;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        colliderList = new List<Collider2D>();
        colliderContactFilter.SetLayerMask(GroundLayer);
    }

    float Deadzoned(float value, float absmax = 0.1f) {
        if (Mathf.Abs(value) < value) return 0;
        return value;
    }

    private void Update() {

        CheckGameOver();


        if (IsDead) return;

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

        // jump into the air
        if (hangCounter > 0f && Input.GetButtonDown("Jump") && !IsDucking) {
            rb.velocity += new Vector2(0, JumpForce);
            // rb.AddForce(new Vector2(0, JumpForce * 3000));
        }

        // Allow jumps to semi-interrupted if the jump button is released early mid-jump
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // No move when ducking
        float xInput = IsDucking ? 0 : Input.GetAxisRaw("Horizontal");
        if (xInput != 0f) {
            rb.AddForce(new Vector2(
                Deadzoned(xInput, 0.1f) * MoveForce * 100,
                Deadzoned(rb.velocity.y + 10f, 0.1f)
            ));
        }
        // Limit speed or apply dampening
        if (Mathf.Abs(rb.velocity.x) > MoveSpeed || xInput == 0) {
            float dampening = xInput == 0 ? MovementDampening : 1;
            float maxSpeed = Mathf.Min(MoveSpeed, xInput == 0 ? Mathf.Abs(rb.velocity.x) * dampening : MoveSpeed);
            rb.velocity = new Vector2(
                Deadzoned(maxSpeed * Mathf.Sign(rb.velocity.x)),
                rb.velocity.y
            );
        }

        if (Graphics != null) {
            Graphics.transform.eulerAngles = Vector3.zero;
        }

        // are we moving?
        if (Mathf.Abs(xInput) > 0) {
            // are we facing where we think?
            IsFacingLeft = xInput < 0;
            var s = Graphics.transform.localScale;
            s.x = IsFacingLeft ? -1 : 1;
            Graphics.transform.localScale = s;
        }

        if (Animator != null) {
            Animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            Animator.SetBool("Jumping", !isGrounded);
            Animator.SetFloat("VerticalSpeed", rb.velocity.y);
            Animator.SetBool("HasBox", carriedBox != null);
            Animator.SetBool("Ducking", IsDucking);
            Animator.SetBool("HandsUp", IsHandsup);
            Animator.SetBool("Dead", IsDead);
        }

        // -------- Up and down
        // standing + up => nothing
        // standing + down => ducking
        // holding + up => handsup
        // holding + down + handsup => !handsup

        if (Input.GetKeyDown(KeyCode.W)) { // up
            if (IsCarrying) {
                IsHandsup = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            if (IsHandsup) {
                IsHandsup = false;
            } else if (!IsCarrying) {
                IsDucking = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.S)) {
            IsDucking = false;
        }

        if (IsHandsup && !IsCarrying) {
            IsHandsup = false;
        }

        if (IsDucking && IsCarrying) {
            IsDucking = false;
        }

        // --------Box carrying

        var arm = ArmsStandingPosition;
        if (IsDucking) arm = ArmsDuckingPosition;
        if (IsHandsup) arm = ArmsUpPosition;
        Hand.transform.position = arm.transform.position.xy0();
        var handRadius = arm.transform.position.z;

        // --------Box grabbing
        var grabBox = Input.GetKeyDown(KeyCode.E);
        if (grabBox) {
            if (carriedBox == null) {
                var boxCollider = Physics2D.OverlapCircle(Hand.position, handRadius, BoxesLayer);
                if (boxCollider != null) {
                    carriedBox = boxCollider.gameObject;
                    carriedBox.GetComponent<Rigidbody2D>().gravityScale = 0;
                    carriedBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    foreach (var collider in GetComponentsInChildren<Collider2D>()) {
                        Physics2D.IgnoreCollision(collider, boxCollider, true);
                    }
                }
            } else if (carriedBox) {
                carriedBox.GetComponent<Rigidbody2D>().gravityScale = 1;
                carriedBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                var boxCollider = carriedBox.GetComponentInChildren<Collider2D>();
                foreach (var collider in GetComponentsInChildren<Collider2D>()) {
                    Physics2D.IgnoreCollision(collider, boxCollider, false);
                }
                if (IsHandsup) {
                    var f = ThrowForce;
                    f.x *= IsFacingLeft ? -1 : 1;
                    carriedBox.GetComponent<Rigidbody2D>().AddForce(f);
                }
                carriedBox = null;
            }
        }
    }

    private void FixedUpdate() {
        UpdateCarriedBox();
    }

    Vector3 HeldCorner(BoxCollider2D box) {
        var size = box.size / 2f;
        if (IsFacingLeft) size.x = -size.x;
        return size;
    }

    void UpdateCarriedBox() {
        if (carriedBox != null && Hand != null) {
            var box = carriedBox.GetComponentInChildren<BoxCollider2D>();
            var corner = HeldCorner(box);
            carriedBox.transform.position = Hand.position + corner;

            var rot = carriedBox.transform.rotation.eulerAngles.z;
            var newRot = Quaternion.Euler(0, 0, Mathf.MoveTowardsAngle(
                rot, (int)(rot / 90) * 90, PickupStraighteningSpeed * Time.deltaTime
            ));
        }
    }

    void CheckGameOver() {
        // Trigger game over if the player falls below a certain y value
        if (transform.position.y <= -30) {
            GotoGameOver();
        }
    }

    void KillSlowly() {
        IsDead = true;
        Animator.SetTrigger("Died");
    }

    void GotoGameOver() {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    void DrawHandGizmo(Transform hand) {
        var v = hand.position;
        float r = v.z;
        v.z = 0;
        Gizmos.DrawWireSphere(v, r);
    }

    private void OnDrawGizmos() {
        UpdateCarriedBox();
        if (GroundCheckObject != null)
            Gizmos.DrawWireSphere(GroundCheckObject.position, GroundCheckRadius);
        if (ArmsStandingPosition != null)
            DrawHandGizmo(ArmsStandingPosition);
        if (ArmsDuckingPosition != null)
            DrawHandGizmo(ArmsDuckingPosition);
        if (ArmsUpPosition != null)
            DrawHandGizmo(ArmsUpPosition);
    }

    void DeathAnimationComplete() {
        GotoGameOver();
    }
}


public static class VecTools {
    public static Vector2 xy(this Vector3 v) {
        return new Vector2(v.x, v.y);
    }

    public static Vector3 xy0(this Vector3 v) {
        return new Vector3(v.x, v.y, 0);
    }
}
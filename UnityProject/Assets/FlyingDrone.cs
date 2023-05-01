using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDrone : MonoBehaviour {

    public bool FacingRight;
    bool _FacingRight;
    Animator animator => GetComponent<Animator>();

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (FacingRight != _FacingRight) {
            _FacingRight = FacingRight;
            animator.SetBool("FacingRight", FacingRight);
        }
    }

    private void OnValidate() {
        Update();
    }
}

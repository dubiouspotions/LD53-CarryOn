using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    Animator animator => GetComponent<Animator>();


    public Transform Platform;

    public GameObject BoxPrefab;


    bool _IsEndPortal;
    public bool IsEndPortal;

    bool _isOpen;
    public bool IsOpen;

    Box attachedBox;

    void Start() {

    }

    void Update() {
        if (IsOpen != _isOpen) {
            _isOpen = IsOpen;
            animator.SetTrigger("Open");
        }
        if (IsEndPortal != _IsEndPortal) {
            _IsEndPortal = IsEndPortal;
            if (IsEndPortal)
                animator.SetTrigger("IsEndPortal");
        }
    }

    private void OnValidate() {
        Update();
    }

    public void PlatformStarting() {
        var box = Instantiate(BoxPrefab, Platform.transform.position, Quaternion.identity);
        box.transform.SetParent(Platform);
        attachedBox = box.GetComponent<Box>();// get the box class
        attachedBox.SetCollisionEnabled(false);
    }

    public void PlatformHasArrived() {
        var box = attachedBox;
        box.transform.SetParent(GameObject.Find("Boxes").transform);
        box.SetCollisionEnabled(true);
        attachedBox = null;
    }
}

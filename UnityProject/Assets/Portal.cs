using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    Animator animator => GetComponent<Animator>();


    public Transform Platform;

    public GameObject BoxPrefab;

    bool _isOpen;
    public bool IsOpen;

    Box attachedBox;

    void DoOpen() {
        _isOpen = true;
        animator.SetTrigger("Open");
    }

    void Start() {

    }

    void Update() {
        if (IsOpen && !_isOpen) {
            DoOpen();
        }
    }

    public void PlatformStarting() {
        var box = Instantiate(BoxPrefab, Platform.transform.position, Quaternion.identity);
        box.transform.SetParent(Platform);
        box.transform.position += Vector3.forward;
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

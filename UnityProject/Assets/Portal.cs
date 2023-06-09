using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    Animator animator => GetComponent<Animator>();


    public Transform Platform;

    public GameObject BoxPrefab;

    public int BoxSpawnCount = 1;

    bool _IsEndPortal;
    public bool IsEndPortal;

    bool _isOpen;
    public bool IsOpen;

    Box attachedBox;

    void FixedUpdate() {
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
        FixedUpdate();
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

    private void OnTriggerEnter2D(Collider2D other) {
        print("Spawning box");
        //if (other.transform.GetComponent<Player>() != null) {
        SpawnBox();
        //}
    }

    public void SpawnBox() {
        if (BoxSpawnCount <= 0) return;
        BoxSpawnCount -= 1;
        animator.SetTrigger("Open");
    }
}

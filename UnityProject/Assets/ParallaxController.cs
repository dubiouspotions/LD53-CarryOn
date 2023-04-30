using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    Transform cam; // Main Camera
    Vector3 camStartPos;
    float distance; // distance between the camera start position and its current position.

    GameObject[] backgrounds;
    Material[] mat; 
    float[] backSpeed;

    float farthestBack;
    [Range(0.01f,0.05f)]
    public float parallaxSpeed;


    // Start is called before the first frame update
    void Start() {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++) {
          backgrounds[i] = transform.GetChild(i).gameObject;
          mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        } 

        backSpeedCalculate(backCount);
    }

    void backSpeedCalculate(int backCount) {
      for (int i = 0; i < backCount; i++) { // find the farthest background
        if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack) {
          farthestBack = backgrounds[i].transform.position.z - cam.position.z;
        }
      }

      for (int i = 0; i < backCount; i++) { // set the speed of backgrounds
        backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
      }
    }

    private void LateUpdate() {
      // distance = cam.position.x - camStartPos.x;
      // transform.position = new Vector3(cam.position.x, cam.position.y, 0);

      for (int i = 0; i < backgrounds.Length; i++) {
        float speed = backSpeed[i] * parallaxSpeed;
        mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
      }
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        if (player.transform) {
          //transform.position = player.transform.position;
          var position = transform.position;
          position.Set(player.transform.position.x, player.transform.position.y, transform.position.z);
          transform.position = position;  
        }
    }
}

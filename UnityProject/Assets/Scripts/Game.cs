using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
      
    }

    public void StartGame() {
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }

    public void DisplayVictoryScene() {
      SceneManager.LoadScene("Victory", LoadSceneMode.Single);
    }
}

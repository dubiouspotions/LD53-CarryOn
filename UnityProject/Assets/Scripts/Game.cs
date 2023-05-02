using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
    public void StartGame() {
        SceneManager.LoadScene("Level0", LoadSceneMode.Single);
    }

    public void GoMenu() {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}

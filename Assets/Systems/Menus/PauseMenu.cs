using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    private PlayerMovement movement;

    [SerializeField] private string mainMenuScene;
    [SerializeField] private GameObject content;

    private bool paused;

    private void Awake() {
        movement = GetComponentInParent<PlayerMovement>();
    }

    private void Start() {
        Pause(false);
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) Pause(!paused);
    }

    private void Pause(bool pause) {

        paused = pause;

        movement.enabled = !pause;
        content.SetActive(pause);
        Time.timeScale = pause ? 0 : 1;

        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pause;
    }

    public void ReturnToMainMenu() {
        Pause(false);
        SceneManager.LoadScene(mainMenuScene);
    }
}

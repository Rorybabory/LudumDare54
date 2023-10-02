using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private SoundEffect fireSound, startSound;
    [SerializeField] private string playScene;

    private void Start() {
        fireSound.Init(gameObject);
        startSound.Init(gameObject);
    }

    public void Play() {

        StartCoroutine(Fall());

        IEnumerator Fall() {
            startSound.Play();
            Camera.main.gameObject.AddComponent<Rigidbody>();

            yield return new WaitForSeconds(3);

            SceneManager.LoadScene(playScene);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        fireSound.Play();
    }

    public void OnPointerExit(PointerEventData eventData) {
        fireSound.Stop();
    }
}

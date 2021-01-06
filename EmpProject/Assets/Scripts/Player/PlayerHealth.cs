using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int health = 100;
    public Transform cameraTransform;
    public Volume sceneSettingsVolume;

    public bool isAlive = true;
    public TextMeshProUGUI text;

    private void Awake() {
        sceneSettingsVolume = FindObjectOfType<Volume>();
    }

    public void Update () {
        float normalizedHealth = health / 100f;

        if (!isAlive) {
            if (InputManager.AnyKeyDown()) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        text.text = $"Health: {health}";
    }

    public void TakeDamage (int amount) {
        health = Mathf.Max(health - amount,0);

        if (health == 0) {
            Kill();
        }
    }

    public void Kill () {
        if (!isAlive) return;

        health = 0;
        isAlive = false;

        cameraTransform.GetComponent<PlayerCamera>().enabled = false;

        var col = cameraTransform.gameObject.AddComponent<SphereCollider>();

        col.radius = 0.25f;

        var rb = cameraTransform.gameObject.AddComponent<Rigidbody>();

        rb.velocity = gameObject.GetComponent<Movement>().velocity;

        //rb.AddForce(Vector3.down);
        //rb.AddTorque(Random.insideUnitSphere * 0.1f);

        foreach (var component in gameObject.GetComponents<MonoBehaviour>()) {
            if (component == this) continue;

            component.enabled = false;
        }

    }
}

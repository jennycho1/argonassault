using System.Collections;
using System.Collections.Generic;
using Unity.QuickSearch;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f;
    [SerializeField] ParticleSystem crashVFX;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("crashed");
        StartCrashSequence();
        // Reload scene

    }

    void StartCrashSequence()
    {
        // effects
        crashVFX.Play();
        GetComponent<MeshRenderer>().enabled = false;
        // disable input control and collider
        GetComponent<PlayerControls>().enabled = false;
        // turn off collider
        transform.GetChild(0).gameObject.SetActive(false);
        //GetComponent<BoxCollider>().enabled = false;
        Invoke("ReloadLevel", 1f);
    }

    void ReloadLevel()
    {
        Debug.Log("Reload Level");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}

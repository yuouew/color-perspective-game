using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemy : MonoBehaviour
{
    /// <summary>
    /// player walks into collider
    /// wait 1 second
    /// indicator turns on
    /// if player in indicator and is moving, kill
    /// wait 1 second
    /// turn indicator off
    /// </summary>
    public GameObject indicator;
    private bool playerInside = false;
    private GameObject player;
    private Color originalEmission; // store this once

    private void Start()
    {
        indicator.GetComponent<MeshRenderer>().enabled = false;
        originalEmission = indicator.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            StartCoroutine(BeginSeeking());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    IEnumerator BeginSeeking()
    {
        // Grab material and base emission
        Material mat = indicator.GetComponent<MeshRenderer>().material;

        // Step 1: Warm-up Phase
        indicator.GetComponent<MeshRenderer>().enabled = true;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", originalEmission * 1f); // low glow

        yield return new WaitForSeconds(1f);

        // Step 2: Kill Mode On
        playerInside = true;
        mat.SetColor("_EmissionColor", originalEmission * 3f); // bright glow

        yield return new WaitForSeconds(0.2f);


        float killTimer = 2f;
        float elapsed = 0f;

        while (elapsed < killTimer)
        {
            elapsed += Time.deltaTime;

            if (playerInside && player.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
            {
                Debug.Log("Player killed!");
                player.GetComponent<MovementDetector>().Respawn();
                //break; // stop checking once player is killed
            }

            yield return null;
        }

        // Step 3: Turn Off
        mat.SetColor("_EmissionColor", originalEmission * 0.1f); // optional: fade out slowly or reset
        yield return new WaitForSeconds(0.2f);
        indicator.GetComponent<MeshRenderer>().enabled = false;

        playerInside = false;
    }

}
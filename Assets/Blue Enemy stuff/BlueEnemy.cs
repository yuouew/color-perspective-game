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
    ///
    /// </summary>

    //public GameObject indicator;

    //self and spawn chance
    public GameObject self;
    private int chance;

    //for player detection
    public bool playerInside = false;
    private GameObject player;

    //stances
    public int stance;
    private bool isWarmingUp = false;
    private bool isKilling = false;
    private bool isDeactivating = false;

    //for indicator color changing
    private Material mat;
    private Color originalEmission;

    private void Start()
    {
        chance = Random.Range(0, 1);
        if (chance == 0)
        {
            self.SetActive(true);
        }
        else
        {
            self.SetActive(false);
        }

        mat = GetComponent<MeshRenderer>().material;

        GetComponent<MeshRenderer>().enabled = false;
        originalEmission = GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");

        stance = 0;
    }

    private void Update()
    {
        switch (stance)
        {
            //passive
            case 0:
                break;

            //activated
            //warming up
            case 1:
                if (!isWarmingUp)
                {
                    StartCoroutine(WarmUp());
                }
                break;

            //kill in zone
            case 2:
                if (!isKilling)
                {
                    StartCoroutine(Killing());
                }
                break;

            //turning off, loop around
            case 3:
                if (!isDeactivating)
                {
                    StartCoroutine(Deactivate());
                }
                break;
        }
    }

    //stance 0
    //indicator collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    //stance 1
    IEnumerator WarmUp()
    {
        isWarmingUp = true;

        GetComponent<MeshRenderer>().enabled = true;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", originalEmission * 1f); // low glow

        yield return new WaitForSeconds(1f);

        stance = 2;
        isWarmingUp = false;
    }

    //stance 2
    IEnumerator Killing()
    {
        isKilling = true;

        mat.SetColor("_EmissionColor", originalEmission * 3f); // bright glow

        yield return new WaitForSeconds(0.2f);

        float killTimer = 2f;
        float elapsed = 0f;

        while (elapsed < killTimer)
        {
            elapsed += Time.deltaTime;

            if (playerInside && player.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
            {
                player.GetComponent<MovementDetector>().Respawn();
            }
            yield return null;
        }
        stance = 3;
        isKilling = false;
    }

    //stance 3
    IEnumerator Deactivate()
    {
        isDeactivating = true;

        mat.SetColor("_EmissionColor", originalEmission * 0.1f); // optional: fade out slowly or reset
        yield return new WaitForSeconds(0.2f);
        GetComponent<MeshRenderer>().enabled = false;

        stance = 0;
        isDeactivating = false;
    }
}
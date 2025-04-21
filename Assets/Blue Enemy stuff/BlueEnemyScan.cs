using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemyScan : MonoBehaviour
{
    //for indicator color changing
    private Material mat;
    private Color originalEmission;

    //test
    public float scanSpeed = 1f; // speed of sweep
    public float scanAngle = 45f; // how wide the scan should be
    public bool isActive = false;

    private float scanStartTime;
    public float bufferTime;
    public float scanTime;

    public GameObject scanParent;
    private float timeOffset;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        mat = GetComponent<MeshRenderer>().material;
        originalEmission = GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        mat.EnableKeyword("_EMISSION");

        StartCoroutine(Scanning());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            StartCoroutine(Killing());
        }
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.GetComponent<MeshRenderer>().enabled = isActive;

        if (isActive)
        {
            float elapsed = Time.time - scanStartTime;
            float scanSpeed = (2 * Mathf.PI) / scanTime; // full wave

            float angle = Mathf.Sin(elapsed * scanSpeed) * scanAngle;
            scanParent.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    IEnumerator Scanning()
    {

        while (true)
        {
            yield return new WaitForSeconds(bufferTime);
            isActive = true;

            while (isActive)
            {
                scanParent.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // clean reset
                scanStartTime = Time.time;

                yield return new WaitForSeconds(scanTime);
                isActive = false;
            }
        }
    }

    IEnumerator Killing()
    {

        mat.SetColor("_EmissionColor", originalEmission * 3f); // bright glow

        player.GetComponent<MovementDetector>().Respawn();

        yield return new WaitForSeconds(.5f);

        mat.SetColor("_EmissionColor", originalEmission);
    }
}

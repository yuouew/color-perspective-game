using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementDetector : MonoBehaviour
{
    //camera obj
    public GameObject view;
    //saved rotation values separate from camera
    private Quaternion frontRotation;
    private Quaternion backRotation;
    //turning back speed
    private float turnSpeed = 1000f;

    public GameObject blackScreen;

    //for movement
    private Rigidbody rb;
    private float moveForce = 5f;
    private bool isRotating = false;
    private bool canMove;

    public Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(fadeInAndOut(blackScreen, false, 1f));

        rb = GetComponent<Rigidbody>();
        spawnPoint = gameObject.transform.position;

        canMove = true;

        frontRotation = view.transform.localRotation;
        backRotation = frontRotation * Quaternion.Euler(0, 180f, 0);
    }

    void FixedUpdate()
    {
        Vector3 currentVelocity = rb.velocity;

        if (canMove && Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * moveForce + Vector3.up * currentVelocity.y;
        }
        else
        {
            rb.velocity = Vector3.zero + Vector3.up * currentVelocity.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //for looking back
        Quaternion targetRotation = Input.GetKey(KeyCode.S) ? backRotation : frontRotation;

        view.transform.localRotation = Quaternion.RotateTowards(
                view.transform.localRotation,
            targetRotation,
    turnSpeed * Time.deltaTime
        );

        //turn left
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!isRotating)
            {
                StartCoroutine(RotateOverTime(-90f, 0.5f));
            }
        }

        //turn right
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isRotating)
            {
                StartCoroutine(RotateOverTime(90f, 0.5f));
            }
        }
    }

    IEnumerator RotateOverTime(float angle, float duration)
    {
        isRotating = true;
        Quaternion start = transform.rotation;
        Quaternion end = start * Quaternion.Euler(0, angle, 0);
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }

        transform.rotation = end;
        isRotating = false;
    }

    public void Respawn()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        //fades into black screen (turns black screen on)
        canMove = false;
        StartCoroutine(fadeInAndOut(blackScreen, true, 1f));

        yield return new WaitForSeconds(1f);
        gameObject.transform.position = spawnPoint;

        //fades out of  black screen (turns black screen off)
        StartCoroutine(fadeInAndOut(blackScreen, false, 1f));
        canMove = true;
    }

    IEnumerator fadeInAndOut(GameObject objectToFade, bool fadeIn, float duration)
    {
        float counter = 0f;

        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0;
            b = 1;

            //Debug.Log("a: " + a + ", b: " + b);
        }
        else
        {
            a = 1;
            b = 0;

            //Debug.Log("a: " + a + ", b: " + b);
        }
        int mode = 0;
        Color currentColor = Color.clear;

        SpriteRenderer tempSPRenderer = objectToFade.GetComponent<SpriteRenderer>();
        Image tempImage = objectToFade.GetComponent<Image>();
        RawImage tempRawImage = objectToFade.GetComponent<RawImage>();
        Text tempText = objectToFade.GetComponent<Text>();
        MeshRenderer tempRenderer = objectToFade.GetComponent<MeshRenderer>();

        //Check if this is a Sprite
        if (tempSPRenderer != null)
        {
            currentColor = tempSPRenderer.color;
            mode = 0;
        }
        //Check if Image
        else if (tempImage != null)
        {
            currentColor = tempImage.color;
            mode = 1;
        }
        //Check if RawImage
        else if (tempRawImage != null)
        {
            currentColor = tempRawImage.color;
            mode = 2;
        }
        //Check if Text 
        else if (tempText != null)
        {
            currentColor = tempText.color;
            mode = 3;
        }

        //Check if 3D Object
        else if (tempRenderer != null)
        {
            currentColor = tempRenderer.material.color;
            mode = 4;

            //ENABLE FADE Mode on the material if not done already
            tempRenderer.material.SetFloat("_Mode", 2);
            tempRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            tempRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            tempRenderer.material.SetInt("_ZWrite", 0);
            tempRenderer.material.DisableKeyword("_ALPHATEST_ON");
            tempRenderer.material.EnableKeyword("_ALPHABLEND_ON");
            tempRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            tempRenderer.material.renderQueue = 3000;
        }
        else
        {
            yield break;
        }

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            switch (mode)
            {
                case 0:
                    tempSPRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
                case 1:
                    tempImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
                case 2:
                    tempRawImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
                case 3:
                    tempText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
                case 4:
                    tempRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
            }
            yield return null;
        }
    }
}
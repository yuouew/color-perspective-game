using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSwitch : MonoBehaviour
{

    public GameObject ColoredLight;

    public bool blue;
    public bool red;

    public List<GameObject> blueObjects = new List<GameObject>();
    public List<GameObject> redObjects = new List<GameObject>();

    void Start()
    {
        ColoredLight.GetComponent<Light>().color = Color.white;

        redObjects.AddRange(GameObject.FindGameObjectsWithTag("red"));
        blueObjects.AddRange(GameObject.FindGameObjectsWithTag("blue"));

        foreach (GameObject obj in blueObjects)
        {
            obj.GetComponent<MeshCollider>().enabled = false;
            StartCoroutine(fadeInAndOut(obj, false, 1f));
        }

        foreach (GameObject obj in redObjects)
        {
            obj.GetComponent<MeshCollider>().enabled = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            ToggleBlue();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            ToggleRed();
        }
    }

    /// <summary>
    /// toggle blue
    /// make thing appear
    /// wall that fades in?
    ///
    /// 
    /// toggle red
    /// make thing dissapear
    /// 
    /// </summary>


    //if obj is currently fading, should collider be on
    // currently yes, collider is on while fading
    //red use is slower
    //blue use is faster
    //should depend on in and out
    //fade in means it should be in faster
    //fdae out should out faster


    public void ToggleBlue()
    {
        if (!blue && !red)
        {
            ColoredLight.GetComponent<Light>().color = Color.blue;
            StartCoroutine(RestoreOriginal());

            foreach (GameObject obj in blueObjects)
            {
                //turn mesh on
                obj.GetComponent<MeshCollider>().enabled = true;

                //fade in
                StartCoroutine(fadeInAndOut(obj, true, 1f));
            }

            blue = true;
        }
    }

    public void ToggleRed()
    {
        if (!red && !blue)
        {
            ColoredLight.GetComponent<Light>().color = Color.red;
            StartCoroutine(RestoreOriginal());

            foreach (GameObject obj in redObjects)
            {
                //obj is already on
                obj.GetComponent<MeshCollider>().enabled = false;

                //fade out
                StartCoroutine(fadeInAndOut(obj, false, 1f));
            }

            red = true;
        }
    }

    IEnumerator RestoreOriginal()
    {
        if (red)
        {
            foreach (GameObject obj in redObjects)
            {
                //
                yield return new WaitForSeconds(1f);
                obj.GetComponent<MeshCollider>().enabled = false;
            }
        }

        yield return new WaitForSeconds(2f);
        ColoredLight.GetComponent<Light>().color = Color.white;

        if (blue)
        {
            foreach (GameObject obj in blueObjects)
            {
                //fade out
                StartCoroutine(fadeInAndOut(obj, false, 1f));
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(FinishFade());
        }

        if (red)
        {
            foreach (GameObject obj in redObjects)
            {
                //fade in
                StartCoroutine(fadeInAndOut(obj, true, 1f));
                //turn obj back on
                obj.GetComponent<MeshCollider>().enabled = true;
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(FinishFade());
        }
    }

    /// <summary>
    /// Fade-out in 3 seconds
    /// StartCoroutine(fadeInAndOut(SpriteRend, false, 3f));
    /// 
    /// Fade-in in 3 seconds
    /// StartCoroutine(fadeInAndOut(SpriteRend, true, 3f));
    /// </summary>

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

    IEnumerator FinishFade()
    {
        if (blue)
        {
            foreach (GameObject obj in blueObjects)
            {
                obj.GetComponent<MeshCollider>().enabled = false;
            }
        }

        blue = false;
        red = false;

        yield return null;
    }
}
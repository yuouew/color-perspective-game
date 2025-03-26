using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitch : MonoBehaviour
{
    public Camera targetCamera; // Assign the camera in the Inspector
    public LayerMask redLayer; // Assign the layer in the Inspector
    public LayerMask blueLayer; // Assign the layer in the Inspector

    private int allLayer; // Store the original mask

    void Start()
    {
        // Store the original culling mask
        allLayer = targetCamera.cullingMask;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.J))
        {
            RestoreOriginalMask();
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            ToggleBlue();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            ToggleRed();
        }

    }

    public void ToggleBlue()
    {
        // Check if the layer is currently visible
        if ((targetCamera.cullingMask & blueLayer) != 0)
        {
            // If blue, remove blue
            targetCamera.cullingMask &= ~blueLayer;
        }
        else
        {
            // If not blue, add blue
            targetCamera.cullingMask |= blueLayer;
            targetCamera.cullingMask &= ~redLayer;

        }
    }

    public void ToggleRed()
    {
        // Check if the layer is currently visible
        if ((targetCamera.cullingMask & redLayer) != 0)
        {
            // If visible, remove it
            targetCamera.cullingMask &= ~redLayer;
        }
        else
        {
            // If not visible, add it
            targetCamera.cullingMask |= redLayer;
            targetCamera.cullingMask &= ~blueLayer;

        }
    }

    public void RestoreOriginalMask()
    {
        // Restore the original culling mask
        targetCamera.cullingMask = allLayer;
    }
}

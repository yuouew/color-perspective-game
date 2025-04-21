using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTrigger : MonoBehaviour
{
    public GameObject parent;

    private void OnTriggerEnter(Collider other)
    {
        if (parent.GetComponent<BlueEnemy>().stance == 0)
        {
            parent.GetComponent<BlueEnemy>().stance = 1;
        }
    }
}

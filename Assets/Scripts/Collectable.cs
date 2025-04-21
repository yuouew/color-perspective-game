using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("collectable"))
            {
                gameObject.SetActive(false);
                other.GetComponent<MovementDetector>().spawnPoint = transform.position;
                GameObject.Find("Collection Manager").GetComponent<CollectionManager>().RegisterCollectable();
            }

            if (gameObject.CompareTag("objective"))
            {
                Debug.Log("win");
            }
        }
    }
}

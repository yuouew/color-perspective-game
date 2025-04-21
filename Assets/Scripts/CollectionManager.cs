using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectionManager : MonoBehaviour
{
    private int totalCollectables;
    private int collectedCount;

    public TextMeshProUGUI collectableText;

    public GameObject objective;
    public GameObject blocker;


    void Start()
    {
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("collectable");
        totalCollectables = collectables.Length;
        collectedCount = 0;
    }

    public void RegisterCollectable()
    {
        collectedCount++;

        collectableText.text = $"Collected: {collectedCount}/{totalCollectables}";

        if (collectedCount >= totalCollectables)
        {
            blocker.SetActive(false);

            collectableText.text = "All collectables gathered!";
        }
    }
}

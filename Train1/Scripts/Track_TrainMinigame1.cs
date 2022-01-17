using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track_TrainMinigame1 : MonoBehaviour
{
    public bool isOnColumn = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Path"))
        {
            isOnColumn = true;
        }
    }
}

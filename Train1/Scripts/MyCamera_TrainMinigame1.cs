using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera_TrainMinigame1 : MonoBehaviour
{
    private BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.size = new Vector2(2 * ((Screen.width * 1.0f) / Screen.height) * GetComponent<Camera>().orthographicSize, 2 * GetComponent<Camera>().orthographicSize);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Path"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Balloon"))
        {
            GameController_TrainMinigame1.instance.SpawnCloudRiver();          
        }
    }
}

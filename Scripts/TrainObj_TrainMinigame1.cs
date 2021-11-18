using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainObj_TrainMinigame1 : MonoBehaviour
{
    public Camera mainCamera;
    public float offset;
    public bool isResumeSpawn = true;
    public Track_TrainMinigame1 trackLose;
    public int columnCount;

    private void Start()
    {
        offset = transform.position.x + mainCamera.transform.position.x;
        columnCount = 0;
    }

    public void RunComplete()
    {     
        isResumeSpawn = false;
        GameController_TrainMinigame1.instance.isMouseDown = false;
        float duration;
        if (transform.position.x - GameController_TrainMinigame1.instance.nextColumn1.transform.position.x < 5)
        {
            duration = 0.5f;
        }
        else
        {
            duration = 1;
        }
        columnCount++;
        GameController_TrainMinigame1.instance.isProgressUping = true;
        transform.DOMoveX(GameController_TrainMinigame1.instance.nextColumn1.transform.position.x, duration).SetEase(Ease.Linear).OnComplete(() =>
        {          
            if (columnCount == 8)
            {
                GameController_TrainMinigame1.instance.Win();
            }
            else
            {
                GameController_TrainMinigame1.instance.currentColumn = GameController_TrainMinigame1.instance.nextColumn1;
                GameController_TrainMinigame1.instance.nextColumn1 = GameController_TrainMinigame1.instance.nextColumn2;
                if (columnCount != 7)
                {
                    GameController_TrainMinigame1.instance.SpawnColumn();
                }
                GameController_TrainMinigame1.instance.SetUpPosSpawnTrack();

                mainCamera.transform.DOMoveX(transform.position.x - offset, 0.5f).OnComplete(() =>
                {
                    isResumeSpawn = true;
                    GameController_TrainMinigame1.instance.isSpawning = true;
                    GameController_TrainMinigame1.instance.isDropping = false;
                });
            }

        });

    }



    public void RunLose()
    {
        transform.DOMoveX(trackLose.transform.position.x + 2, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMoveY(transform.position.y - 10, 3);
            transform.DOShakeRotation(3);
            GameController_TrainMinigame1.instance.Lose();
        });
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudandRiver_TrainMinigame1 : MonoBehaviour
{


    private void Start()
    {
        if (GameController_TrainMinigame1.instance.isFirstCloud)
        {
            transform.DOLocalMoveX(transform.position.x - 30, 25).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(gameObject, 10);
            });
            GameController_TrainMinigame1.instance.isFirstCloud = false;
        }
        else
        {
            MoveCloudRiver();
        }
    }

    void MoveCloudRiver()
    {
        transform.DOLocalMoveX(transform.position.x - 40, 35).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject, 5);
        });
    }

}

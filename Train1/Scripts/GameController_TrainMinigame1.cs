using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_TrainMinigame1 : MonoBehaviour
{
    public static GameController_TrainMinigame1 instance;

    public Camera mainCamera;
    public List<Transform> listPosSpawnTrack = new List<Transform>();
    public List<Transform> listPosSpawnColumn = new List<Transform>();
    public GameObject columnPrefab;
    public GameObject trackPrefab;
    public bool isSpawning;
    public int indexSpawn;
    public bool isDropping;
    public TrainObj_TrainMinigame1 trainObj;
    public GameObject currentColumn;
    public GameObject nextColumn1;
    public GameObject nextColumn2;
    public bool isFirst, isWin, isLose;
    public Transform posFirst;
    public bool isMouseDown;
    public bool isSpawnColumnIndexMin;
    private int random;
    public CloudandRiver_TrainMinigame1 cloud_river_Prefab;
    public CloudandRiver_TrainMinigame1 cloudriverObj;
    public bool isFirstCloud;
    public GameObject tutorial;
    public Canvas canvas;
    public ProgressBar_PileDriverMinigame1 progressBar;
    public bool isProgressUping;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        isProgressUping = false;
    }

    private void Start()
    {
        SetSizeCamera();
        cloudriverObj = Instantiate(cloud_river_Prefab, Vector3.zero, Quaternion.identity);
        for (int v = 2; v < 9; v++)
        {
            canvas.transform.GetChild(0).GetChild(v).GetChild(0).gameObject.SetActive(false);
        }
        progressBar.current_progress = 0;
        progressBar.max_progress = 100;
        progressBar.Bar.fillAmount = progressBar.current_progress / progressBar.max_progress;
        isSpawning = true;
        isDropping = false;
        isMouseDown = false;
        isFirstCloud = true;
        isFirst = true;
        isWin = false;
        isLose = false;
        isSpawnColumnIndexMin = true;
        SetUpPosSpawnTrack();
        SetUpFirst();
        SpawnColumn();
        indexSpawn = 0;
        tutorial.SetActive(true);
    }

    void SetSizeCamera()
    {
        float f1;
        float f2;
        f1 = 16.0f / 9;
        f2 = Screen.width * 1.0f / Screen.height;

        mainCamera.orthographicSize *= f1 / f2;
    }

    void SetUpFirst()
    {
        nextColumn1 = Instantiate(columnPrefab, posFirst.position, Quaternion.identity);
    }

    public void SetUpPosSpawnTrack()
    {
        listPosSpawnTrack.Clear();
        listPosSpawnTrack.Add(currentColumn.transform.GetChild(0));
        for (int i = 0; i < currentColumn.transform.GetChild(0).childCount; i++)
        {
            listPosSpawnTrack.Add(currentColumn.transform.GetChild(0).GetChild(i));
        }
    }

    public void SpawnCloudRiver()
    {
        cloudriverObj = Instantiate(cloud_river_Prefab, cloudriverObj.transform.GetChild(0).transform.position, Quaternion.identity);
    }


    void SpawnTrack()
    {
        if (isSpawning)
        {
            indexSpawn++;
            if (indexSpawn == listPosSpawnTrack.Count)
            {
                DropTrack();
            }
            else
            {
                var tmpTrack = Instantiate(trackPrefab);
                tmpTrack.transform.position = listPosSpawnTrack[indexSpawn - 1].position;
                tmpTrack.transform.parent = currentColumn.transform.GetChild(0);
                tmpTrack.transform.DOMoveY(listPosSpawnTrack[indexSpawn].position.y, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    SpawnTrack();

                });
            }
        }
    }

    void DropTrack()
    {
        isDropping = true;
        indexSpawn = 0;
        isSpawning = false;
        var tmpPivot = currentColumn.transform.GetChild(0);
        tmpPivot.DORotate(new Vector3(tmpPivot.rotation.x, tmpPivot.rotation.y, -90), 2).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            if (currentColumn.transform.GetChild(0).GetChild(currentColumn.transform.GetChild(0).childCount - 1).GetComponent<Track_TrainMinigame1>().isOnColumn)
            {
                trainObj.RunComplete();
            }
            else if (!currentColumn.transform.GetChild(0).GetChild(currentColumn.transform.GetChild(0).childCount - 1).GetComponent<Track_TrainMinigame1>().isOnColumn)
            {
                trainObj.trackLose = currentColumn.transform.GetChild(0).GetChild(currentColumn.transform.GetChild(0).childCount - 1).GetComponent<Track_TrainMinigame1>();
                trainObj.RunLose();
            }
        });
    }

    public void SpawnColumn()
    {
        if (isSpawnColumnIndexMin)
        {
            isSpawnColumnIndexMin = false;
            nextColumn2 = Instantiate(columnPrefab, nextColumn1.transform.GetChild(1).GetChild(Random.Range(0, 3)).position, Quaternion.identity);
        }
        else if (!isSpawnColumnIndexMin)
        {
            random = Random.Range(0, nextColumn1.transform.GetChild(1).childCount);
            nextColumn2 = Instantiate(columnPrefab, nextColumn1.transform.GetChild(1).GetChild(random).position, Quaternion.identity);
            if (random == nextColumn1.transform.GetChild(1).childCount - 1 || random == nextColumn1.transform.GetChild(1).childCount - 2 || random == nextColumn1.transform.GetChild(1).childCount - 3)
            {
                isSpawnColumnIndexMin = true;
            }
        }
    }

    public void Lose()
    {
        isLose = true;
        Debug.Log("Thua");

    }

    public void Win()
    {
        isWin = true;
        Debug.Log("Win");
        mainCamera.DOOrthoSize(mainCamera.orthographicSize * 0.5f, 1);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isSpawning && trainObj.isResumeSpawn && !isWin && !isLose)
        {
            if (tutorial.activeSelf)
            {
                tutorial.SetActive(false);
            }
            isMouseDown = true;
            SpawnTrack();
        }
        if (Input.GetMouseButtonUp(0) && isSpawning && trainObj.isResumeSpawn && !isWin && !isLose)
        {
            if (isMouseDown)
            {
                if (!isDropping)
                {
                    DropTrack();
                }
            }
        }
        if (isProgressUping)
        {
            progressBar.current_progress += Time.deltaTime * 40;
            progressBar.Bar.fillAmount = progressBar.current_progress / progressBar.max_progress;
            if (progressBar.current_progress > (100f * trainObj.columnCount) / 8)
            {
                isProgressUping = false;
                progressBar.current_progress = (100f * trainObj.columnCount) / 8;
                if (!isWin)
                {
                    GameObject tmpStar = canvas.transform.GetChild(0).GetChild(trainObj.columnCount + 1).GetChild(0).gameObject;
                    tmpStar.SetActive(true);
                    tmpStar.transform.DOScale(1.5f, 0.5f).OnComplete(() =>
                    {
                        tmpStar.transform.DOScale(1, 0.5f);
                    });
                }
               

            }
        }
    }
}

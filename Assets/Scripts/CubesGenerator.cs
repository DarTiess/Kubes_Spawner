using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesGenerator : MonoBehaviour
{
   public enum ParamType
    {
        Speed,
        Distance,
        Timer
    }
    [Header("Push Settings")]
    [SerializeField] private List<GameObject> cubesPrefabs;
    [SerializeField] private int listSize;
    private List<GameObject> cubesList = new List<GameObject>();
    int cubeIndex = 0;
    [SerializeField] private float generateTimer;
    float timer = 0;
    [SerializeField] private float roadWidth;
    float roadPart;
    [SerializeField] private float speedStep;
    [SerializeField] private float distanceStep;
    [SerializeField] private float timerStep;
    [SerializeField] private Transform distanceLimit;
    float distLimit;
    [Header("Cube's Settings")]
    [SerializeField] private float destination;
    [SerializeField] private float speed;
    [SerializeField] private float stopDistance;
    float rndXpos;
    int rndCube;
   
  
    void Start()
    {

        roadPart = roadWidth / 2;
        distLimit = Vector3.Distance(transform.position, distanceLimit.position);
        InitializeCubesList(destination, speed, stopDistance);
        CanvasController.Instance._SpeedMinus += SpeedMinus;
        CanvasController.Instance._SpeedPlus += SpeedPlus;
        CanvasController.Instance._DistancedPlus += DistancePlus;
        CanvasController.Instance._DistanceMinus += DistanceMinus;
        CanvasController.Instance._TimerMinus += TimerMinus;
        CanvasController.Instance._TimerPlus += TimerPlus;

        CanvasController.Instance.InitializeCanvas(speed,speedStep, destination,distanceStep, distLimit, generateTimer, timerStep);
    }

 

    void Update()
    {
        if (timer < generateTimer)
        {
            timer += Time.deltaTime;
            return;
        }
        PushCube();
    }

    
    void PushCube()
    {
        if (cubeIndex >= cubesList.Count)
        {
            cubeIndex = 0;
        }
        rndXpos = Random.Range(-roadPart, roadPart);
        cubesList[cubeIndex].transform.position =new Vector3(transform.position.x - rndXpos, transform.position.y, transform.position.z);
        cubesList[cubeIndex].GetComponent<CubesController>().InitCubeSettings(destination, speed, stopDistance);
        cubesList[cubeIndex].SetActive(true);
        cubeIndex++;
        timer = 0;
        return;
    }
   
    void InitializeCubesList(float dest, float speed, float stopDistance)
    {
        for(int i = 0; i < listSize; i++)
        {
            rndCube = Random.Range(0, cubesPrefabs.Count);
            GameObject cube = Instantiate(cubesPrefabs[rndCube], gameObject.transform);
          
            cube.SetActive(false);
            cubesList.Add(cube);
        }
    }
 

    void MinusParametr(ref float currentValue, float step)
    {
        currentValue -= step;
    } 
    void PlusParametr(ref float currentValue, float step)
    {
        currentValue += step;
    }
    private void TimerPlus()
    {
        PlusParametr(ref generateTimer, timerStep);
    }

    private void TimerMinus()
    {
        MinusParametr(ref generateTimer, timerStep);
    }

    private void DistanceMinus()
    {
        MinusParametr(ref destination, distanceStep);
    }

    private void DistancePlus()
    {
        PlusParametr(ref destination, distanceStep);
    }

    private void SpeedPlus()
    {
        PlusParametr(ref speed, speedStep);
    }

    private void SpeedMinus()
    {
        MinusParametr(ref speed, speedStep);
    }


}

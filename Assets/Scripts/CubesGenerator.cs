using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesGenerator : MonoBehaviour
{
  
    [Header("Push Settings")]
    [SerializeField] private List<GameObject> cubesPrefabs;
    [SerializeField] private int listSize;
    private List<GameObject> cubesList = new List<GameObject>();
    int cubeIndex = 0;
    [SerializeField] private float generateTimer;
    [HideInInspector] public float timerValue { get { return generateTimer; } }
    float timer = 0;
    [SerializeField] private float roadWidth;
    float roadPart;
   
    [SerializeField] private Transform distanceLimit;
    float distLimit;
    [HideInInspector] public float distLimitValue { get { return distLimit = Vector3.Distance(transform.position, distanceLimit.position); } }

  
    [Header("Cube's Settings")]
    [SerializeField] private float destination;
    [HideInInspector] public float distValue { get { return destination; } }


    [SerializeField] private float speed;
   [HideInInspector]public float speedValue { get { return speed; } }
    [SerializeField] private float stopDistance;
    float rndXpos;
    int rndCube;

  

    private void Start()
    {
        roadPart = roadWidth / 2;
        distLimit = Vector3.Distance(transform.position, distanceLimit.position);
        InitializeCubesList(destination, speed, stopDistance);
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
 
    public void ChangeSpeed(float value)
    {
        speed = value;
    }
    public void ChangeTimer(float value)
    {
        generateTimer = value;
    }

    public void ChangeDistance(float value)
    {
        destination = value;
    }

}

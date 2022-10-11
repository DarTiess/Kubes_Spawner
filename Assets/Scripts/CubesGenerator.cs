using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesGenerator : MonoBehaviour
{
    [Header("List Settings")]
    [SerializeField] private List<GameObject> cubesPrefabs;
    [SerializeField] private int listSize;
    private List<GameObject> cubesList = new List<GameObject>();
    int cubeIndex = 0;
    [SerializeField] private float generateTimer;
    float timer = 0;

    [Header("Cube's Settings")]
    [SerializeField] private float destination;
    [SerializeField] private float speed;
    [SerializeField] private float stopDistance;
    float rndXpos;
    int rndCube;
    void Start()
    {
        InitializeCubesList(destination, speed, stopDistance);
    }

    // Update is called once per frame
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
        rndXpos = Random.Range(-5.0f, 5.0f);
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

}

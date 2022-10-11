using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CubesController : MonoBehaviour
{

    NavMeshAgent navMesh;
   
   
    Vector3 destPosition;
    private float stopDistance;
   
    // Start is called before the first frame update
    void Start()
    {
       
      
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void InitCubeSettings(float destination, float speed, float stopDist)
    {
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = speed;
        destPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + destination);
        stopDistance = stopDist;
    }
    private void Move()
    {
        navMesh.SetDestination(destPosition);
        if (Vector3.Distance(transform.position, destPosition) < stopDistance)
        {
         StopMove();
        }
    }
    void StopMove()
    {
        navMesh.isStopped = true;
        gameObject.SetActive(false);
       
    }

}

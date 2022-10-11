using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CubesController : MonoBehaviour
{

    NavMeshAgent navMesh;
   
    [SerializeField] private float destination;
    Vector3 destPosition;
    Vector3 startPosition;
    [SerializeField] private float speed;
    [SerializeField] private float stopDistance;
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = speed;
        destPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + destination);
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
       // gameObject.SetActive(false);
        StartCoroutine(RepeatMove());
    }

    IEnumerator RepeatMove()
    {
        yield return new WaitForSeconds(1f);
        gameObject.transform.position = startPosition;
        navMesh.isStopped = false;
    }
}

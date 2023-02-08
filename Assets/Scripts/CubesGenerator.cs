using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

enum DownloadType
{
    FromPrefabs,
    AssetBundles,
    Addressables
}
public class CubesGenerator : MonoBehaviour
{

    [Header("Push Settings")]
    [SerializeField] private DownloadType downloadType;
    [SerializeField] private List<GameObject> cubesPrefabs;
    [SerializeField] private int listSize;
    [SerializeField] private float generateTimer;
    [SerializeField] private float roadWidth;
    [SerializeField] private Transform distanceLimit;
    [Header("Cube's Settings")]
    [SerializeField] private float destination;
    [SerializeField] private float speed;
    [SerializeField] private float stopDistance;
    [Header("Adressables key")]
    [SerializeField] private List<string> _keysList;

    float timer = 0;
    float roadPart;
    float distLimit;
    float rndXpos;
    int rndCube;
    private List<GameObject> cubesList = new List<GameObject>();
    List<AsyncOperationHandle> _handlers = new List<AsyncOperationHandle>();

    [HideInInspector] public float timerValue { get { return generateTimer; } }
    [HideInInspector] public float distLimitValue { get { return distLimit = Vector3.Distance(transform.position, distanceLimit.position); } }
    [HideInInspector] public float distValue { get { return destination; } }
    [HideInInspector] public float speedValue { get { return speed; } }

    private void Start()
    {
        roadPart = roadWidth / 2;
        distLimit = Vector3.Distance(transform.position, distanceLimit.position);
        InitializeCubesList();
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
        if (cubesList.Count <= 0)
        {
            return;
        }

        rndXpos = UnityEngine.Random.Range(-roadPart, roadPart);
        for (int i = 0; i < cubesList.Count; i++)
        {
            if (!cubesList[i].activeInHierarchy)
            {
                cubesList[i].transform.position = new Vector3(transform.position.x - rndXpos, transform.position.y, transform.position.z);
                cubesList[i].GetComponent<CubesController>().InitCubeSettings(destination, speed, stopDistance);
                cubesList[i].SetActive(true);
               timer = 0; 
                return;
            }
        }

          timer = 0; 
       
        return;
    }

    void InitializeCubesList()
    {
        for (int i = 0; i < listSize; i++)
        {
            switch (downloadType)
            {
                case DownloadType.FromPrefabs:
                    GameObject cube = null;
                    rndCube = UnityEngine.Random.Range(0, cubesPrefabs.Count);
                    cube = Instantiate(cubesPrefabs[rndCube], gameObject.transform);
                    cube.SetActive(false);
                    cubesList.Add(cube);
                    break;
                case DownloadType.AssetBundles:
                    StartCoroutine(DownloadAssetBundlesFromServer());
                    break;
                case DownloadType.Addressables:
                     LoadPrefabsFromAdressables();
                    break;
            }

        }
    }
 
    public async void LoadPrefabsFromAdressables()
    {
         rndCube = UnityEngine.Random.Range(0, _keysList.Count);
        var loadAssync=Addressables.LoadAssetAsync<GameObject>(_keysList[rndCube]);
       _handlers.Add(loadAssync);
      
                 
        await loadAssync.Task;
            InstantiateGameObjectFromAssets(loadAssync.Result);           

    }

    private void OnDisable()
    {
        foreach (var handler in _handlers)
        {
            Addressables.Release(handler);
        }
    }
    private IEnumerator DownloadAssetBundlesFromServer()
    {
        GameObject obj = null;

        string url = "https://drive.google.com/u/0/uc?id=1itiMfzCl-W5jiFK6H0p48ImomYRNbEwR&export=download";

        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning("Error detecting: " + url + " " + www.error);
            }
            else
            {

                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                int rnd = UnityEngine.Random.Range(0, bundle.GetAllAssetNames().Length);
                obj = bundle.LoadAsset(bundle.GetAllAssetNames()[rnd]) as GameObject;
                bundle.Unload(false);

                yield return new WaitForEndOfFrame();
            }
            www.Dispose();
        }
        yield return new WaitForSeconds(1f);
        InstantiateGameObjectFromAssets(obj);
    }

    private void InstantiateGameObjectFromAssets(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("your assetBundle is empty");
        }
        else
        {
            GameObject newObj = Instantiate(obj);
            newObj.transform.position = gameObject.transform.position;
            newObj.SetActive(false);
            cubesList.Add(newObj);
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

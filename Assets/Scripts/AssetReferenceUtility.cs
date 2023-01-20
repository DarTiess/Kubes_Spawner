using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class AssetReferenceUtility : MonoBehaviour
{
    public AssetReference objectToLoad;
    public AssetReference accesoryObjectToLoad;
    private GameObject instantiateObject;
    private AsyncOperationHandle<GameObject> objectOperation;
    private AsyncOperationHandle<GameObject> accesoryObjectOperation;
    // Start is called before the first frame update
    void Start()
    {
        objectOperation = Addressables.LoadAssetAsync<GameObject>(objectToLoad);
        objectOperation.Completed+= ObjectLoadDone;



    }

    private void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject loadedObject = obj.Result;
            Debug.Log("Successfully loaded object");
            instantiateObject = Instantiate(loadedObject);
            Debug.Log(" Success instantiated obj");
            if (accesoryObjectToLoad != null)
            {
                accesoryObjectOperation = accesoryObjectToLoad.InstantiateAsync(instantiateObject.transform);
                accesoryObjectOperation.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        Debug.Log("Well done");
                    }
                };
            }
        }
    }

    private void OnDestroy()
    {
        if (accesoryObjectOperation.IsValid())
        {
            DestroyObject(accesoryObjectOperation);
        }

        if (objectOperation.IsValid())
        {
            DestroyObject(objectOperation);
        }

        Destroy(instantiateObject);
        Debug.Log("destroyed inst object");
    }

    void DestroyObject(AsyncOperationHandle<GameObject> obj)
    {
        Addressables.Release(obj);
        Debug.Log("released "+ obj+" load operation");
    }
}

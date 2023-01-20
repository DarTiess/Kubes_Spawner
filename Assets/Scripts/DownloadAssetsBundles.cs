using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadAssetsBundles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadAssetBundlesFromServer());
    }


    private IEnumerator DownloadAssetBundlesFromServer()
    {
        GameObject obj= null;

        string url="https://drive.google.com/u/0/uc?id=1s5gPWKMsfpCM_1iFg_9ATzF-pCGfxk_9&export=download";

        using(UnityWebRequest www= UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return www.SendWebRequest();
            if(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) 
            {
                Debug.LogWarning("Error detecting: "+ url+" "+ www.error);
            }
            else
            {
                AssetBundle bundle= DownloadHandlerAssetBundle.GetContent(www);
                obj=bundle.LoadAsset(bundle.GetAllAssetNames()[0]) as GameObject;
                bundle.Unload(false);

                yield return new WaitForEndOfFrame();
            }
            www.Dispose();
        }
        InstantiateGameObjectFromAssets(obj);

    }

    private void InstantiateGameObjectFromAssets(GameObject obj)
    {
       if(obj == null)
        {
            Debug.LogWarning("your assetBundle is empty");
        }
        else
        {
            GameObject newObj=Instantiate(obj);
            newObj.transform.position=Vector3.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

enum TypeOfBundle
{
    Unknow,
    GameObject,
    Texture2D,
    AudioClip
}
public class DownloadAssetBundlesByTypes : MonoBehaviour
{

    [SerializeField]private Image _image;
    [SerializeField]private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        LoadAssetBundles(ActionWantedToAssetBundle);
    }

    private void LoadAssetBundles(CallbackDelegate<dynamic, TypeOfBundle> callbackFunction, string assetByndleName = "")
    {
        StartCoroutine(DownloadAssetBundlesFromServer(callbackFunction, assetByndleName));
    }
     private IEnumerator DownloadAssetBundlesFromServer(CallbackDelegate<dynamic, TypeOfBundle> callbackFunction, string assetByndleName="")
    {
        dynamic assetBundleLoad= null;
        TypeOfBundle typereceived= TypeOfBundle.Unknow;

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
                assetBundleLoad=bundle.LoadAsset(bundle.GetAllAssetNames()[0]);
                bundle.Unload(false);

                yield return new WaitForEndOfFrame();
            }
            www.Dispose();
        }
      
        Debug.Log("The asset bundle unloaded is type of "+ assetBundleLoad);
        typereceived=(TypeOfBundle)CheckAssetBundleLoadType(assetBundleLoad);
        callbackFunction(assetBundleLoad, typereceived);
    }

    private int CheckAssetBundleLoadType( dynamic assetBundleLoad)
    {
        TypeOfBundle typeOfBundle= TypeOfBundle.Unknow;

       if(assetBundleLoad is GameObject)
        {
            typeOfBundle = TypeOfBundle.GameObject;
        }
       else if(assetBundleLoad is Texture2D)
        {
            typeOfBundle= TypeOfBundle.Texture2D;
        }
       else if (assetBundleLoad is AudioClip) 
       {
            typeOfBundle= TypeOfBundle.AudioClip;
       }

       return (int)typeOfBundle;
    }

    private void ActionWantedToAssetBundle(dynamic assetDownload, TypeOfBundle typeOfBundle)
    {
        switch (typeOfBundle)
        {
            case TypeOfBundle.Unknow:
                Debug.Log("Has NO type");
                break;
            case TypeOfBundle.GameObject:
                InstantiateGameObjectFromAssetBundle(assetDownload as GameObject);
                break;
            case TypeOfBundle.Texture2D:
                ApplyImageFromAssetBundle(assetDownload as Sprite);
                break;
            case TypeOfBundle.AudioClip:
                PlayAudioClipFromAssetBundles(assetDownload as AudioClip);
                break;
            default:
                break;
        }
    }

    private void ApplyImageFromAssetBundle(Sprite sprite)
    {
        _image.sprite= sprite;
        _image.type=Image.Type.Simple;
        _image.preserveAspect=true;
    }
    private void PlayAudioClipFromAssetBundles(AudioClip audioClip)
    {
        _audioSource.clip= audioClip;
        _audioSource.Play();
    }

    private void InstantiateGameObjectFromAssetBundle(GameObject gameObject)
    {
        GameObject newObj=Instantiate(gameObject);
        newObj.transform.position= transform.position;
    }
}

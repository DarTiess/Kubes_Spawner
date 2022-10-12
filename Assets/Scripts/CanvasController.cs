using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
  
   
    float speed=0;
    [SerializeField] private InputField speedInput;
    [SerializeField] private InputField distanceInput;
    [SerializeField] private InputField timerInput;
    
 
    float distance=0;
    private float distanceLimit;

    
    float timer=0;
   
    [SerializeField] private CubesGenerator cubesGenerator;

 
    // Start is called before the first frame update
    void Start()
    {
        speed = cubesGenerator.speedValue;
        distance = cubesGenerator.distValue;
        timer = cubesGenerator.timerValue;
        distanceLimit = cubesGenerator.distLimitValue;
        Debug.Log(distanceLimit);
        speedInput.text = speed.ToString();
        speedInput.onEndEdit.AddListener(ChangeSpeed); 
        
        distanceInput.text = distance.ToString();
        distanceInput.onEndEdit.AddListener(ChangeDistance); 
        
        timerInput.text = timer.ToString();
        timerInput.onEndEdit.AddListener(ChangeTimer);
    }

    private void ChangeTimer(string arg)
    {
        if (float.Parse(arg, CultureInfo.GetCultureInfo("en-US"))> 0)
        {
            timer = float.Parse(arg, CultureInfo.GetCultureInfo("en-US"));
        }
        else
        {
            ChangeToZeroValue(ref timer, timerInput);
        }
        cubesGenerator.ChangeTimer(timer);
    }

    private void ChangeDistance(string arg)
    {
        if(distanceLimit > float.Parse(arg, CultureInfo.GetCultureInfo("en-US")))
        {
           distance= float.Parse(arg, CultureInfo.GetCultureInfo("en-US"));
        }
        else
        {
            distance = distanceLimit;
            distanceInput.text = distance.ToString();
        }
        cubesGenerator.ChangeDistance(distance);
    }

    private void ChangeSpeed(string arg)
    {
        if (float.Parse(arg, CultureInfo.GetCultureInfo("en-US") )> 0)
        {
            speed = float.Parse(arg, CultureInfo.GetCultureInfo("en-US"));
        }
        else
        {
            ChangeToZeroValue(ref speed, speedInput);
        }
        cubesGenerator.ChangeSpeed(speed);
    }

    void ChangeToZeroValue(ref float param, InputField inputField)
    {
        param = 0;
        inputField.text = param.ToString();
    }
 
   
}

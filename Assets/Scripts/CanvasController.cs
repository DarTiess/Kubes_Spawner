using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance;
    [Header("Speed")]
    [SerializeField] private Text speedTxt;
    float speed=0;
    [SerializeField] private Button speedBtnMinus;
    [SerializeField] private Button speedBtnPlus;
     private float speedStep;
    [Header("Distance")]
    [SerializeField] private Text distanceTxt;
    float distance=0;
    [SerializeField] private Button distanceBtnMinus;
    [SerializeField] private Button distanceBtnPlus;
     private float distanceStep;
    private float distanceLimit;
    [SerializeField] private Text timerTxt;
    float timer=0;
    [SerializeField] private Button timerBtnMinus;
    [SerializeField] private Button timerBtnPlus;
    private float timerStep;

    public Action _SpeedMinus;
    public Action _SpeedPlus;
    public Action _DistanceMinus;
    public Action _DistancedPlus;
    public Action _TimerMinus;
    public Action _TimerPlus;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        speedBtnMinus.onClick.AddListener(SpeedMinus);
        speedBtnPlus.onClick.AddListener(SpeedPlus);
        distanceBtnMinus.onClick.AddListener(DistanceMinus);
        distanceBtnPlus.onClick.AddListener(DistancePlus);
        timerBtnMinus.onClick.AddListener(TimerMinus);
        timerBtnPlus.onClick.AddListener(TimerPlus);
    }

    public void InitializeCanvas(float speed,float speedStep, float dist, float distStep, float limitDist, float time, float timerStep)
    {
        this.speed = speed;
        distance = dist;
        timer = time;

        this.speedStep = speedStep;
        distanceStep = distStep;
       this.timerStep = timerStep;
        distanceLimit = limitDist;
        speedTxt.text = speed.ToString();
        distanceTxt.text = distance.ToString();
        timerTxt.text = timer.ToString();
    }
    void SpeedMinus()
    {
        MakeMinus(ref speed, speedStep, speedTxt);
        _SpeedMinus?.Invoke();
    }
    void SpeedPlus()
    {
        MakePlus(ref speed, speedStep, speedTxt);
        _SpeedPlus?.Invoke();
    }
    void DistanceMinus()
    {
        MakeMinus(ref distance, distanceStep, distanceTxt);
        _DistanceMinus?.Invoke();
    }
    void DistancePlus()
    {
        if (distance < distanceLimit)
        {
            MakePlus(ref distance, distanceStep, distanceTxt);
            _DistancedPlus?.Invoke();
        }
    }
    void TimerMinus()
    {
        MakeMinus( ref timer, timerStep, timerTxt);
        _TimerMinus?.Invoke();
    }
    void TimerPlus()
    {
        MakePlus(ref timer, timerStep, timerTxt);
        _TimerPlus?.Invoke();
    }


    void MakeMinus(ref float currentValue, float stepValue, Text text)
    {
        if (currentValue > 0)
        {
            currentValue -= stepValue;
            text.text = currentValue.ToString();
        }
      
    }
    
    void MakePlus(ref float currentValue, float stepValue, Text text)
    {
        currentValue += stepValue;
        text.text = currentValue.ToString();
    }
   
}

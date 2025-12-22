using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorSliding : MonoBehaviour
{
    public bool IsOpen = false;
    [Header("Sliding door")]
    [SerializeField] Vector3 SlideDirection = Vector3.back;
    [SerializeField] private float SlideAmount = 1.9f;
    [SerializeField] private float speed = 1.0f;

    private bool IsRotatingDoor = true;
    [Header("Rotaing Door")]
    [SerializeField] private float RotationAmount = 1.0f;
    [SerializeField] private float ForwardDirection = 0;
 
    private Vector3 StartPosition;
    private Vector3 StartRotation;
    private Vector3 Forward;
    
    private Coroutine AnimationCoroutine;
    public void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        Forward = transform.right;
        StartPosition = transform.position;
    }
    public void Open(Vector3 UserPosition)
    {
        if (!IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            if (IsRotatingDoor)
            {
                float dot = Vector3.Dot(Forward, (UserPosition - transform.position).normalized);
                Debug.Log($"Dot: {dot.ToString("N3")}");
                AnimationCoroutine = StartCoroutine(DoSlidingOpen());
            }
            else
                AnimationCoroutine = StartCoroutine(DoSlidingOpen());
        }
    }
    private IEnumerator DoSlidingOpen()
    {
        Vector3 endPosition = StartPosition + SlideAmount * SlideDirection;
        Vector3 startPosition = transform.position;

        float time = 0;
        IsOpen = true;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    
   
    public void Close()
    {
        if (IsOpen) 
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            if (IsRotatingDoor)
            {
                AnimationCoroutine = StartCoroutine(DoSlidingClose());
            }
            else
            {
                AnimationCoroutine = StartCoroutine(DoSlidingClose());
            }
        }
       
    }
   
    private IEnumerator DoSlidingClose()
    {
        Vector3 endPosition = StartPosition;
        Vector3 startPosition = transform.position;
        float time = 0;
        IsOpen = false;

        while (time < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}

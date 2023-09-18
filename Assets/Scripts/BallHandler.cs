using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private int chances = 3;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float respanDelay;
    [SerializeField] private float delayDuration = 1f;
    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool isDraging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }
    private void OnEnable() 
    {
        EnhancedTouchSupport.Enable();
    }
    private void OnDisable() 
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if(chances == 0)
        {
            SceneManager.LoadScene(0);
        }
        if(currentBallRigidbody == null) {return;}
        if(Touch.activeTouches.Count == 0) 
        {
            if(isDraging)
            {
                LaunchBall();
            }
            isDraging = false;
            return;
        }
        isDraging = true;
        Vector2 touchPosition = new Vector2();
        foreach(Touch touch in Touch.activeTouches)
        {
            touchPosition += touch.screenPosition;
        }
        touchPosition /= Touch.activeTouches.Count;
       
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        currentBallRigidbody.isKinematic = true;
        currentBallRigidbody.position = worldPosition;
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
        currentBallSpringJoint.connectedBody = pivot;
        chances --;
    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;
        Invoke("DetachBall", delayDuration);
      
    }
    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respanDelay);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D currentBallRigidbody;
    [SerializeField] private SpringJoint2D currentBallSpringJoint;
    [SerializeField] private float delayDuration = 1f;
    private Camera mainCamera;
    private bool isDraging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBallRigidbody == null) {return;}
        if(!Touchscreen.current.primaryTouch.press.isPressed) 
        {
            if(isDraging)
            {
                LaunchBall();
            }
            isDraging = false;
            return;
        }
        isDraging = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        currentBallRigidbody.isKinematic = true;
        currentBallRigidbody.position = worldPosition;
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
    }
}

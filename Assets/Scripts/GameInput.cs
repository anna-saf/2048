using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    [SerializeField] private float _minDistanceForSwipe = 20f;

    public static GameInput Instance { get; private set; }

    public event EventHandler<SwipeData> OnSwipeAction;

    private Vector2 fingerUpPos;
    private Vector2 fingerDownPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            SwipeDetectLogic();
        }
    }

    private void SwipeDetectLogic()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPos = touch.position;
                //fingerUpPos = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerUpPos = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerDownPos.y - fingerUpPos.y > 0 ? SwipeDirection.Down : SwipeDirection.Up;
                SendSwipe(direction);
            }
            else
            {
                var direction = fingerDownPos.x - fingerUpPos.x > 0 ? SwipeDirection.Left : SwipeDirection.Right;
                SendSwipe(direction);
            }
        }
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > _minDistanceForSwipe || HorizontalMovementDistance() > _minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            direction = direction,
            startPos = fingerDownPos,
            endPos = fingerUpPos,
        };
        OnSwipeAction?.Invoke(this,swipeData);
    }

    protected void OnDestroy()
    {

    }
}

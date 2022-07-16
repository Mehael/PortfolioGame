using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class ExtensionMethods
{
    private const float DefaultMovementSpeed = 5;

    public static IEnumerator MoveTo(this Transform transform, Vector3 target, float speed = DefaultMovementSpeed, 
        Action callback = null)
    {
        var start = transform.position;
        var t = 0f;
        while (t <= 1.0f) {
            t += Time.deltaTime * speed; // Goes from 0 to 1, incrementing by step each time
            if (transform == null)
                yield break;
            
            transform.position = Vector3.Lerp(start, target, t);
            yield return new WaitForEndOfFrame();       
        }
        
        if (transform != null)
            transform.position = target;

        callback?.Invoke();
    }
    
    public static IEnumerator LocalMoveTo(this Transform transform, Vector3 target, float speed = DefaultMovementSpeed)
    {
        var start = transform.localPosition;
        var t = 0f;
        while (t <= 1.0f) {
            t += Time.deltaTime * speed; 
            if (transform == null)
                yield break;
            
            transform.localPosition = Vector3.Lerp(start, target, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        transform.localPosition = target;
    }

    private static List<Transform> WobblesList = new List<Transform>();
    private const float DefaultWobbleSpeed = 2;
    public static IEnumerator Wobble(this Transform transform, float growPercent = 0.2f, float speed = DefaultWobbleSpeed)
    {
        if (WobblesList.Contains(transform))
            yield break;
        
        WobblesList.Add(transform);
        var baseScale = transform.localScale;
        var newScale = new Vector3(
            baseScale.x * (1 - growPercent), 
            baseScale.y * (1 + growPercent), 
            baseScale.y);
        
        yield return transform.ScaleFromTo(baseScale,newScale, speed);
        yield return transform.ScaleFromTo(newScale, baseScale, speed);
        WobblesList.Remove(transform);
    }
    
    private const float DefaultScaleSpeed = 50; 
    public static IEnumerator ScaleFromTo(this Transform transform, Vector3 start, Vector3 end, float speed = DefaultScaleSpeed)
    {
        var t = 0f;
        while (t <= 1.0f) {
            t += Time.deltaTime * speed;
            if (transform == null)
                yield break;
            
            transform.localScale = Vector3.Lerp(start, end, t); 
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = end;
    }

    public static IEnumerator ScaleAppear(this Transform transform, Vector3 targetScale, float speed = DefaultScaleSpeed)
    {
        return transform.ScaleFromTo(Vector3.zero, targetScale, speed);
    }
    
    public static IEnumerator ScaleHide(this Transform transform, float speed = DefaultScaleSpeed)
    {
        return transform.ScaleFromTo(transform.localScale, Vector3.zero, speed);
    }
    
    public static Color GetRandomColor()
    {
        return new Color(
            Random.value,
            Random.value,
            Random.value
        );
    }
}
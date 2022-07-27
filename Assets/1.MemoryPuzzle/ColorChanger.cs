using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class ColorChanger : MonoBehaviour
{
    public VolumeProfile PostProcessing;

    private IEnumerator Start()
    {
        var check = PostProcessing.TryGet<Bloom>(out var bloom);
        if (!check)
            yield break;

        while (true) {
            var timer = Random.Range(.5f, 5f);
            var t = 0f;
            var nextColor = Random.ColorHSV(0,1,.5f,.8f);
            var currentColor = bloom.tint.value;
            while (t < timer)
            {
                bloom.tint.value = Color.Lerp(currentColor, nextColor, t / timer);
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }  
        }

        
    }
}

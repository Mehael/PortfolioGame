using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MemoryPuzzle;
using UnityEngine;

namespace MemoryPuzzle
{
    public class RuneAnimator : MonoBehaviour
    {
        private static readonly int DissolveTime = Shader.PropertyToID("_DissolveTime");
        private const float DissolveSpeed = 1f; 
        private const float DelayBeforeDissolve = .3f; 
    
        IEnumerator Start()
        {
            var scaler = StartCoroutine(transform.ScaleFromTo(Vector3.one * .7f,Vector3.one * 1.5f, 1.5f));
            yield return new WaitForSeconds(DelayBeforeDissolve);
        
            var materials = GetComponentsInChildren<Renderer>().SelectMany(r => r.materials).ToArray();
            var t = 0f;
            while (t <= 1)
            {
                t += DissolveSpeed * Time.deltaTime;
                foreach (var material in materials)
                    material.SetFloat(DissolveTime, t);
                yield return new WaitForEndOfFrame();
            }
        
            yield return scaler;
            Destroy(gameObject);
        }
    }
}


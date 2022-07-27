using System;
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
        private Material[] materials;
        
        public void Awake()
        {
            materials = GetComponentsInChildren<Renderer>().SelectMany(r => r.materials).ToArray();
            SetVisibility(false);
        }

        public void Interact()
        {
            StopAllCoroutines();
            SetVisibility(true);
            StartCoroutine(Bounce());
            
            IEnumerator Bounce() {
                var routine = StartCoroutine(transform.ScaleFromTo(Vector3.one * .7f, Vector3.one * 1.5f, 1.5f));
                yield return new WaitForSeconds(DelayBeforeDissolve);
                StartCoroutine(Dissolve());
                yield return routine;
            }
        }

        public void Highlight(int radius, float time)
        {
            StopAllCoroutines();
            SetVisibility(true);
            StartCoroutine(Light());
            
            IEnumerator Light() {
                var baseScale = Mathf.Pow(.8f,radius) * Vector3.one;
                StartCoroutine(transform.ScaleFromTo(baseScale * .5f, baseScale, 1f));
                yield return new WaitForSeconds(time);
                yield return StartCoroutine(Dissolve());
            }    
        }
        
        IEnumerator Dissolve() {
            var t = 0f;
            while (t <= 1) {
                t += DissolveSpeed * Time.deltaTime;
                if (t > 1) t = 1;
                foreach (var material in materials)
                    material.SetFloat(DissolveTime, t);
                yield return new WaitForEndOfFrame();
            }
        }

        void SetVisibility(bool isVisible)
        {
            foreach (var material in materials)
                material.SetFloat(DissolveTime, isVisible ? 0 : 1);
        }
    }
}


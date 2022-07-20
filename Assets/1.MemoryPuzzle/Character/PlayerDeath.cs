using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    public class PlayerDeath : MonoBehaviour
    {
        public ParticleSystem DeathVFX;
        public MaskingController MaskingController;
        
        private PlayerController controller;
        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            controller.OnDeath += () => StartCoroutine(Die());
        }
        
        private IEnumerator Die()
        {
            DeathVFX.Play();
            var longest = StartCoroutine(MaskingController.PlayShine());
            AudioSystem.Play("FallToHole");
            StartCoroutine(transform.ScaleHide(1f));
            yield return longest;
            LevelLoader.RestartLevel();
        }
    } 

}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        public ParticleSystem DeathVFX;
        public FieldHighlightController HighlightController;
        public RuneAnimator FallRune;
        
        private Animator animator;
        private PlayerController controller;
        
        private Vector3 prevPosition;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int DieTrigger = Animator.StringToHash("Die");
        private static readonly int VictoryTrigger = Animator.StringToHash("Victory");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<PlayerController>();
            controller.OnDeath += () => StartCoroutine(Die());
            controller.OnVictory += () => StartCoroutine(Victory());
            prevPosition = transform.position;
        }
        
        private void Update()
        {
            var delta = transform.position - prevPosition;
            animator.SetBool(IsMoving, controller.IsInMovement);
            prevPosition = transform.position;
            if (controller.IsInMovement)
                transform.LookAt(transform.position + delta, Vector3.forward);
        }

        private IEnumerator Die()
        {
            Instantiate(FallRune, transform.position, FallRune.transform.rotation).Interact();
            DeathVFX.Play();
            StartCoroutine(HighlightController.PlayShine(controller.Coords));
            animator.SetTrigger(DieTrigger);
            AudioSystem.Play("FallToHole");
            StartCoroutine(transform.ScaleHide(1f));
            yield return new WaitForSeconds(3f);
            LevelLoader.RestartLevel();
        }
        
        private IEnumerator Victory()
        {
            animator.SetTrigger(VictoryTrigger);
            yield return 2f;
            LevelLoader.RestartLevel();
        }
    } 

}


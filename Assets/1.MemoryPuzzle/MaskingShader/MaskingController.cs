using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    public class MaskingController : MonoBehaviour
    {
        public float ShiningSpeed = 10f;
        public float DecoySpeed = .5f;
        public Transform MaskingCenter;
        private List<Material> maskedMaterials = new List<Material>();
        private Camera mainCamera;
        private static readonly int CutoutSize = Shader.PropertyToID("_CutoutSize");
        private static readonly int CutoutPos = Shader.PropertyToID("_CutoutPos");
        private static readonly int FalloutSize = Shader.PropertyToID("_FalloutSize");

        private float size = 0;
        
        private void Awake()
        {
            mainCamera = GetComponent<Camera>();
        }

        private void Init()
        {
            foreach (var tile in Board.current.Tiles)
                foreach (var material in tile.Value.Model.materials)
                    if (!maskedMaterials.Contains(material))
                        maskedMaterials.Add(material);
        }

        private void Update()
        {
            if (maskedMaterials.Count == 0)
                Init();
            
            var cutoutPos = mainCamera.WorldToViewportPoint(MaskingCenter.position);
            //cutoutPos.y /= Screen.width / Screen.height;
            
            foreach (var material in maskedMaterials)
            {
                material.SetVector(CutoutPos, cutoutPos); 
                material.SetFloat(CutoutSize, size);
                //material.SetFloat(FalloutSize, .05f);
            }
        }
        
        public IEnumerator PlayShine()
        {
            while (size < 1)
            {
                size += ShiningSpeed * Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            
            while (size > 0)
            {
                size -= DecoySpeed * Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}


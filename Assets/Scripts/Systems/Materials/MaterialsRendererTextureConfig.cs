using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class MaterialsRendererTextureConfig : MaterialsConfig
    {
        [SerializeField] private List<Texture2D> _texture2D;
        [SerializeField] private Renderer _renderer;
        
        protected override void Awake()
        {
            base.Awake();
            _renderer ??= GetComponent<Renderer>();
            _materials = _renderer.sharedMaterials;
        }

        public override void onEventChangeTexture(string tex = "EX = Name, Texture index, Materials index")
        {
            string[] getTex = tex.Split(",");
            _materials[int.Parse(getTex[2])].SetTexture(getTex[0], _texture2D[int.Parse(getTex[1])]);
        }
        
        
    }
}
using System;
using UnityEngine;

namespace GDD
{
    public class MaterialsConfig : MonoBehaviour
    {
        protected Material[] _materials;
        protected Material _material;
        
        public float parameterFloat;
        
        protected virtual void Awake()
        {
            
        }

        protected virtual void Update()
        {
            
        }

        public virtual void onEventChangeTexture(string tex = "EX = Name, Texture value")
        {
            
        }

        public virtual string[] onEventChangeFloat(string tex = "EX = Name, Materials value")
        {
            return tex.Split(",");;
        }

        public void ChangeFloat(string name, float value)
        {
            parameterFloat = value;
            _material.SetFloat(name, value);
        }
    }
}
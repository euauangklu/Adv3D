using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class MaterialsCanvasImageConfig : MaterialsConfig
    {
        [SerializeField] private Image _image;
        [SerializeField] private string nameParameter;
        
        protected override void Awake()
        {
            base.Awake();

            _image ??= GetComponent<Image>();
            _material = _image.material;
        }

        protected override void Update()
        {
            base.Update();
            
            ChangeFloat(nameParameter, parameterFloat);
        }

        public override string[] onEventChangeFloat(string tex = "EX = Name, Materials value")
        {
            string[] getTex = base.onEventChangeFloat(tex);

            _material.SetFloat(getTex[0], float.Parse(getTex[1]));
            return getTex;
        }
    }
}
using UnityEngine;

namespace GDD
{
    public class CharacterEquipSystem : MonoBehaviour
    {
        private string shirtThaiEthnicCulture = ThaiEthnicCulture.Indeterminate.ToString();
        private string trousersThaiEthnicCulture = ThaiEthnicCulture.Indeterminate.ToString();
        private string hairThaiEthnicCulture = ThaiEthnicCulture.TaiLue.ToString();
        private GameManager GM;
        private CharacterCreatorScript _character;

        protected void OnEnable()
        {
            GM ??= GameManager.Instance;
            _character ??= GetComponent<CharacterCreatorScript>();
            shirtThaiEthnicCulture = ThaiEthnicCulture.Indeterminate.ToString();
            trousersThaiEthnicCulture = ThaiEthnicCulture.Indeterminate.ToString();
            hairThaiEthnicCulture = ThaiEthnicCulture.TaiLue.ToString();
        }

        public void SetTrousersEthnicCulture(CharacterAsset asset)
        {
            GM.characterInstance.theme = asset.theme;
            trousersThaiEthnicCulture = asset.theme;
        }
        
        public void SetHairEthnicCulture(CharacterAsset asset)
        {
            hairThaiEthnicCulture = asset.theme;
            
            if(shirtThaiEthnicCulture != ThaiEthnicCulture.Indeterminate.ToString() || trousersThaiEthnicCulture != ThaiEthnicCulture.Indeterminate.ToString())
                return;
            
            GM.characterInstance.theme = asset.theme;
        }

        public void SetShirtEthnicCulture(CharacterAsset asset)
        {
            GM.characterInstance.theme = asset.theme;
            shirtThaiEthnicCulture = asset.theme;
        }

        public void SetShoeOffset(CharacterAsset asset)
        {
            if(asset != null)
                transform.position = ((CharacterMeshAsset)asset).offsetFoot;
            else
            {
                _character.ResetFootOffset();
            }
        }

        public void ResetShoeOffset(CharacterAsset asset)
        {
            _character.ResetFootOffset();
        }

        public void ResetShirtEthnicCulture(CharacterAsset asset)
        {
            if (asset == null)
            {
                if (hairThaiEthnicCulture != ThaiEthnicCulture.Indeterminate.ToString())
                    GM.characterInstance.theme = hairThaiEthnicCulture;
                else
                    GM.characterInstance.theme = ThaiEthnicCulture.TaiLue.ToString();
            }
            else
                GM.characterInstance.theme = trousersThaiEthnicCulture;

            GM.characterInstance.theme = ThaiEthnicCulture.TaiLue.ToString();
        }

        public void ResetTrousersEthnicCulture(CharacterAsset asset)
        {
            if (asset == null)
            {
                if (hairThaiEthnicCulture != ThaiEthnicCulture.Indeterminate.ToString())
                    GM.characterInstance.theme = hairThaiEthnicCulture;
                else
                    GM.characterInstance.theme = ThaiEthnicCulture.TaiLue.ToString();
            }
            else
                GM.characterInstance.theme = shirtThaiEthnicCulture;

            GM.characterInstance.theme = ThaiEthnicCulture.TaiLue.ToString();
        }

        public void ResetHairEthnicCulture(CharacterAsset asset)
        {
            if (asset == null)
            {
                GM.characterInstance.theme = ThaiEthnicCulture.TaiLue.ToString();
                return;
            }
            
            GM.characterInstance.theme = asset.theme;
            hairThaiEthnicCulture = asset.theme;

            if (shirtThaiEthnicCulture == ThaiEthnicCulture.Indeterminate.ToString() &&
                trousersThaiEthnicCulture == ThaiEthnicCulture.Indeterminate.ToString())
                GM.characterInstance.theme = ThaiEthnicCulture.TaiLue.ToString();
            else if (shirtThaiEthnicCulture != ThaiEthnicCulture.Indeterminate.ToString())
                GM.characterInstance.theme = shirtThaiEthnicCulture;
            else if (trousersThaiEthnicCulture != ThaiEthnicCulture.Indeterminate.ToString())
                GM.characterInstance.theme = trousersThaiEthnicCulture;
        }

        //When Equip Head Item Keep UnEquip All
        public void ResetHeadItem(string type)
        {
            GM.characterInstance.characterWardrobe[type] = _character.characterElements[type]._defaultOutfit;
            RemoveClothes(type);
        }
        
        protected async void RemoveClothes(string type)
        {
            GM.characterInstance.characterWardrobe[type] = _character.characterElements[type]._defaultOutfit;
            //await AutoLoadAssetType(_character.characterElements[type]._settingComponent, _character.characterElements[type]._defaultOutfit, type);
        }
    }
}
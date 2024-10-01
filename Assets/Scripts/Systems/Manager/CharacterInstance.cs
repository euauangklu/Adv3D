using System;
using System.Collections.Generic;
using SolidUtilities.Collections;
using UnityEngine.Serialization;

namespace GDD
{
    [Serializable]
    public class CharacterInstance
    {
        public string name;
        public string gender = "Male";
        public string theme = "TaiLue";
        public SerializableDictionary<string, string> characterWardrobe = new SerializableDictionary<string, string>();
    }
}
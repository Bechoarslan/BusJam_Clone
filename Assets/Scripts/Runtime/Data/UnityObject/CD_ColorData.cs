using System;
using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_ColorData", menuName = "Clone/CD_ColorData", order = 0)]
    public class CD_ColorData : ScriptableObject
    {
        public List<ColorData> Data;
        private Dictionary<PassengerColorType,Material> _colorMaterialDict;
        private bool _isInitialized;

        private void OnEnable()
        {
            _isInitialized = false;
        }

        private void Initialize()
        {
      
            if (_isInitialized) return;
            _colorMaterialDict = new Dictionary<PassengerColorType, Material>();
            foreach (var colorData in Data)
            {
                if (!_colorMaterialDict.ContainsKey(colorData.ColorType))
                {
                    _colorMaterialDict.Add(colorData.ColorType, colorData.ColorMaterial);
                }
            }
            _isInitialized = true;
        }

        public Material GetMaterial(PassengerColorType colorType)
        {
            Initialize();
            return _colorMaterialDict.GetValueOrDefault(colorType);
        }
    }
}
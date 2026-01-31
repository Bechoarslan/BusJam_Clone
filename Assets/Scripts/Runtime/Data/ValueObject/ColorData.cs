using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class ColorData
    {
        public PassengerColorType ColorType;
        public Material ColorMaterial;
    }
}
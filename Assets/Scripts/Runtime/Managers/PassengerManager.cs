using System;
using Runtime.Data.UnityObject;
using Runtime.Enums;
using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.Managers
{
    public class PassengerManager : MonoBehaviour, IPassenger
    {
        #region Self Variables

        #region Public Variables

        public PassengerColorType ColorType => colorType;

        #endregion

        #region Serialized Variables

        [SerializeField] private PassengerColorType colorType;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private CD_ColorData colorData;

        #endregion

        #region Private Variables


        #endregion

        #endregion


        private void Awake()
        {
            var randomColor = UnityEngine.Random.Range(1, Enum.GetNames(typeof(PassengerColorType)).Length);
            colorType = (PassengerColorType)randomColor;
            SetColor();
        }

        private void SetColor()
        {
            var colorMaterial = colorData.GetMaterial(colorType);
            
            meshRenderer.sharedMaterial = colorMaterial;
        }

     
    }

}
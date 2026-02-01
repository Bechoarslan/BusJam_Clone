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
        public void ChangeOutLine(float intensity)
        { 
            
            meshRenderer.GetPropertyBlock(_propertyBlock);
          
            _propertyBlock.SetFloat("_OutlineSize", intensity);
              
            meshRenderer.SetPropertyBlock(_propertyBlock);
        }

        #endregion

        #region Serialized Variables

        [SerializeField] private PassengerColorType colorType;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private CD_ColorData colorData;

   
        #endregion

        #region Private Variables


        private MaterialPropertyBlock _propertyBlock;
        #endregion

        #endregion


        private void Awake()
        {
           
            var randomColor = UnityEngine.Random.Range(1, Enum.GetNames(typeof(PassengerColorType)).Length);
            colorType = (PassengerColorType)randomColor;
            _propertyBlock = new MaterialPropertyBlock();
            SetColor();
        }

        private void SetColor()
        {
           
            var colorMaterial = colorData.GetMaterial(colorType);
            meshRenderer.sharedMaterial = colorMaterial;
            
           
        }

     
    }

}
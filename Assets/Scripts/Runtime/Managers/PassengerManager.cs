using System;
using System.Collections.Generic;
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

        [SerializeField]public List<Vector2Int> Path { get; set; }
        public PassengerColorType ColorType => colorType;
       

        
        #endregion

        #region Serialized Variables

        [SerializeField] private PassengerColorType colorType;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private CD_ColorData colorData;

   
        private bool _isReadyToWalk;
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
            Path = new List<Vector2Int>();
        }

        private void SetColor()
        {
           
            var colorMaterial = colorData.GetMaterial(colorType);
            meshRenderer.sharedMaterial = colorMaterial;
            
           
        }

        public void IsReadyToWalk(bool value)
        {
            _isReadyToWalk = value;
            if (value)
            {
                meshRenderer.GetPropertyBlock(_propertyBlock);
                
                _propertyBlock.SetFloat("_OutlineSize", 5);
                meshRenderer.SetPropertyBlock(_propertyBlock);
            }
            else
            {
                meshRenderer.GetPropertyBlock(_propertyBlock);
          
                _propertyBlock.SetFloat("_OutlineSize", 0);
              
                meshRenderer.SetPropertyBlock(_propertyBlock);
            }
        }

    
    }

}
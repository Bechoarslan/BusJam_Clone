using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Data.UnityObject;
using Runtime.Enums;
using Runtime.Interfaces;
using Sirenix.OdinInspector;
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


        [Button]
        public void OnWalk()
        {
            StartCoroutine(StarWalk());
        }

        
        private IEnumerator StarWalk()
        {
            if (_isReadyToWalk && Path.Count > 0)
            {
                foreach (var position in Path)
                {
                    Vector3 targetPosition = new Vector3(position.x, transform.position.y, position.y);
                    while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2f);
                        yield return null;
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

}
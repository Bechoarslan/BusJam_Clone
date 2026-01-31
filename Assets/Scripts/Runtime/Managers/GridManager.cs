using System;
using System.Collections.Generic;
using Runtime.Interfaces;
using Runtime.Keys;
using UnityEngine;

namespace Runtime.Managers
{
    public class GridManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private int Row;
        [SerializeField] private int Column;

        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private GameObject passengerPrefab;
        
        #endregion

        #region Private Variables
        
        private Dictionary<Vector2Int,CellData> _gridData = new Dictionary<Vector2Int, CellData>();
         #endregion

        #endregion


        private void Awake()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            var parent = new GameObject("Grid");
            var gridParent = new GameObject("Grid Parent");
            var passengerParent = new GameObject("Passenger Parent");
            gridParent.transform.parent = parent.transform;
            passengerParent.transform.parent = parent.transform;
            for (int x = Row - 1; x >= 0; x--)
            {
                for (int z = Column; z > 0; z--)
                {
                    var obj = Instantiate(gridPrefab,gridParent.transform);
                    var passenger = Instantiate(passengerPrefab,passengerParent.transform);
                    passenger.transform.position = new Vector3(x, 1.5f, z);
                    var passengerColor = passenger.GetComponent<IPassenger>().ColorType;
                    var cellData = new CellData
                    {
                         PassengerObject =  passenger,
                         IsReadyToWalk = false,
                        
                    };
                    
                    obj.name = $"Grid_{x}_{z}";
                    obj.transform.position = new Vector3(x, 0, z);
                }
            }
        }
    }
}
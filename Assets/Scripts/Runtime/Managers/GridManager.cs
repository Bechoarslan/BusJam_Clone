using System;
using System.Collections.Generic;
using Runtime.Interfaces;
using Runtime.Keys;
using Sirenix.OdinInspector;
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
        
        private Dictionary<GridData,CellData> _gridData = new Dictionary<GridData, CellData>();
        private Transform _gridParent;
         #endregion

        #endregion


        private void Awake()
        {
            CreateGrid();
            
        }

        private void CreateGrid()
        {
            CreateWaitingGrid();
            var parent = new GameObject("Grid");
            _gridParent = parent.transform;
            var gridParent = new GameObject("Grid Parent");
            var passengerParent = new GameObject("Passenger Parent");
            gridParent.transform.parent = parent.transform;
            passengerParent.transform.parent = parent.transform;
            for (int x = 0; x < Row; x++)
            {
                for (int z = 0; z < Column; z++)
                {
                    var obj = Instantiate(gridPrefab,gridParent.transform);
                    var passenger = Instantiate(passengerPrefab,passengerParent.transform);
                    passenger.transform.position = new Vector3(x, 1.5f, z);
                    passenger.name = $"Passenger {x},{z}";
                    var passengerColor = passenger.GetComponent<IPassenger>().ColorType;
                    var cellData = new CellData
                    {
                        PassengerObject =  passenger,
                        IsReadyToWalk = false,
                        PassengerColorType =  passengerColor

                    };

                    
                    var gridData = new GridData
                    {
                        gridPosition = new Vector2Int(x, z),
                        isOccupied =  true

                    };
                    _gridData.Add( gridData,cellData);
                    obj.name = $"Grid_{x}_{z}";
                    obj.transform.position = new Vector3(x, 0, z);
                }
            }
        }

        private void CreateWaitingGrid()
        {
            var parent = new GameObject("Waiting Grid");
            parent.transform.parent = _gridParent;
            for (int i = 0; i < Row; i++)
            {
               
                var gridData = new GridData
                {
                     gridPosition =  new Vector2Int(i, Column),
                     isOccupied =  false

                };
                _gridData.Add(gridData, null);
                if(i >= 5) continue;
                var obj = Instantiate(gridPrefab,parent.transform);
                obj.name = $"Waiting_Grid_{i}";
                obj.transform.position = new Vector3(i + i* 0.5f , 0,   Column + 1);

            }
        }

        [Button("Check Grid Occupied")]
        private void CheckGridIsOccupied()
        {
            var gridMap = new Dictionary<Vector2Int, bool>();
            var visited = new HashSet<Vector2Int>();
            foreach (var data in _gridData.Keys)
            {
                gridMap.Add(data.gridPosition,data.isOccupied);
                
            }

            foreach (var data in _gridData.Keys)
            {
                if(visited.Contains(data.gridPosition) || !data.isOccupied) continue;
                visited.Add(data.gridPosition);
                var positionsToCheck = GetListPosition();
                foreach (var position in positionsToCheck)
                {
                    var newPosition = data.gridPosition + position;
                    if (gridMap.ContainsKey(newPosition))
                    {
                        if (!gridMap[newPosition])
                        {
                            var passenger = _gridData[data].PassengerObject;
                            if (passenger != null)
                            {
                                 passenger.GetComponent<IPassenger>().ChangeOutLine(5);
                            }
                        }
                    }
                }
            }
            
            
        }
        
        
        private List<Vector2Int> GetListPosition()
        {
            return new  List<Vector2Int>(){Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right};
        }
    }
}
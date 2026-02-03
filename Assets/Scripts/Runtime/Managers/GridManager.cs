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
        
        private Dictionary<Vector2Int,CellData> _gridData = new Dictionary<Vector2Int, CellData>();
        private HashSet<Vector2Int> _waitingCells = new HashSet<Vector2Int>();
        private Transform _gridParent;
        [SerializeField]private List<GameObject> _path = new List<GameObject>();
         #endregion

        #endregion


        private void Awake()
        {
            CreateGrid();
            CreateWaitingGrid();
            CheckGridIsOccupied();
        }

        private void CreateGrid()
        {
            
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
                        PassengerColorType =  passengerColor,
                        IsOccupied =  true,
                        IsReadyToWalk = false

                    };

                    
                    
                    _gridData.Add( new Vector2Int( x,z) ,cellData);
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
                _waitingCells.Add( new Vector2Int( i , Column ) );
                if(i >= 5) continue;
                var obj = Instantiate(gridPrefab,parent.transform);
                obj.name = $"Waiting_Grid_{i},{Column}";
                obj.transform.position = new Vector3(i + i* 0.5f , 0,   Column + 1);
                

            }
        }
        
        [Button("Check Grid Is Occupied")]
        private void CheckGridIsOccupied()
        {
            

            foreach (var kvp in _gridData)
            {
                var visited = new HashSet<Vector2Int>();
                var queue = new Queue<Vector2Int>();
                var list = new List<Vector2Int>();
                var startPos = kvp.Key;
                var cell = kvp.Value;
              
                
                if (cell == null || !cell.IsOccupied  || cell.IsReadyToWalk) continue;
                if (visited.Contains(startPos)) continue;

                visited.Add(startPos);
                queue.Enqueue(startPos);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();

                    foreach (var dir in GetListPosition())
                    {
                        var next = current + dir;

                        if (visited.Contains(next)) continue;
                        
                        // Grid içinde
                        if (_gridData.TryGetValue(next, out var nextCell))
                        {
                            if (nextCell == null || !nextCell.IsOccupied)
                            {
                                Debug.Log("Found unoccupied cell at: " + next);
                                visited.Add(next);
                                queue.Enqueue(next);
                                list.Add(next);
                              
                            
                                
                            }

                           
                          
                            
                            
                        }

                       
                        // Waiting grid
                         if (_waitingCells.Contains(next))
                        {
                            
                            Debug.Log("Passenger at " + startPos + " can move to waiting cell at: " + next);
                            var passengerObj = cell.PassengerObject;
                            if (passengerObj == null) continue;
                            passengerObj.GetComponent<IPassenger>().IsReadyToWalk(true);
                            passengerObj.GetComponent<IPassenger>().Path.AddRange(list);
                            Debug.Log(list.Count);
                            cell.IsReadyToWalk = true;


                        }
                       
                      
                    }
                    
                }
                visited.Clear();
                queue.Clear();
            }
        }



        [Button("Clear Objects")]
        private void ClearObjects()
        {
            
            foreach (var obj in _path)
            {
                var objGrid = new Vector2Int(Mathf.RoundToInt(obj.transform.position.x),
                    Mathf.RoundToInt(obj.transform.position.z));
                if (_gridData.ContainsKey(objGrid)) 
                {
                    Debug.Log("Clearing object at: " + objGrid);
                    _gridData[objGrid].IsOccupied = false;
                    Destroy(_gridData[objGrid].PassengerObject);
                    _gridData[objGrid].PassengerObject = null;
                }
            }
            
        }
        
        private List<Vector2Int> GetListPosition()
        {
            return new  List<Vector2Int>(){Vector2Int.up, Vector2Int.right, Vector2Int.left};
        }
    }
}
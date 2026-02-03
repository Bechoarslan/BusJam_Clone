using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Interfaces
{
    public interface IPassenger
    {
        PassengerColorType ColorType { get; }
        void IsReadyToWalk(bool intensity);

        List<Vector2Int> Path { get; set; }

    }
}
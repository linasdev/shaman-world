using System;
using map;
using UnityEngine;

namespace exceptions
{
    public class DirectionAlreadyPresentInMapNodeException : Exception
    {
        private Vector2Int Position { get; }
        private MapDirection Direction { get; }

        public DirectionAlreadyPresentInMapNodeException(Vector2Int position, MapDirection direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}
using System;
using Map;
using UnityEngine;

namespace Exceptions
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
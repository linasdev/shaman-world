using System;
using Map;
using UnityEngine;

namespace Exceptions
{
    public class DirectionMissingInMapNodeException : Exception
    {
        private Vector2Int Position { get; }
        private MapDirection Direction { get; }

        public DirectionMissingInMapNodeException(Vector2Int position, MapDirection direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}
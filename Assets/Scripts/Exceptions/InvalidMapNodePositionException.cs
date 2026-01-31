using System;
using Map;
using UnityEngine;

namespace Exceptions
{
    public class InvalidMapNodePositionException : Exception
    {
        private Vector2Int Position { get; }

        public InvalidMapNodePositionException(Vector2Int position)
        {
            Position = position;
        }
    }
}
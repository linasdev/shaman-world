using System;
using map;

namespace exceptions
{
    public class DirectionMissingInMapNodeException : Exception
    {
        private MapDirection Direction { get; }

        public DirectionMissingInMapNodeException(MapDirection direction)
        {
            this.Direction = direction;
        }
    }
}
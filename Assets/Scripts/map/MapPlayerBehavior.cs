using UnityEngine;
using UnityEngine.InputSystem;

namespace map
{
    public class MapPlayerBehavior : MonoBehaviour
    {
        private MapNode _selectedMapNode = new MainMap().GetRootNode();
        private Transform transform;
        private InputAction moveAction;
        
        void Start()
        {
            transform = GetComponent<Transform>();
            moveAction = InputSystem.actions.FindAction("Move");
        }

        void Update()
        {
            if (!moveAction.triggered)
            {
                return;
            }
            
            Vector2Int movementDirection = Vector2Int.RoundToInt(moveAction.ReadValue<Vector2>());

            if (movementDirection.sqrMagnitude == 0)
            {
                return;
            }

            MapDirection? direction = MapDirectionMethods.FromVector2Int(movementDirection);

            if (direction == null)
            {
                return;
            }

            MapNode nextMapNode =  _selectedMapNode.GetNeighbor((MapDirection)direction);

            if (nextMapNode == null)
            {
                return;
            }
            
            _selectedMapNode = nextMapNode;
            transform.position = new Vector3(_selectedMapNode.GetPosition().x * 5, _selectedMapNode.GetPosition().y * 5, 0);
        }
    }
}
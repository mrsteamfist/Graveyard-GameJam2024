using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SearchService;

namespace Graveyard
{
    public class ControllerAPI : MonoBehaviour
    {
        RPGGraveyard controls;
        public CharacterController2D characerController;

        private void Awake()
        {
            controls = new RPGGraveyard();
            controls.Player.Move.performed += ctx => characerController.nextMoveCommand = ctx.ReadValue<Vector2>();
            controls.Player.Move.canceled += ctx => characerController.nextMoveCommand = Vector2.zero;
        }

        private void OnEnable()
        {
            controls.Player.Enable();
        }

        private void OnDisable()
        {
            controls.Player.Disable();
        }

        private void Update()
        {
            
        }
    }
}
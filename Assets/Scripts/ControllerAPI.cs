using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Graveyard
{
    public class ControllerAPI : MonoBehaviour
    {
        public PixelPerfectCamera SceneCamera;
        RPGGraveyard controls;
        private Vector2 nextMoveCommand;
        public float moveSpeed = 5f;
        public Transform characterPosition;
        public SpriteRenderer spriteRenderer;
        public Sprite bottomFacing;
        public Sprite leftFacing;
        public Sprite topFacing;

        private void Awake()
        {
            controls = new RPGGraveyard();
            controls.Player.Move.performed += ctx => nextMoveCommand = ctx.ReadValue<Vector2>();
            controls.Player.Move.canceled += ctx => nextMoveCommand = Vector2.zero;
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

        void FixedUpdate()
        {
            if (SceneCamera != null)
            {
                Vector2 pos = (Vector2)characterPosition.position + nextMoveCommand * moveSpeed * Time.fixedDeltaTime;

                // Check if the vector is facing up
                if (nextMoveCommand.y > 0 && Mathf.Approximately(nextMoveCommand.x, 0))
                {
                    spriteRenderer.sprite = topFacing;
                }
                // Check if the vector is facing left
                else if (nextMoveCommand.x < 0 && Mathf.Approximately(nextMoveCommand.y, 0))
                {
                    spriteRenderer.sprite = leftFacing;
                    spriteRenderer.flipX = false;
                }
                // Check if the vector is facing right
                else if (nextMoveCommand.x > 0 && Mathf.Approximately(nextMoveCommand.y, 0))
                {
                    spriteRenderer.sprite = leftFacing;
                    spriteRenderer.flipX = true;
                }
                // Check if the vector is facing down
                else //if (nextMoveCommand.y < 0 && Mathf.Approximately(nextMoveCommand.x, 0))
                {
                    spriteRenderer.sprite = bottomFacing;
                }

                characterPosition.position = SceneCamera.RoundToPixel(pos);
            }
        }
    }
}
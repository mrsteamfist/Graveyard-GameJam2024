using RPGM.Gameplay;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Graveyard
{
    [ExecuteAlways]
    public class ControllerAPI : MonoBehaviour
    {
        public enum Facing
        {
            Bottom,
            Left,
            Right,
            Top
        }

        public PixelPerfectCamera SceneCamera;
        RPGGraveyard controls;
        private Vector2 nextMoveCommand;
        public float moveSpeed = 5f;
        public Transform characterPosition;
        public SpriteRenderer spriteRenderer;
        public Sprite bottomFacing;
        public Sprite leftFacing;
        public Sprite topFacing;
        public Transform Lantern;
        public Facing facing = Facing.Bottom;
        public bool HasLantern = false;
        public Vector2 lanternTopPos = new Vector2(0, 1);
        public Quaternion lanternTopRot = new(90f, 0f, 0f, 0f);
        public Vector2 lanternBottomPos = new Vector2(0, 1);
        public Quaternion lanternBottomRot = new(-90f, 0f, 0f, 0f);
        public Vector2 lanternLeftPos = new Vector2(-1, 0);
        public Quaternion lanternLeftRot = new(90f, 0f, 0f, 0f);
        public Vector2 lanternRightPos = new Vector2(0, 1);
        public Quaternion lanternRightRot = new(90f, 0f, 0f, 0f);
        public bool ManualSetFacing = false;

        [HideInInspector]
        public Tombstone currentTombstone;

        private void Awake()
        {
            controls = new RPGGraveyard();
            controls.Player.Move.performed += ctx => nextMoveCommand = ctx.ReadValue<Vector2>();
            controls.Player.Move.canceled += ctx => nextMoveCommand = Vector2.zero;
            controls.Player.Interact.performed += ctx => currentTombstone?.Grow();
        }

        void OnEnable()
        {
            EditorApplication.update += MoveUpdate;
            controls?.Player.Enable();
        }

        private void MoveUpdate()
        {
            if (!ManualSetFacing || nextMoveCommand != Vector2.zero)
            {
                SetFacing();
            }

            if (SceneCamera != null)
            {
                Vector2 pos = (Vector2)characterPosition.position + nextMoveCommand * moveSpeed * Time.fixedDeltaTime;
                characterPosition.position = SceneCamera.RoundToPixel(pos);
            }
            if (Lantern != null)
            {
                Lantern.gameObject.SetActive(HasLantern);
            }

            if (spriteRenderer != null)
            {
                switch (facing)
                {
                    case Facing.Left:
                        spriteRenderer.sprite = leftFacing;
                        spriteRenderer.flipX = false;
                        if (Lantern != null && HasLantern)
                        {
                            Lantern.transform.rotation = lanternLeftRot;
                            Lantern.transform.localPosition = lanternLeftPos;
                        }
                        break;
                    case Facing.Right:
                        spriteRenderer.sprite = leftFacing;
                        spriteRenderer.flipX = true;
                        if (Lantern != null && HasLantern)
                        {
                            Lantern.transform.rotation = lanternRightRot;
                            Lantern.transform.localPosition = lanternRightPos;
                        }
                        break;
                    case Facing.Top:
                        spriteRenderer.sprite = topFacing;
                        if (Lantern != null && HasLantern)
                        {
                            Lantern.transform.rotation = lanternTopRot;
                            Lantern.transform.localPosition = lanternTopPos;
                        }
                        break;
                    default: // Bottom
                        spriteRenderer.sprite = bottomFacing;
                        if (Lantern != null && HasLantern)
                        {
                            Lantern.transform.rotation = lanternBottomRot;
                            Lantern.transform.localPosition = lanternBottomPos;
                        }
                        break;
                }
            }
        }

        static bool IsInbetween(float pos, float small, float large)
        {
            return pos > small && pos < large;
        }

        private void SetFacing()
        {
            // Check if the vector is facing left
            if (nextMoveCommand.x < 0 && IsInbetween(nextMoveCommand.y, -.5f, .5f))
            {
                facing = Facing.Left;
            }
            // Check if the vector is facing right
            else if (nextMoveCommand.x > 0 && IsInbetween(nextMoveCommand.y, -.5f, .5f))
            {
                facing = Facing.Right;
            }
            else if (nextMoveCommand.y > 0)
            {
                facing = Facing.Top;
            }
            // Check if the vector is facing down
            else //if (nextMoveCommand.y < 0
            {
                facing = Facing.Bottom;
            }
        }

        void OnDisable()
        {
            EditorApplication.update -= MoveUpdate;
            controls?.Player.Disable();
        }

        void FixedUpdate()
        {
            if (SceneCamera != null)
            {
                MoveUpdate();
            }
        }
    }
}
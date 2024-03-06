using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : MonoBehaviour
{
    public enum State
    {
        Placeholder,
        Start,
        Grow,
        Highlight,
        Damage,
        Destroy,
    }

    public SpriteRenderer SpriteRenderer;

    public State TombstoneState;

    public Sprite PlaceholderSprite;
    public Sprite StartSprite;
    public Sprite GrowSprite;
    public Sprite HighlightSprite;
    public Sprite DamageSprite;
    public Sprite DestorySprite;

    // Start is called before the first frame update
    void Start()
    {
        if (SpriteRenderer == null)
        {
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// Update the control to make sure it matches to the states
    /// </summary>
    void LateUpdate()
    {
        if (SpriteRenderer == null)
        {
            Debug.LogError("Tombstone is missing the Sprite Renderer");
        }
        else
        {
            switch (TombstoneState)
            {
                case State.Placeholder:
                    SpriteRenderer.sprite = PlaceholderSprite;
                    break;
                case State.Start:
                    SpriteRenderer.sprite = StartSprite;
                    break;
                case State.Grow:
                    SpriteRenderer.sprite = GrowSprite;
                    break;
                case State.Highlight:
                    SpriteRenderer.sprite = HighlightSprite;
                    break;
                case State.Damage:
                    SpriteRenderer.sprite = DamageSprite;
                    break;
                case State.Destroy:
                    SpriteRenderer.sprite = DestorySprite;
                    break;
                default:
                    Debug.LogError($"Unknown Tombstone States {TombstoneState}");
                    break;
            }
        }
    }
}

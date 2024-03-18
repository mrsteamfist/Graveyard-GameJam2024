using Graveyard;
using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.Rendering;


[ExecuteAlways]
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
    public ConversationScript interactConversation;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (interactConversation != null)
            {
                var ev = Schedule.Add<RPGM.Events.ShowConversation>();
                ev.conversation = interactConversation;
                ev.gameObject = gameObject;
                ev.conversationItemKey = "";
            }
            ControllerAPI api;
            if (collision.gameObject.tag == "Player" && (api = collision.gameObject.GetComponent<ControllerAPI>()) != null)
            {
                api.currentTombstone = this;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        ControllerAPI api;
        if (collision.gameObject.tag == "Player" && (api = collision.gameObject.GetComponent<ControllerAPI>()) != null)
        {
            api.currentTombstone = null;
        }
    }

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

    internal void LoadSaved(Tombstone tombstone)
    {
        TombstoneState = tombstone.TombstoneState;
    }

    public void TakeDamage()
    {
        switch (TombstoneState)
        {
            case State.Destroy:
                gameObject.SetActive(false);
                break;
            case State.Damage:
                TombstoneState = State.Destroy;
                break;
            default:
                TombstoneState = State.Damage;
                break;
        }
    }

    public void Grow()
    {
        switch (TombstoneState)
        {
            case State.Destroy:
                gameObject.SetActive(false);
                break;
            case State.Damage:
                TombstoneState = State.Start;
                break;
            case State.Grow:
                TombstoneState = State.Highlight;
                break;
            default:
                TombstoneState = State.Grow;
                break;
        }
    }
}

using Graveyard;
using UnityEngine;

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
    public Sprite DamageSprite;
    public Sprite DestorySprite;
    public int interactMessage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (TombstoneState == State.Placeholder || TombstoneState == State.Start || TombstoneState == State.Damage || TombstoneState == State.Destroy)
                collision.gameObject.GetComponent<CharacterDialogManager>().ShowDialog(interactMessage);

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
            collision.gameObject.GetComponent<CharacterDialogManager>().HideDialog();
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
            case State.Damage:
                TombstoneState = State.Destroy;
                break;
            default:
                TombstoneState = State.Damage;
                break;
        }
        StateManager.Instance.PlayDmgSfx();
    }

    public void Grow()
    {
        switch (TombstoneState)
        {
            case State.Placeholder:
                TombstoneState = State.Start;
                break;
            case State.Destroy:
                TombstoneState = State.Placeholder;
                break;
            default:
                TombstoneState = State.Grow;
                break;
        }
        StateManager.Instance.PlayGrowSfx();
    }
}

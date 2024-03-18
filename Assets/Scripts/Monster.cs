using System;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public AnimationClip DeathAnimation;
    public event EventHandler OnDeath;
    //ToDo: Monster move animation

    public bool CanCollide = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CanCollide || collision == null)
        {
            Debug.Log("Bad collision");
            return;
        }
        Debug.Log("Collision with " + collision.gameObject.name + " tag " + collision.gameObject.tag);

        if (collision.gameObject.tag == "Lantern")
        {
            StartCoroutine(DestroyMonster());
        }
        else if (collision.gameObject.tag == "Tombstone")
        {
            collision.gameObject.GetComponent<Tombstone>()?.TakeDamage();
            StartCoroutine(DestroyMonster());
            // ToDo handle tombstone damage
        }
    }

    public IEnumerator DestroyMonster()
    {
        Debug.Log("Destorying Monster");
        CanCollide = false;
        OnDeath?.Invoke(this, EventArgs.Empty);
        /*
        if (DeathAnimation != null)
        {
            Debug.Log("Destorying Monster");
            GetComponent<Animation>().Play(DeathAnimation.name);
            Debug.Log($"Taking {DeathAnimation.length} to destory");
            yield return new WaitForSeconds(DeathAnimation.length);
        }
        */
        yield return null;
        Debug.Log("Destoryed Monster");
        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public AnimationClip DeathAnimation;
    //ToDo: Monster move animation

    public bool CanCollide = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (!CanCollide)
        {
            return;
        }
        // ToDo: check collision to make sure it's the light
        if (collision != null && collision.gameObject != null)
        {
            DestroyMonster();
        }
    }

    public IEnumerator DestroyMonster()
    {
        CanCollide = false;
        if (DeathAnimation != null)
        {
            Debug.Log("Destorying Monster");
            GetComponent<Animation>().Play(DeathAnimation.name);
            Debug.Log($"Taking {DeathAnimation.length} to destory");
            yield return new WaitForSeconds(DeathAnimation.length);
        }

        Debug.Log("Destoryed Monster");

        Destroy(gameObject);
    }
}

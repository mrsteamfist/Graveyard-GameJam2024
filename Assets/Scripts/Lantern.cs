using UnityEngine;

namespace Graveyard
{
    public class Lantern : MonoBehaviour
    {
        public bool isPlayerTouching = false; //Todo: do I need this anymore?
        public int interactMessage = 0;

        private void Awake()
        {
            isPlayerTouching = false;
        }
        
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                isPlayerTouching = true;
                collision.gameObject.GetComponent<CharacterDialogManager>().ShowDialog(interactMessage);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                isPlayerTouching = true;
                collision.gameObject.GetComponent<CharacterDialogManager>().HideDialog();
            }
        }
    }
}

using RPGM.Core;
using RPGM.Gameplay;
using RPGM.UI;
using UnityEngine;

namespace Graveyard
{
    public class Lantern : MonoBehaviour
    {
        public bool isPlayerTouching = false;
        public DialogController dialogController;
        public string interactMessage;

        private void Awake()
        {
            isPlayerTouching = false;
            dialogController.Hide();
        }
        
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                isPlayerTouching = true;
                
                if (!string.IsNullOrEmpty(interactMessage))
                {
                    dialogController.Show(GetComponent<Transform>().position, interactMessage);
                }
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                isPlayerTouching = true;
                dialogController.Hide();
            }
        }
    }
}

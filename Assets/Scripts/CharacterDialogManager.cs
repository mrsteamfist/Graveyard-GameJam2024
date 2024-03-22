using TMPro;
using UnityEngine;

namespace Graveyard
{
    public class CharacterDialogManager : MonoBehaviour
    {
        public GameObject dialog;
        public TextMeshProUGUI textField;
        public string[] Messages;

        void Awake()
        {
            HideDialog();
        }

        public void ShowDialog(int dialogId)
        {
            HideDialog();
            if (dialog == null || textField == null)
            {
                return;
            }

            if(Messages == null || Messages.Length <= dialogId)
            {
                return;
            }

            textField.text = Messages[dialogId];
            dialog.SetActive(true);
        }

        public void HideDialog()
        {
            dialog?.SetActive(false);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class CreatePopup : MonoBehaviour
    {
        [SerializeField] private string _title;
        [SerializeField] private string _massage;
        [SerializeField] private string _acceptButton;
        [SerializeField] private string _cancelButton;
        [SerializeField] private UnityEvent OnAccept;
        [SerializeField] private UnityEvent OnCancel;

        private MassageManager MM;

        public void ShowPopup()
        {
            MM ??= MassageManager.Instance;
            
            if(MM != null) 
                MM.CreateMassage(() => { OnAccept?.Invoke(); }, () => { OnCancel?.Invoke(); }, _title, _massage, _acceptButton, _cancelButton, 10, true);
        }
    }
}
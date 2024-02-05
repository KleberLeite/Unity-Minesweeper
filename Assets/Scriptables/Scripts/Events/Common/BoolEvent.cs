using UnityEngine;
using UnityEngine.Events;

namespace Minesweeper.Events
{
    [CreateAssetMenu(menuName = "Events/Common/bool")]
    public class BoolEvent : ScriptableObject
    {
        public UnityAction<bool> OnEvent;
        public void Raise(bool arg) => OnEvent?.Invoke(arg);
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace Minesweeper.Events
{
    [CreateAssetMenu(menuName = "Events/Common/void")]
    public class VoidEvent : ScriptableObject
    {
        public UnityAction OnEvent;
        public void Raise() => OnEvent?.Invoke();
    }
}

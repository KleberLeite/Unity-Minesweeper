using UnityEngine;

namespace Minesweeper.Databases
{
    public class Data : ScriptableObject, IData
    {
        [Header("Data Settings")]
        [SerializeField] private object value;

        public int ID { get; set; }

        public object GetValue() => value;
    }
}

using UnityEngine;

namespace Minesweeper.Databases
{
    public class Data : ScriptableObject, IData
    {
        [Header("Data Settings")]
        [SerializeField] private object value;
        [SerializeField, HideInInspector] private int id;

        public int ID { get => id; }

        public object GetValue() => value;
    }
}

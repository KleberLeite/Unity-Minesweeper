using UnityEngine;

namespace Minesweeper.Databases
{
    [CreateAssetMenu(menuName = "Databases/Database")]
    public class Database : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private Data[] datas;

        public int Count() => datas.Length;

        public Data GetDataByID(int id)
        {
            foreach (Data data in datas)
            {
                if (data.ID == id)
                    return data;
            }

            return null;
        }

        public Data GetDataByIndex(int index)
        {
            if (index >= datas.Length)
                return null;

            return datas[index];
        }

        public Data[] GetAll() => datas;
    }
}

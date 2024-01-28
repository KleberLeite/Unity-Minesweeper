using UnityEngine;
using UnityEditor;
using Minesweeper.Databases;
using UnityEditor.UIElements;
using Unity.VisualScripting;

namespace Minesweeper.Editor
{
    public class DatabaseWindow : EditorWindow
    {
        private ObjectField targetField;
        private Data[] datas;

        [MenuItem("Windows/Database ")]
        public static void ShowExample()
        {
            DatabaseWindow wnd = GetWindow<DatabaseWindow>();
            wnd.titleContent = new GUIContent("DatabaseWindow");
        }

        private void OnGUI()
        {
            if (!targetField.value)
                return;

            Database target = (Database)targetField.value;
            SerializedObject datasSO = new SerializedObject(target);
            SerializedProperty datasSP = datasSO.FindProperty("datas");
            EditorGUILayout.PropertyField(datasSP, true);
            datasSO.ApplyModifiedProperties();

            if (GUILayout.Button("Set ID by ordering"))
                SetIDByOrdering(target);
        }

        public void CreateGUI()
        {
            targetField = new ObjectField("Target:");
            targetField.allowSceneObjects = false;
            targetField.objectType = typeof(Database);
            rootVisualElement.Add(targetField);
        }

        private void SetIDByOrdering(Database target)
        {
            Data[] datas = target.GetAll();
            for (int i = 0; i < datas.Length; i++)
                datas[i].ID = i;
        }
    }
}

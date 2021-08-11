using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Infinite_story
{
    class SaveLoadContoller : MonoBehaviour
    {
        [HideInInspector]public static string SaveFileName;
        private SaveGame GameSaver;
        private LoadGame GameLoad;
#if UNITY_EDITOR
        [MenuItem("Project/Define Save File")]
        static void DefineSaveFile()
        {
            SaveFileName = EditorUtility.SaveFilePanel(
                "Define Save File",
                Application.dataPath,
                "infinite_story.sav",
                "sav");
            /*
            if(SaveFileName == null)
            {
                EditorUtility.DisplayDialog("Choose file to save", "You must define Save File first!", "OK");
                return;
            }
            */
        }
#endif
        private void Awake()
        {
            if (SaveFileName == null)
            {
                SaveFileName = Application.dataPath + "/infinite_story.sav";
            }
            GameSaver = new SaveGame(SaveFileName);
            GameSaver.Awake();
            GameLoad = new LoadGame(SaveFileName);
            GameLoad.Awake();
        }

        private void OnDestroy()
        {
            GameSaver.OnDestroy();
            GameLoad.OnDestroy();
        }

        private void Start()
        {
            
        }

    }
}

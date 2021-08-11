using System;
using UnityEngine;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace Infinite_story
{
    public class LoadGame
    {
        public LoadGame(string filename)
        {
            _saveGameFileName = filename;
        }
        private string _saveGameFileName;

        public void Awake()
        {
            UIController.LoadFileAction += ReadSaveFile;
        }

        public void OnDestroy()
        {
            UIController.LoadFileAction -= ReadSaveFile;
        }

        public void ReadSaveFile()
        {
            // читаем документ с диска
            XmlDocument doc = new XmlDocument();
            doc.Load(_saveGameFileName);
            #region Read bonuses
            // Ищем элементы, где живут аттрибуты с нужными параметрами
            XmlNodeList elemList = doc.GetElementsByTagName("RootObject");

            for (int i = 0; i < elemList.Count; i++)
            {
                XmlNode ObjectRootNode = elemList.Item(i);
                GameObject RootGameObject = new GameObject();
                float RootPosX, RootPosY, RootPosZ;

                XmlAttribute attr = (XmlAttribute)ObjectRootNode.Attributes.GetNamedItem("x");
                float.TryParse(attr.Value, out RootPosX);
                attr = (XmlAttribute)ObjectRootNode.Attributes.GetNamedItem("y");
                float.TryParse(attr.Value, out RootPosY);
                attr = (XmlAttribute)ObjectRootNode.Attributes.GetNamedItem("z");
                float.TryParse(attr.Value, out RootPosZ);
                attr = (XmlAttribute)ObjectRootNode.Attributes.GetNamedItem("tag");
                RootGameObject.tag = attr.Value;


                RootGameObject.transform.position = new Vector3(RootPosX, RootPosY, RootPosZ);

                // Вытаскиваем дочерние ноды, из них аттрибуты и спауним бонусы
                if (ObjectRootNode.HasChildNodes)
                {
                    for (int j = 0; j < ObjectRootNode.ChildNodes.Count; j++)
                    {
                        attr = (XmlAttribute)ObjectRootNode.ChildNodes[j].Attributes.GetNamedItem("Type");
                        string LoadPrefabFullName = Regex.Replace(attr.Value, @"\(.+\)", String.Empty);
                        string LoadPrefabShortName = "Prefabs/" + LoadPrefabFullName;
                        LoadPrefabFullName = "Assets/Resources/Prefabs/" + LoadPrefabFullName + ".prefab";

                        float PosX, PosY, PosZ;
                        attr = (XmlAttribute)ObjectRootNode.ChildNodes[j].Attributes.GetNamedItem("x");
                        float.TryParse(attr.Value, out PosX);
                        attr = (XmlAttribute)ObjectRootNode.ChildNodes[j].Attributes.GetNamedItem("y");
                        float.TryParse(attr.Value, out PosY);
                        attr = (XmlAttribute)ObjectRootNode.ChildNodes[j].Attributes.GetNamedItem("z");
                        float.TryParse(attr.Value, out PosZ);

                        if (File.Exists(LoadPrefabFullName))
                        {
                            GameObject LoadPrefabGameObject = (GameObject)Resources.Load(LoadPrefabShortName);
                            GameObject Bonus = GameObject.Instantiate(LoadPrefabGameObject, RootGameObject.transform, true);
                            Bonus.transform.position = new Vector3(PosX, PosY, PosZ);

                        }
                        else
                        {
                            Debug.LogError($"Can't found {LoadPrefabFullName}");
                        }
                    }
                }
            }
            #endregion
            #region Read Roads
            XmlNodeList roadList = doc.GetElementsByTagName("Roads");
            for(int i = 0; i < roadList.Count; i++)
            {
                XmlNode RoadsRootNode = roadList.Item(i);
                XmlAttribute attr;
                if (RoadsRootNode.HasChildNodes)
                {
                    for (int j = 0; j < RoadsRootNode.ChildNodes.Count; j++)
                    {
                        attr = (XmlAttribute)RoadsRootNode.ChildNodes[j].Attributes.GetNamedItem("Type");
                        string LoadPrefabFullName = Regex.Replace(attr.Value, @"\(.+\)", String.Empty);
                        string LoadPrefabShortName = "Prefabs/" + LoadPrefabFullName;
                        LoadPrefabFullName = "Assets/Resources/Prefabs/" + LoadPrefabFullName + ".prefab";

                        float PosX, PosY, PosZ;
                        attr = (XmlAttribute)RoadsRootNode.ChildNodes[j].Attributes.GetNamedItem("x");
                        float.TryParse(attr.Value, out PosX);
                        attr = (XmlAttribute)RoadsRootNode.ChildNodes[j].Attributes.GetNamedItem("y");
                        float.TryParse(attr.Value, out PosY);
                        attr = (XmlAttribute)RoadsRootNode.ChildNodes[j].Attributes.GetNamedItem("z");
                        float.TryParse(attr.Value, out PosZ);
                        string RoadTag;
                        attr = (XmlAttribute)RoadsRootNode.ChildNodes[j].Attributes.GetNamedItem("tag");
                        RoadTag = attr.Value;

                        if (File.Exists(LoadPrefabFullName))
                        {
                            GameObject LoadPrefabGameObject = (GameObject)Resources.Load(LoadPrefabShortName);
                            GameObject Road = GameObject.Instantiate(LoadPrefabGameObject);
                            Road.transform.position = new Vector3(PosX, PosY, PosZ);
                            Road.tag = RoadTag;

                        }
                        else
                        {
                            Debug.LogError($"Can't found {LoadPrefabFullName}");
                        }
                    }
                }
            }
            #endregion
        }
    }
}

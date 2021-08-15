using System;
using UnityEngine;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace Infinite_story
{
    public class LoadGame
    {
        List<GameObject> LoadedObjects;
        public static Action<List<GameObject>> OnSaveFileReaded;
        public static Action<int> SetScore;
        public static Action<int> SetScrollSpeed;
        public static Action<Vector3> SetPlayerPos;
       
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
        /// <summary>
        /// тут заменить реурсивным методом
        /// </summary>
        public void ReadSaveFile()
        {
            // читаем документ с диска
            XmlDocument doc = new XmlDocument();
            doc.Load(_saveGameFileName);
            LoadedObjects = new List<GameObject>();
            #region Read bonuses

            // Ищем элементы, где живут аттрибуты с нужными параметрами

            XmlNodeList elemList = doc.GetElementsByTagName("RootObject");

            for (int i = 0; i < elemList.Count; i++)
            {
                XmlNode ObjectRootNode = elemList.Item(i);
                GameObject RootGameObject = new GameObject();
                float RootPosX, RootPosY, RootPosZ;

                XmlAttribute RootObjAttrX = (XmlAttribute)ObjectRootNode.Attributes.GetNamedItem("x");
                float.TryParse(RootObjAttrX.Value, out RootPosX);
                XmlAttribute RootObjAttrY = (XmlAttribute)ObjectRootNode.Attributes.GetNamedItem("y");
                float.TryParse(RootObjAttrY.Value, out RootPosY);
                XmlAttribute RootObjAttrZ = (XmlAttribute)ObjectRootNode.Attributes.GetNamedItem("z");
                float.TryParse(RootObjAttrZ.Value, out RootPosZ);
                XmlAttribute RootObjTag = (XmlAttribute)ObjectRootNode.Attributes.GetNamedItem("tag");
                RootGameObject.tag = RootObjTag.Value;

                RootGameObject.transform.position = new Vector3(RootPosX, RootPosY, RootPosZ);

                // Вытаскиваем дочерние ноды, из них аттрибуты и спауним бонусы
                if (ObjectRootNode.HasChildNodes)
                {
                    for (int j = 0; j < ObjectRootNode.ChildNodes.Count; j++)
                    {
                        
                        XmlAttribute BonusBlankName = (XmlAttribute)ObjectRootNode.ChildNodes[j].Attributes.GetNamedItem("Name");
                        GameObject BonusBlank = new GameObject(BonusBlankName.Value);
                        XmlAttribute BonusBlankTag = (XmlAttribute)ObjectRootNode.ChildNodes[j].Attributes.GetNamedItem("tag");
                        BonusBlank.tag = BonusBlankTag.Value;

                        float BonusBlankPosX, BonusBlankPosY, BonusBlankPosZ;
                        XmlAttribute BonusBlankX = (XmlAttribute)ObjectRootNode.ChildNodes[j].Attributes.GetNamedItem("x");
                        float.TryParse(BonusBlankX.Value, out BonusBlankPosX);
                        XmlAttribute BonusBlankY = (XmlAttribute)ObjectRootNode.ChildNodes[j].Attributes.GetNamedItem("y");
                        float.TryParse(BonusBlankY.Value, out BonusBlankPosY);
                        XmlAttribute BonusBlankZ = (XmlAttribute)ObjectRootNode.ChildNodes[j].Attributes.GetNamedItem("z");
                        float.TryParse(BonusBlankZ.Value, out BonusBlankPosZ);
                        BonusBlank.transform.position = new Vector3(BonusBlankPosX, BonusBlankPosY, BonusBlankPosZ);
                        BonusBlank.transform.SetParent(RootGameObject.transform, true);
                    }
                }
                LoadedObjects.Add(RootGameObject);
            }
            #endregion
            #region Read Roads
            XmlNodeList roadList = doc.GetElementsByTagName("Roads");
            for(int i = 0; i < roadList.Count; i++)
            {
                XmlNode RoadsRootNode = roadList.Item(i);
                if (RoadsRootNode.HasChildNodes)
                {
                    for (int j = 0; j < RoadsRootNode.ChildNodes.Count; j++)
                    {
                        XmlAttribute RoadName = (XmlAttribute)RoadsRootNode.ChildNodes[j].Attributes.GetNamedItem("Name");
                        GameObject LoadedRoad = new GameObject(RoadName.Value);
                        XmlAttribute RoadTag = (XmlAttribute)RoadsRootNode.ChildNodes[j].Attributes.GetNamedItem("tag");
                        LoadedRoad.tag = RoadTag.Value;

                        float LoadedRoadPosX, LoadedRoadPosY, LoadedRoadPosZ;
                        XmlAttribute RoadPosX = (XmlAttribute)RoadsRootNode.ChildNodes[j].Attributes.GetNamedItem("x");
                        float.TryParse(RoadPosX.Value, out LoadedRoadPosX);
                        XmlAttribute RoadPosY = (XmlAttribute)RoadsRootNode.ChildNodes[j].Attributes.GetNamedItem("y");
                        float.TryParse(RoadPosY.Value, out LoadedRoadPosY);
                        XmlAttribute RoadPosZ = (XmlAttribute)RoadsRootNode.ChildNodes[j].Attributes.GetNamedItem("z");
                        float.TryParse(RoadPosZ.Value, out LoadedRoadPosZ);
                        LoadedRoad.transform.position = new Vector3(LoadedRoadPosX, LoadedRoadPosY, LoadedRoadPosZ);
                        LoadedObjects.Add(LoadedRoad);
                        
                    }
                }
            }
            #endregion
            // Читаем общую информацию об игре
            XmlNodeList GameInfoNodeList = doc.GetElementsByTagName("GameInfo");
            int GameScore = -1;
            int ScrollSpeed = -1;
            Vector3 PlayerPos = new Vector3();
            for (int i = 0; i < GameInfoNodeList.Count; i++)
            {
                XmlNode GameInfoNode = GameInfoNodeList.Item(i);
                XmlAttribute GameInfoScore = (XmlAttribute)GameInfoNode.Attributes.GetNamedItem("Score");
                XmlAttribute GameinfoScrollSpeed = (XmlAttribute)GameInfoNode.Attributes.GetNamedItem("ScrollSpeed");
                XmlAttribute PosX = (XmlAttribute)GameInfoNode.Attributes.GetNamedItem("PlayerX");
                XmlAttribute PosY = (XmlAttribute)GameInfoNode.Attributes.GetNamedItem("PlayerY");
                XmlAttribute PosZ = (XmlAttribute)GameInfoNode.Attributes.GetNamedItem("PlayerZ");
                
                float.TryParse(PosX?.Value, out PlayerPos.x);
                float.TryParse(PosY?.Value, out PlayerPos.y);
                float.TryParse(PosZ?.Value, out PlayerPos.z);

                Int32.TryParse(GameInfoScore?.Value, out GameScore);
                Int32.TryParse(GameinfoScrollSpeed?.Value, out ScrollSpeed);
            }
            if (GameScore > 0) SetScore?.Invoke(GameScore);
            if (ScrollSpeed > 0) SetScrollSpeed?.Invoke(ScrollSpeed);
            SetPlayerPos?.Invoke(PlayerPos);

            OnSaveFileReaded?.Invoke(LoadedObjects);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO.Compression;

namespace Infinite_story
{

    public class SaveGame
    {
        public SaveGame(string file)
        {
            _filename = file;
        }

        private string _filename;

        private int _score;
        private int _scrollSpeed;
        private string _time;

        // Список объектов с респаунеными бонусами
        private List<BonusesSpawner> ListOfBonusesRoots;
        private List<GameObject> _roadsList;

        private GameObject _player;

        public void Awake()
        {
            PlayerController.currentScore += GetScore;
            PlayerController.NewSpeed += OnSpeedChanged;
            PlayerController.SetPlayer += SetPlayer;
            UIController.SendTimeToSaveFile += SetGameTime;
            ListOfBonusesRoots = new List<BonusesSpawner>();
            _roadsList = new List<GameObject>();
            FloorController.SetBonuses += GetBonuses;
            FloorController.SetRoads += GetRoads;
            FloorController.Save += Save;
        }


        public void OnDestroy()
        {
            PlayerController.currentScore -= GetScore;
            PlayerController.NewSpeed -= OnSpeedChanged;
            PlayerController.SetPlayer -= SetPlayer;
            UIController.SendTimeToSaveFile -= SetGameTime;
            FloorController.SetBonuses -= GetBonuses;
            FloorController.SetRoads -= GetRoads;
            FloorController.Save -= Save;
        }

        public void GetBonuses(List<BonusesSpawner> bonuses)
        {
            ListOfBonusesRoots = bonuses;
        }

        public void GetRoads(List<GameObject> roads)
        {
            _roadsList = roads;
        }

        private void SetPlayer(GameObject Player)
        {
            _player = Player;
        }


        public void SetGameTime(string NewTime)
        {
            _time = NewTime;
        }

        public void OnSpeedChanged(int Speed)
        {
            _scrollSpeed = Speed;
        }

        void GetScore(int Score)
        {
            _score = Score;
        }

        public void Save()
        {
            if (ListOfBonusesRoots.Count > 0)
            {
                XmlDocument SaveFile = new XmlDocument();
                XmlDeclaration xmldecl;
                xmldecl = SaveFile.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                // добавляем блок с бонусами
                XmlNode RootNode = SaveFile.CreateElement("SaveGame");

                SaveFile.AppendChild(RootNode);
                SaveFile.InsertBefore(xmldecl, RootNode);
                XmlNode ObjectsRootNode = SaveFile.CreateElement("RootObjects");
                foreach (BonusesSpawner myGo in ListOfBonusesRoots)
                {
                    XmlElement NestObjElement = SaveFile.CreateElement("RootObject");
                    NestObjElement.SetAttribute("tag", myGo.RootBonusesObjects.tag);
                    NestObjElement.SetAttribute("x", myGo.RootBonusesObjects.transform.position.x.ToString());
                    NestObjElement.SetAttribute("y", myGo.RootBonusesObjects.transform.position.y.ToString());
                    NestObjElement.SetAttribute("z", myGo.RootBonusesObjects.transform.position.z.ToString());

                    for (int i = 0; i < myGo.RootBonusesObjects.transform.childCount; i++)
                    {
                        XmlElement GameObjEl = SaveFile.CreateElement("GameObject");

                        Transform childTransform = myGo.RootBonusesObjects.transform.GetChild(i);

                        GameObject goChild = childTransform.gameObject;
                        GameObjEl.SetAttribute("Name", goChild.name);
                        GameObjEl.SetAttribute("tag", goChild.tag);
                        GameObjEl.SetAttribute("x", childTransform.position.x.ToString());
                        GameObjEl.SetAttribute("y", childTransform.position.y.ToString());
                        GameObjEl.SetAttribute("z", childTransform.position.z.ToString());
                        GameObjEl.SetAttribute("RotX", childTransform.transform.localEulerAngles.x.ToString());
                        GameObjEl.SetAttribute("RotY", childTransform.transform.localEulerAngles.y.ToString());
                        GameObjEl.SetAttribute("RotZ", childTransform.transform.localEulerAngles.z.ToString());

                        NestObjElement.AppendChild(GameObjEl);

                    }
                    ObjectsRootNode.AppendChild(NestObjElement);
                }
                XmlNode RoadsNode = SaveFile.CreateElement("Roads");
                foreach (GameObject road in _roadsList)
                {
                    XmlElement RoadEl = SaveFile.CreateElement("Road");
                    RoadEl.SetAttribute("tag", road.tag);
                    RoadEl.SetAttribute("Name", road.name);
                    RoadEl.SetAttribute("x", road.transform.position.x.ToString());
                    RoadEl.SetAttribute("y", road.transform.position.y.ToString());
                    RoadEl.SetAttribute("z", road.transform.position.z.ToString());
                    RoadsNode.AppendChild(RoadEl);

                }
                RootNode.AppendChild(RoadsNode);
                XmlElement GameInfoElement = SaveFile.CreateElement("GameInfo");
                GameInfoElement.SetAttribute("ScrollSpeed", _scrollSpeed.ToString());
                GameInfoElement.SetAttribute("Score", _score.ToString());
                GameInfoElement.SetAttribute("PlayerX", _player.transform.position.x.ToString());
                GameInfoElement.SetAttribute("PlayerY", _player.transform.position.y.ToString());
                GameInfoElement.SetAttribute("PlayerZ", _player.transform.position.z.ToString());
                //GameInfoElement.SetAttribute("Time", _time);
                RootNode.AppendChild(ObjectsRootNode);
                RootNode.AppendChild(GameInfoElement);

                SaveFile.Save(_filename);
            }

        }
    }

}

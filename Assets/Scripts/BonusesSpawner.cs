using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infinite_story
{
    public class BonusesSpawner
    
    {
        /// <summary>
        /// Spawn bonuses to map
        /// </summary>
        /// <param name="GoodBonusesNum">Good bonuses quantity</param>
        /// <param name="BabBonusesNum">Bad bonuses quantity</param>
        /// <param name="TrapsNum">Traps quantity</param>
        /// <param name="GridX">Cells for bonuses along X axis</param>
        /// <param name="GridZ">Cells for bonuses along Z axis</param>
        /// <param name="goodbonuses">List of GameObject as Good bonuses</param>
        /// <param name="badbonuses">List of GameObject as Bad bonuses</param>
        /// <param name="mods">List of GameObject as Player's Modificator</param>
        /// <param name="traps">List of GameObject as Traps</param>
        /// <param name="RoadObject">Object where represent sizes of road</param>
        public BonusesSpawner(
            List<GameObject> goodbonuses,
            int GoodBonusesNum,
            List<GameObject> badbonuses,
            int BabBonusesNum,
            List<GameObject> mods,
            int modsNum,
            List<GameObject> traps,
            int TrapsNum, 
            int GridX, 
            int GridZ,
            GameObject RoadObject
            ) 
        {
            GoodBonusesNumber = GoodBonusesNum;
            BadBonusesNumber = BabBonusesNum;
            TrapNumber = TrapsNum;
            GridSizeX = GridX;
            GridSizeZ = GridZ;
            BadBonuses = badbonuses;
            GoodBonuses = goodbonuses;
            Modificators = mods;
            ModsNumber = modsNum; 
            Traps = traps;
            RoadObj = RoadObject;
        }
        /// <summary>
        /// If you want load game, use this constructor
        /// </summary>
        /// <param name="LoadedGameObj">Already Spawned GameObject</param>
        public BonusesSpawner(GameObject LoadedGameObj)
        {
            RootBonusesObjects = LoadedGameObj;
        }
        public int GridSizeX;
        public int GridSizeZ;

        public int GoodBonusesNumber;
        public int BadBonusesNumber;
        public int ModsNumber;
        public int TrapNumber;

        public List<GameObject> BadBonuses;
        public List<GameObject> GoodBonuses;
        public List<GameObject> Modificators;
        public List<GameObject> Traps;
        public GameObject RoadObj;

        public static System.Action OnSpawnBonuses;

        private List<Vector3> _spawnCoords;
        private Vector3 _colliderBeginPoint;

        private Collider _bounds;

        // Объект, к которому привязаны бонусы
        public GameObject RootBonusesObjects;

        public void SpawnBonuses()
        {
            // создаём новый GameObject, чтобы привязать к нему бонусы этого спавна
            RootBonusesObjects = new GameObject();
            // Поищем в списках тэгиобъектов, чтобы потом назодить и удалять при загрузки объекты со сцены
            if(BadBonuses.Count > 0)
            {
                RootBonusesObjects.tag = BadBonuses[0].gameObject.tag;
            }
            else if (GoodBonuses.Count > 0)
            {
                RootBonusesObjects.tag = GoodBonuses[0].gameObject.tag;
            }
            else if (Modificators.Count > 0)
            {
                RootBonusesObjects.tag = Modificators[0].gameObject.tag;
            }
            else if (Traps.Count > 0)
            {
                RootBonusesObjects.tag = Traps[0].gameObject.tag;
            }
            _bounds = RoadObj.GetComponent<BoxCollider>();

            // начало координат BoxCollider у пола
            _colliderBeginPoint = new Vector3(_bounds.bounds.center.x - _bounds.bounds.extents.x,
            _bounds.bounds.center.y + _bounds.bounds.extents.y,
            _bounds.bounds.center.z - _bounds.bounds.extents.z);


            // Делим площадку на сетку.
            float pointsX = _bounds.bounds.size.x / GridSizeX;
            float pointsZ = _bounds.bounds.size.z / GridSizeZ;
            _spawnCoords = new List<Vector3>();

            // разбиваем площадку на сетку, в которой будут респаунится бонусы
            for (int i = 0; i < GridSizeX; i++)
            {
                for (int j = 0; j < GridSizeZ; j++)
                {
                    // самая левая кромка поля нас не интересует
                    if (i > 0)
                    {
                        Vector3 currentPoint = new Vector3(_colliderBeginPoint.x + pointsX * i, _colliderBeginPoint.y, _colliderBeginPoint.z + pointsZ * j);
                        _spawnCoords.Add(currentPoint);
                        //Debug.Log(currentPoint);
                    }
                }
            }

            
            RootBonusesObjects.transform.position = Vector3.zero;

            BonusesSpawn(BadBonuses, BadBonusesNumber, RootBonusesObjects);
            BonusesSpawn(GoodBonuses, GoodBonusesNumber, RootBonusesObjects);
            BonusesSpawn(Traps, TrapNumber, RootBonusesObjects);
            BonusesSpawn(Modificators, ModsNumber, RootBonusesObjects);
            OnSpawnBonuses?.Invoke();
        }
                       
        public void BonusesSpawn(List<GameObject> bonusesList, int bonusesNum, GameObject parent)
        {
            if (bonusesList.Count > 0 && _spawnCoords.Count > 0)
            {
                try
                {
                    for (int i = 0; i < bonusesNum; i++)
                    {
                        // Количество типов бонусов в списке (задаётся в редакторе)
                        int BonusesTypeNum = Random.Range(0, bonusesList.Count);
                        // случайно выбираем координаты
                        int BonusInstanceCoords = Random.Range(0, _spawnCoords.Count);
                        Vector3 spawnInstanceCoords = _spawnCoords[BonusInstanceCoords];
                        // спавним префаб над полом
                        GameObject bonusInstance = GameObject.Instantiate(bonusesList[BonusesTypeNum], parent.transform, true);
                        Renderer instanceRend = bonusInstance.GetComponent<Renderer>();
                        bonusInstance.transform.position = spawnInstanceCoords;
                        bonusInstance.transform.position = new Vector3(
                            bonusInstance.transform.position.x,
                        bonusInstance.transform.position.y + instanceRend.bounds.extents.y,
                        bonusInstance.transform.position.z);
                        // удаляем координаты из списка после спавна, чтобы на этом месте ещё что-то не появилось
                        _spawnCoords.Remove(spawnInstanceCoords);
                    }
                    
                }
                catch (ArgumentOutOfRangeException)
                {
                    Debug.LogError($"Бонусов больше, чем ячеек");
                }
            }
        }

        public void Update(int ScrollSpeed)
        {
            RootBonusesObjects.transform.Translate(-Vector3.forward * ScrollSpeed * Time.deltaTime);
            
        }
    }
}
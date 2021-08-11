using System;
using UnityEngine;

namespace Infinite_story
{
    public class CamConroller 
    {
        public Camera MainCamera;
        private GameObject _spawnedRoad;
        private Transform _player;
        private float _widgthLimit, _halfWidth, _boundsCenterX;
        private BoxCollider _roadCollider;
        /// <summary>
        /// Control camera
        /// </summary>
        /// <param name="player">Player Transform</param>
        /// <param name="cam">Camera</param>
        /// <param name="Road">Prevously spawned road</param>
        public CamConroller(Transform player, Camera cam, GameObject Road)
        {
            MainCamera = cam;
            _spawnedRoad = Road;
            _player = player;
        }
        
        public void Start()
        {
            MainCamera.transform.position = new Vector3(_player.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z);
            
            try 
            {

                _spawnedRoad.TryGetComponent<BoxCollider>(out _roadCollider);
                _boundsCenterX = _roadCollider.bounds.center.x;
                _halfWidth = _roadCollider.bounds.size.x / 2;
                
                //_roadBounces = FloorScroller._bounds.bounds.center;
                //_cam.transform.position = new Vector3(_roadBounces.x, _cam.transform.position.y, _cam.transform.position.z);
                
                
                }
            catch (NullReferenceException e)
            {
                Debug.LogError(e.Message);
            }

            
        }

        public void Update()
        {
            
                _widgthLimit = Mathf.Clamp(_player.position.x, _boundsCenterX - _halfWidth, _boundsCenterX + _halfWidth);
                MainCamera.transform.position = new Vector3(_widgthLimit, MainCamera.transform.position.y, MainCamera.transform.position.z);
           
            
        }
    }

}

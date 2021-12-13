using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infinite_story
{


    [CreateAssetMenu(fileName = "New Bonuses Data", menuName = "Bonuses Data", order = 51)]
    public class BonusesData : ScriptableObject
    {
        [Header("Bonuses setup")]
        [SerializeField] private List<GameObject> _goodBonuses;
        [SerializeField] [Tooltip("Min quantity of Good Bonuses at each Road segment")] private int _goodBonusesQuantityMin;
        [SerializeField] [Tooltip("Max quantity of Good Bonuses at each Road segment")] private int _goodBonusesQuantityMax;
        [SerializeField] private List<GameObject> _badBonuses;
        [SerializeField] [Tooltip("Min quantity of Bad Bonuses at each Road segment")] private int _badBonusesQuantityMin;
        [SerializeField] [Tooltip("Max quantity of Bad Bonuses at each Road segment")] private int _badBonusesQuantityMax;
        [SerializeField] private List<GameObject> _playerModificators;
        [SerializeField] [Tooltip("Min quantity of Player Modificators at each Road segment")] private int _playerModificatorsQuantityMin;
        [SerializeField] [Tooltip("Max quantity of Player Modificators at each Road segment")] private int _playerModificatorsQuantityMax;
        [SerializeField] private List<GameObject> _traps;
        [SerializeField] [Tooltip("Min quantity of Traps at each Road segment")] private int _trapsQuantityMin;
        [SerializeField] [Tooltip("Max quantity of Traps at each Road segment")] private int _trapsQuantityMax;
        [Header("Grid Setup")]
        [SerializeField] private int _gridSizeX;
        [SerializeField] private int _gridSizeZ;

        public List<GameObject> GoodBonuses
        {
            get
            {
                return _goodBonuses;
            }
        }

        public int GoodBonusesQuantityMin
        {
            get
            {
                return _goodBonusesQuantityMin;
            }
        }

        public int GoodBonusesQuantityMax
        {
            get
            {
                return _goodBonusesQuantityMax;
            }
        }

        public List<GameObject> BadBonuses
        {
            get
            {
                return _badBonuses;
            }
        }

        public int BadBonusesQuantityMin
        {
            get
            {
                return _badBonusesQuantityMin;
            }
        }

        public int BadBonusesQuantityMax
        {
            get
            {
                return _badBonusesQuantityMax;
            }
        }

        public List<GameObject> PlayerModificators
        {
            get
            {
                return _playerModificators;
            }
        }

        public int PlayerModificatorsQuantityMin
        {
            get
            {
                return _playerModificatorsQuantityMin;
            }
        }

        public int PlayerModificatorsQuantityMax
        {
            get
            {
                return _playerModificatorsQuantityMax;
            }
        }

        public List<GameObject> Traps
        {
            get
            {
                return _traps;
            }
        }

        public int TrapsQuantityMin
        {
            get
            {
                return _trapsQuantityMin;
            }
        }

        public int TrapsQuantityMax
        {
            get
            {
                return _trapsQuantityMax;
            }
        }

        public int GridSizeX
        {
            get
            {
                return _gridSizeX;
            }
        }

        public int GridSizeZ
        {
            get
            {
                return _gridSizeZ;
            }
        }
    }
}
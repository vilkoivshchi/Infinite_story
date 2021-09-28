using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBonuses : MonoBehaviour
{

    public RoadBonuses(List<GameObject> goodBonuses, List<GameObject> badBonuses, List<GameObject> modificators, List<GameObject> traps)
    {
        _goodBonuses = goodBonuses;
        _badBonuses = badBonuses;
        _modificators = modificators;
        _traps = traps;
    }

    private List<GameObject> _goodBonuses;
    private List<GameObject> _badBonuses;
    private List<GameObject> _modificators;
    private List<GameObject> _traps;

    public List<GameObject> GoodBonuses 
    { 
        get 
        { 
            return _goodBonuses; 
        } 
        set 
        { 
            _goodBonuses = value; 
        } 
    }

    public List<GameObject> BadBonuses
    {
        get
        {
            return _badBonuses;
        }
        set
        {
            _badBonuses = value;
        }
    }

    public List<GameObject> PlayerModificators
    {
        get
        {
            return _modificators;
        }
        set
        {
            _modificators = value;
        }
    }

    public List<GameObject> Traps
    {
        get
        {
            return _traps;
        }
        set
        {
            _traps = value;
        }
    }

    
}


using UnityEngine;

public interface IBonusFactory 
{
    GameObject CreateBonus(GameObject prefab, Vector3 pos, GameObject parent = null);
}

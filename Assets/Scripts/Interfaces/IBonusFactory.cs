
using UnityEngine;

public interface IBonusFactory 
{
    GameObject CreateBonus(GameObject prefab, Vector3 position, Quaternion rotation, GameObject parent = null);
}

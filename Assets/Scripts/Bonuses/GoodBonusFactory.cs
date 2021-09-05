using UnityEngine;

public class GoodBonusFactory : IBonusFactory
{
    public GameObject CreateBonus(GameObject prefab, Vector3 position, GameObject parent)
    {
        GameObject SpawnedBonus = GameObject.Instantiate(prefab, position, Quaternion.identity, parent.transform);

        return SpawnedBonus;
    }
}

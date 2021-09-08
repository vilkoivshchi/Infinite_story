using UnityEngine;

public class GoodBonusFactory : IBonusFactory
{
    public GameObject CreateBonus(GameObject prefab, Vector3 position, Quaternion rotation, GameObject parent = null)
    {
        Renderer bonusRender;
        prefab.TryGetComponent<Renderer>(out bonusRender);
        if(bonusRender != null)
        {
            position = new Vector3(position.x, position.y + bonusRender.bounds.extents.y, position.z);
        }

        GameObject SpawnedBonus;
        if(parent != null)
        {
            SpawnedBonus = GameObject.Instantiate(prefab, position, rotation, parent.transform);
        }
        else
        {
            SpawnedBonus = GameObject.Instantiate(prefab, position, rotation);
        }
        

        return SpawnedBonus;
    }
}

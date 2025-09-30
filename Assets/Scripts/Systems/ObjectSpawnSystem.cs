using UnityEngine;

namespace Systems
{
    public class ObjectSpawnSystem
    {
        public static void Spawn(GameObject obj, Vector3 position = new())
        {
            Object.Instantiate(obj, position, Quaternion.identity);
        }
    }
}
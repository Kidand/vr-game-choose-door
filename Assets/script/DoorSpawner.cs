using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    private bool doorSpawned = false;

    public void SpawnDoor()
    {
        Debug.Log("create doors!!");
        if (doorSpawned) return;

        GameObject doorPrefab = Resources.Load<GameObject>("CorridorGame");
        if (doorPrefab != null)
        {
            Vector3 position = new Vector3(0, 0, 0);
            Quaternion rotation = Quaternion.Euler(0, 0, 0);

            Instantiate(doorPrefab, position, rotation);
            doorSpawned = true;
        }
        else
        {
            Debug.LogError("Swinging door prefab not found in Resources folder.");
        }
    }
}

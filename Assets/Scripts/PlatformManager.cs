using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager instance;

    public GameObject platformPrefab;
    public GameObject platformPrefabHydro1;
    public GameObject platformPrefabHydro2;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public IEnumerator RespawnPlatform(RespawnData data)
    {
        yield return new WaitForSeconds(data.respawnTime);

        switch (data.type)
        {
            case "hydro1":
                Instantiate(platformPrefabHydro1, data.pos, platformPrefab.transform.rotation);
                break;
            case "hydro2":
                Instantiate(platformPrefabHydro2, data.pos, platformPrefab.transform.rotation);
                break;
            case "default":
                Instantiate(platformPrefab, data.pos, platformPrefab.transform.rotation);
                break;
        }

    }
}

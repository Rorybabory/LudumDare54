using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public GameObject prefabObj;

    // Start is called before the first frame update
    void Start()
    {
        // test
        ///Vector3 testPos =  Vector3.zero;
        // Quaternion testRot = Quaternion.identity;
        // Transform testTransform = this.transform.Find("spawnpoint");
        // Spawn(testPos, testRot, testTransform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(Vector3 pos = default, Quaternion rotats = default, Transform spawnpoint = null)
    {
        GameObject spawnedObject = Instantiate(prefabObj, default, rotats, spawnpoint);
        spawnedObject.transform.localPosition = pos;
    }

    public void Spawn()
    {
        this.Spawn(default);
    }
}

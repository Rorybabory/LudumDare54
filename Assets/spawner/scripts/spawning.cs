using UnityEngine;
using UnityEngine.UIElements;

public class spawning : MonoBehaviour
{
    public GameObject prefabObj;
    // Start is called before the first frame update
    void Start()
    {
        // test
        // Vector3 testPos =  Vector3.zero;
        // Quaternion testRot = Quaternion.identity;
        // Spawn(testPos, testRot);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(Vector3 pos, Quaternion rotats)
    {
        GameObject spawnedObject = Instantiate(prefabObj, pos, rotats);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuild : MonoBehaviour, IBuild {
    public GameObject soldierPrefab;
    public void Build(Units unit) {
        if(unit == Units.SOLDIER) {
            Debug.Log("Build soldier");

            Vector3 pos = gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
            GameObject.Instantiate(soldierPrefab, pos, Quaternion.identity);
        }
    }
}
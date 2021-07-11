using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    FirebaseManager firebase;

    private void Start()
    {
        firebase = GetComponent<FirebaseManager>();
    }
    public void save()
    {
        firebase.SaveData();
    }
}

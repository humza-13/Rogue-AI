
using UnityEngine;
using UnityEngine.SceneManagement;


public class SignOut : MonoBehaviour
{
    FirebaseManager firebase;

    private void Start()
    {
        firebase = GetComponent<FirebaseManager>();
    }
    public void LogOut()
    {
        firebase.SignOut();
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;


public class SignOut : MonoBehaviour
{
    LevelSelectorManager levelSelector;
    FirebaseManager firebase;

    private void Start()
    {
        levelSelector = FindObjectOfType<LevelSelectorManager>();
        firebase = GetComponent<FirebaseManager>();
    }
    public void LogOut()
    {
        levelSelector.isSignedin = false;
        SceneManager.LoadScene("Login");
        firebase.SignOut();
    }
   
}

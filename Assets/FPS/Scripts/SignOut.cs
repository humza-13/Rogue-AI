
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignOut : MonoBehaviour
{
  
    public void signOut()
    {
        SceneManager.LoadScene("Login");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UserManager : MonoBehaviour
{
    FirebaseManager firebase;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [Header("Views")]
    public GameObject objectToToggle_1;
    public GameObject objectToToggle_2;
    private void Start()
    {
        firebase = GetComponent<FirebaseManager>();

        warningLoginText.text = " ";
        warningRegisterText.text = " ";
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(firebase.Login(emailLoginField, passwordLoginField, warningLoginText));
    }

    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(firebase.Register(emailRegisterField, passwordRegisterField, passwordRegisterVerifyField, usernameRegisterField, warningRegisterText, objectToToggle_1, objectToToggle_2));
    }
}

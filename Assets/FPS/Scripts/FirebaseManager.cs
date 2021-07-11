using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Database;


public class FirebaseManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;
    public DatabaseReference DBreference;


    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    //Function for the save button
    public void SaveData()
    {
        
        StartCoroutine(UpdateUsernameAuth(auth.CurrentUser.DisplayName.ToString()));
        StartCoroutine(UpdateLogicPoints(PlayerPrefs.GetInt("LogicPoints")));
        StartCoroutine(UpdateKillPoints(PlayerPrefs.GetInt("KillPoints")));
        //PlayerPrefs.SetInt("LevelReached", PlayerPrefs.GetInt("LevelReached") + 1);
        StartCoroutine(UpdateLevel(PlayerPrefs.GetInt("LevelReached")));
        
       

    }


    public IEnumerator Login(TMP_InputField _email, TMP_InputField _password, TMP_Text _warning)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email.text, _password.text);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }

            _warning.text = message;
            
        }
        else
        {
            //logged in 
           
            _warning.text = "Logging In...";
            StartCoroutine(LoadUserData());
            yield return new WaitForSeconds(2);
            ClearLoginFeilds(_email, _password);
            ChangeScene();
            _warning.text = "";
        }
    }

 

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };
        
        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = auth.CurrentUser.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);
      
        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
       
    }

    private IEnumerator UpdateLogicPoints(int _logicPoints)
    {
        
        //Set the currently logged in user logic points
        var DBTask = DBreference.Child("users").Child(auth.CurrentUser.UserId).Child("logicPoints").SetValueAsync(_logicPoints);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
      
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
       
    }

    private IEnumerator UpdateKillPoints(int _killPoints)
    {
        //Set the currently logged in user kill points
        var DBTask = DBreference.Child("users").Child(auth.CurrentUser.UserId).Child("killPoints").SetValueAsync(_killPoints);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

    }

    private IEnumerator UpdateLevel(int _levelreached)
    {
        //Set the currently logged in user logic points
        var DBTask = DBreference.Child("users").Child(auth.CurrentUser.UserId).Child("levelReached").SetValueAsync(_levelreached);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

    }


    public void RefreashDb()
    {
        StartCoroutine(LoadUserData());
    }

    public IEnumerator LoadUserData()
    {
        
        //Get the currently logged in user data
        var DBTask = DBreference.Child("users").Child(auth.CurrentUser.UserId).GetValueAsync();
        
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            PlayerPrefs.SetInt("LogicPoints", 0);
            PlayerPrefs.SetInt("KillPoints", 0);
            PlayerPrefs.SetInt("LevelReached", 1);
            
           
            
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            PlayerPrefs.SetInt("LogicPoints", int.Parse(snapshot.Child("logicPoints").Value.ToString()));
            PlayerPrefs.SetInt("KillPoints", int.Parse(snapshot.Child("killPoints").Value.ToString()));
            PlayerPrefs.SetInt("LevelReached", int.Parse(snapshot.Child("levelReached").Value.ToString()));

           
            
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("LevelSelector");
    }
    public void ClearLoginFeilds(TMP_InputField _email, TMP_InputField _password)
    {
        _email.text = "";
        _password.text = "";
    }
    public void ClearRegisterFeilds(TMP_InputField _username,TMP_InputField _email, TMP_InputField _password, TMP_InputField _passwordre)
    {
        _username.text = "";
        _email.text = "";
        _password.text = "";
        _passwordre.text = "";
    }
    public void SignOut()
    {
        StopCoroutine(LoadUserData());
        auth.SignOut();
        SceneManager.LoadScene("Login");
    }

    public string getUserName()
    {
        return auth.CurrentUser.DisplayName;
    }
    public IEnumerator Register(TMP_InputField _email, TMP_InputField _password, TMP_InputField _passwordre , TMP_InputField _username, TMP_Text warningRegisterText, GameObject gameObject_1, GameObject gameObject_2)
    {
        if (_username.text == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
            
        }
        else if(_password.text != _passwordre.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
            
        }
        else 
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email.text, _password.text);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password, password should be atleat 8 charchter long";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
                
            }
            else
            {
                //User has now been created
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile{DisplayName = _username.text};

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                        
                    }
                    else
                    {
                        //Username is now set
                        ClearRegisterFeilds(_username, _email, _password, _passwordre);
                        gameObject_1.SetActive(false);
                        gameObject_2.SetActive(true);
                        warningRegisterText.text = "";
                    }
                }
            }
        }
    }
}

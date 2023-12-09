using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.Auth;
using Xsolla.Core;


    public class AuthorizationPage : MonoBehaviour
    {
        // Declaration of variables for UI elements
        public TMP_InputField UsernameInput;
        public TMP_InputField PasswordInputField;
        public Button SignInButton;

        private void Start()
        {
            // Handling the button click
            SignInButton.onClick.AddListener(() =>
            {
                // Get the username and password from input fields
                var username = UsernameInput.text;
                var password = PasswordInputField.text;

                // Call the user authorization method
                // Pass credentials and callback functions for success and error cases
                XsollaAuth.SignIn(username, password, OnSuccess, OnError);
            });
        }

        private void OnSuccess()
        {
            Debug.Log("Authorization successful");
            // Add actions taken in case of success
        }

        private void OnError(Error error)
        {
            Debug.LogError($"Authorization failed. Error: {error.errorMessage}");
            // Add actions taken in case of error
        }
    }

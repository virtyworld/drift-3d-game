using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.Auth;
using Xsolla.Core;

public class RegistrationPage : MonoBehaviour
{
    public TMP_InputField UsernameInput;
    public TMP_InputField EmailInputField;
    public TMP_InputField PasswordInputField;
    public Button RegisterButton;

    private void Start()
    {
        // Handling the button click
        RegisterButton.onClick.AddListener(() =>
        {
            // Get the username, email and password from input fields
            var username = UsernameInput.text;
            var email = EmailInputField.text;
            var password = PasswordInputField.text;

            // Call the user registration method
            // Pass credentials and callback functions for success and error cases
            XsollaAuth.Register(username, password, email, OnSuccess, OnError);
        });
    }

    private void OnSuccess(LoginLink loginLink)
    {
        Debug.Log("Registration successful");
        // Add actions taken in case of success
    }

    private void OnError(Error error)
    {
        Debug.LogError($"Registration failed. Error: {error.errorMessage}");
        // Add actions taken in case of error
    }
}
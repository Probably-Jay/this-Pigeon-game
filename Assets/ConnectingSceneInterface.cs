using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using NetSystem;
using System;

namespace SceneControl
{

    public class ConnectingSceneInterface : MonoBehaviour
    {
        [SerializeField] TMP_Text messagText;
        [SerializeField] Image loadingImage;
        [SerializeField] Button backButton;

        Coroutine logginInCoroutine;

        const float rotationSpeed = 100;

        private void Start()
        {
            logginInCoroutine = StartCoroutine(Login());
        }

        IEnumerator Login()
        {
            var response = new CallResponse();
            StartCoroutine(LoginWait(response));

            var loginCallbacks = new APIOperationCallbacks
                (
                onSucess: () => {
                    response.status.SetSucess();
                    LoginSucess();
                },
                onfailure: (errorReason) => { 
                    LoginFailure(errorReason);
                    response.status.SetError(errorReason);
                }
                ) ;

            if (!NetworkHandler.Instance._useDebugAcountLogin) // this can be removed after login
            {
                NetworkHandler.Instance.AnonymousLoginPlayer(loginCallbacks);

            }
            else
            {
                NetworkHandler.Instance.AnonymousLoginDebugPlayer(loginCallbacks);
            }

            yield return new WaitUntil(() => response.status.Complete);
        }

      

        IEnumerator LoginWait(CallResponse callResponse)
        {
            messagText.enabled = true;
            loadingImage.enabled = true;
            messagText.text = "Logging in...";
            while (!callResponse.status.Complete)
            {
                loadingImage.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private void LoginSucess()
        {
            messagText.enabled = true;
            messagText.text = "Logged in";
            loadingImage.enabled = false;
        }

        private void LoginFailure(FailureReason errorReason)
        {
            messagText.enabled = true;
            messagText.text = "Unable to log in... Please check your internet and try again later";
            loadingImage.enabled = false;
        }


    }
}

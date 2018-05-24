using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Http
{
    class Request : MonoBehaviour
    {

        private HomeRevalSession homeRevalSession;
  
        public Request()
        {
            homeRevalSession = HomeRevalSession.Instance;
        }

        public void Get(string url, string json)
        {
            StartCoroutine(getRequest(url, json));
        }

        public void Post(string url, string json)
        {
            StartCoroutine(postRequest(url, json));
        }

        private IEnumerator getRequest(string url, string json)
        {
            var uwr = new UnityWebRequest(url, "GET");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("Authorization", "Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJhZG1pbiIsImV4cCI6MTUyNTM0NTIwMn0.LINprnMRZeaphdaYXB8e7BldpFs0FK7XcxheTdWwqP5q-wJ3BLMTZ_bgcbyKXhYwyq6d-gMtm2jtK0DYo_GjqQ");

            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
        }

        private IEnumerator postRequest(string url, string json)
        {
            var uwr = new UnityWebRequest(url, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("Authorization", "Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJhZG1pbiIsImV4cCI6MTUyNTM0NTIwMn0.LINprnMRZeaphdaYXB8e7BldpFs0FK7XcxheTdWwqP5q-wJ3BLMTZ_bgcbyKXhYwyq6d-gMtm2jtK0DYo_GjqQ");

            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
        }
    }
}

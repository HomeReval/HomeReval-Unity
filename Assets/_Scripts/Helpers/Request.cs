using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Helpers
{
    class Request
    {
        public delegate void PostCompleted(string response);
        public delegate void PostError(string response);
        //public delegate void GetCompleted(string response);
        //public delegate void GetError(string response);


        private List<PostCompleted> postCompletedHandlers = new List<PostCompleted>();
        private List<PostError> postErrorHandlers = new List<PostError>();
        //private List<GetCompleted> getCompletedHandlers = new List<GetCompleted>();
        //private List<GetError> getErrorHandlers = new List<PostCompleted>();

        private HomeRevalSession homeRevalSession;
        private const string API = "http://homereval.ga:5000/api";
  
        public Request()
        {            
            homeRevalSession = HomeRevalSession.Instance;
        }

        public void AddPostCompletedHandler(PostCompleted p)
        {
            postCompletedHandlers.Add(p);
        }

        public void AddPostErrorHandler(PostError p)
        {
            postErrorHandlers.Add(p);
        }

        public IEnumerator Get(string path, string json)
        {
            return getRequest(path, json);
        }

        public IEnumerator Post(string path, string json, Action<string> success, Action<string> error)
        {
            return postRequest(path, json, success, error);
        }

        private IEnumerator getRequest(string path, string json)
        {
            var uwr = new UnityWebRequest(API + path, "GET");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            // Add token as header if exists
            if (string.IsNullOrEmpty(homeRevalSession.Token))
                uwr.SetRequestHeader("Authorization", "Bearer " + homeRevalSession.Token);

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

        private IEnumerator postRequest(string path, string json, Action<string> success, Action<string> error)
        {
            var uwr = new UnityWebRequest(API + path, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            // Add token as header if exists
            if(string.IsNullOrEmpty(homeRevalSession.Token))
                uwr.SetRequestHeader("Authorization", "Bearer " + homeRevalSession.Token);

            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                error("Error While Sending: " + uwr.error);
            }
            else
            {
                success("Received: " + uwr.downloadHandler.text);
            }

        }
    }
}

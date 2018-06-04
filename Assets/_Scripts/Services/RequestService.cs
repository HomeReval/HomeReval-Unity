using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace HomeReval.Services
{
    class RequestService: IRequestService
    {
        private HomeRevalSession homeRevalSession;
        //private const string API = "http://homereval.ga:5000/api";
        private const string API = "http://localhost:58580/api";


        public RequestService()
        {            
            homeRevalSession = HomeRevalSession.Instance;
        }

        public IEnumerator Get(string path, /*string json,*/ Action<string> success, Action<string> error)
        {
            return getRequest(path, /*json,*/ success, error);
        }

        public IEnumerator Post(string path, string json, Action<string> success, Action<string> error)
        {
            return postRequest(path, json, success, error);
        }

        private IEnumerator getRequest(string path, /*string json,*/ Action<string> success, Action<string> error)
        {
            var uwr = new UnityWebRequest(API + path, "GET");
            //byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            //uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            // Add token as header if exists
            if (!string.IsNullOrEmpty(homeRevalSession.Token))
                uwr.SetRequestHeader("Authorization", "bearer " + homeRevalSession.Token);
                

            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                error(uwr.error);
            }
            else
            {
                Debug.Log(uwr.responseCode);
                
                success(uwr.downloadHandler.text);
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
            if(!string.IsNullOrEmpty(homeRevalSession.Token))
                uwr.SetRequestHeader("Authorization", "bearer " + homeRevalSession.Token);

            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                error(uwr.error);
            }
            else
            {
                Debug.Log(uwr.responseCode);

                success(uwr.downloadHandler.text);
            }

        }
    }
}

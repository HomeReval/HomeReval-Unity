using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Helpers
{
    class Request
    {

        private HomeRevalSession homeRevalSession;
        private const string API = "http://homereval.ga:5000/api";
  
        public Request()
        {            
            homeRevalSession = HomeRevalSession.Instance;
        }

        public IEnumerator Get(string path, string json)
        {
            return getRequest(path, json);
        }

        public IEnumerator Post(string path, string json)
        {
            return postRequest(path, json);
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

        private IEnumerator postRequest(string path, string json)
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
                Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
        }
    }
}

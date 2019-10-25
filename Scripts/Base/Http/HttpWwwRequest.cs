using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Proyecto26;
using UnityEngine.UI;

namespace GFrame
{
    public class HttpWwwRequest : TSingleton<HttpWwwRequest>
    {
        public void Get(string url, Action<UnityWebRequest> actionResult)
        {
            StaticCoroutine.StartCoroutine(_Get(url, actionResult));
        }


        public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
        {
            StaticCoroutine.StartCoroutine(_DownloadFile(url, downloadFilePathAndName, actionResult));
        }

        public void GetTexture(string url, Action<Texture2D> actionResult)
        {
            StaticCoroutine.StartCoroutine(_GetTexture(url, actionResult));
        }
        public void GetAssetBundle(string url, Action<AssetBundle> actionResult)
        {
            StaticCoroutine.StartCoroutine(_GetAssetBundle(url, actionResult));
        }

        public void GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
        {
            StaticCoroutine.StartCoroutine(_GetAudioClip(url, actionResult, audioType));
        }

        public void Post(string serverURL, WWWForm lstformData, Action<UnityWebRequest> actionResult)
        {
            StaticCoroutine.StartCoroutine(_Post(serverURL, lstformData, actionResult));
        }

        public void Delete(string url, Action<string> actionResult)
        {
            StaticCoroutine.StartCoroutine(_Delete(url, actionResult));
        }

        public void UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult)
        {
            StaticCoroutine.StartCoroutine(_UploadByPut(url, contentBytes, actionResult, ""));
        }

        public void UploadByPut(string url, string method, Dictionary<string, string> header, UploadHandler uploadHandler, Action<string> actionResult)
        {
            StaticCoroutine.StartCoroutine(_UploadByPut(url, method, header, uploadHandler, actionResult));
        }

        IEnumerator _Get(string url, Action<UnityWebRequest> actionResult)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(" end  " + www.error);
                }
                else
                {
                    if (actionResult != null)
                    {
                        actionResult(www);
                    }
                }
            }
        }

        IEnumerator _Delete(string url, Action<string> actionResult)
        {
            using (UnityWebRequest www = UnityWebRequest.Delete(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(" Completed  " + www.error);
                }
                else
                {
                    //  Debug.Log("删除结束");
                }
            }
        }

        IEnumerator _DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
        {
            var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);
            yield return uwr.SendWebRequest();
            if (actionResult != null)
            {
                actionResult(uwr);
            }
        }

        IEnumerator _GetTexture(string url, Action<Texture2D> actionResult)
        {
            UnityWebRequest uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            uwr.timeout = 30;
            uwr.downloadHandler = downloadTexture;
            yield return uwr.SendWebRequest();
            Texture2D t = null;
            if (!(uwr.isNetworkError || uwr.isHttpError))
            {
                t = downloadTexture.texture;
            }
            if (actionResult != null)
            {
                actionResult(t);
            }
        }

        IEnumerator _GetAssetBundle(string url, Action<AssetBundle> actionResult)
        {
            UnityWebRequest www = new UnityWebRequest(url);
            DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
            www.downloadHandler = handler;
            yield return www.SendWebRequest();
            AssetBundle bundle = null;
            if (!(www.isNetworkError || www.isHttpError))
            {
                bundle = handler.assetBundle;
            }
            if (actionResult != null)
            {
                actionResult(bundle);
            }
            string str = www.downloadHandler.text;
        }

        IEnumerator _GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
        {
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
            {
                yield return uwr.SendWebRequest();
                if (!(uwr.isNetworkError || uwr.isHttpError))
                {
                    if (actionResult != null)
                    {
                        actionResult(DownloadHandlerAudioClip.GetContent(uwr));
                    }
                }
            }
        }

        IEnumerator _Post(string serverURL, WWWForm lstformData, Action<UnityWebRequest> actionResult)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(serverURL, lstformData);
            yield return uwr.SendWebRequest();
            if (actionResult != null)
            {
                actionResult(uwr);
            }
        }

        IEnumerator _UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult, string contentType = "application/octet-stream")
        {
            UnityWebRequest uwr = new UnityWebRequest();
            UploadHandler uploader = new UploadHandlerRaw(contentBytes);

            uploader.contentType = contentType;

            uwr.uploadHandler = uploader;


            yield return uwr.SendWebRequest();

            bool res = true;
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                res = false;
            }
            if (actionResult != null)
            {
                actionResult(res);
            }
        }

        IEnumerator _UploadByPut(string url, string method, Dictionary<string, string> header, UploadHandler uploadHandler, Action<string> actionResult)
        {
            UnityWebRequest uwr = new UnityWebRequest(url, method);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            if (header != null && header.Count >= 0)
            {
                foreach (var key in header.Keys)
                {
                    uwr.SetRequestHeader(key, header[key]);
                }
            }
            uwr.uploadHandler = uploadHandler;

            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Log.e(uwr.error);
            }
            if (actionResult != null)
            {
                actionResult(uwr.downloadHandler.text);
            }
        }






    }
}
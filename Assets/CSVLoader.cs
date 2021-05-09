using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace Localisation
{
    internal class CSVLoader
    {
        
    

        const string fileName = "localisationData.csv";
        char lineSeperator = '\n';
        char[] delimiter = { '#' };


        FileStream file;
  

        public Dictionary<string, string> GetDictionary(string atributeId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string data = GetData();

            string[] lines = data.Split(lineSeperator);

            string[] headers = lines[0].Split(delimiter, System.StringSplitOptions.None);

            int attributeIndex = -1;

            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i].Contains(atributeId))
                {
                    attributeIndex = i;
                    break;
                }
            }

            if (attributeIndex == -1)
            {
                throw new System.Exception($"This language id {atributeId} is not in this dictaionary");
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line == "")
                {
                    continue;
                }


                string[] feilds = line.Split(delimiter, System.StringSplitOptions.None);



                var key = feilds[0];

                if (dict.ContainsKey(key))
                {
                    throw new System.Exception($"This language id {atributeId} attempts to use key {key} in row {i} which is already used previously");
                }

                if (attributeIndex > feilds.Length)
                {
                    throw new System.Exception($"This language id {atributeId} attempts to get a column {attributeIndex} which is not in row {i}");
                }

                var value = feilds[attributeIndex];


                if (value == "" || value == "\r")
                {
                    continue;
                }

                dict.Add(key, value);
            }

            return dict;



        }

        private string GetData()
        {
            var path = Path.Combine(Application.streamingAssetsPath, fileName);
            string data;

            if (Application.platform == RuntimePlatform.Android)
            {
                UnityWebRequest request = UnityWebRequest.Get(path);
                request.SendWebRequest();

                while (!request.isDone)
                {
                    if (request.isNetworkError || request.isHttpError)
                    {
                        Debug.LogError("Can't get file");
                        break;
                    }
                }

                if (request.isNetworkError || request.isHttpError)
                {
                    return "Error";
                }

                string cachePath = Path.Combine(Application.temporaryCachePath, "localisationData.locDat");
                File.WriteAllBytes(cachePath, request.downloadHandler.data);

                data = File.ReadAllText(cachePath, System.Text.Encoding.UTF8);
            }
            else
            {
                data= File.ReadAllText(path, System.Text.Encoding.UTF8);

            }

            return data;
        }
    }
}
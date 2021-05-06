using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Localisation
{
    internal class CSVLoader
    {
        
        public CSVLoader()
        {
            Load(); 
        }

        const string fileName = "localisationData.csv";
        char lineSeperator = '\n';
        char[] delimiter = { '#' };


        FileStream file;
        public void Load()
        {
            var path = Path.Combine(Application.streamingAssetsPath, fileName);
            file = File.OpenRead(path);
        }

        public Dictionary<string, string> GetDictionary(string atributeId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            Load();

            var data = file.ToString();

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

                dict.Add(key, value);
            }

            return dict;



        }
    }
}
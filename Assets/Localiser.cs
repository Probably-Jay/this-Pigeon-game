using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Localisation
{

    public enum Language 
    { 
        English
    }

    public enum TextID 
    { 
        
    }


    public class Localiser : MonoBehaviour
    {
        static Dictionary<Language, Dictionary<TextID, string>> text = null;

        static bool Inited => text != null;

        static Language currentLanguage  = Language.English;

        public static void SetLanguage(Language language)
        {
            currentLanguage = language;
        }

        public static void Init()
        {
            CSVLoader loader = new CSVLoader();
           

            text = new Dictionary<Language, Dictionary<TextID, string>>();

            foreach (var language in Helper.Utility.GetEnumValues<Language>())
            {
                LoadLanguage(loader, language);
            }

        }

        private static void LoadLanguage(CSVLoader loader, Language language)
        {
            Dictionary<string, string> rawDictionary = loader.GetDictionary(language.ToString());
            Dictionary<TextID, string> dictionary = new Dictionary<TextID, string>();
            foreach (var textID in Helper.Utility.GetEnumValues<TextID>())
            {
                if (!rawDictionary.ContainsKey(textID.ToString()))
                {
                    throw new System.Exception($"ID {textID} was not present in the data loaded for the language {language}");
                }
                dictionary.Add(textID, rawDictionary[textID.ToString()]);
            }

            text.Add(language, dictionary);
        }

        public static string GetString(TextID id)
        {
            if (!Inited)
            {
                Init(); // lazy intitialiseations
            }

            var value = text[currentLanguage][id];

            return value;
        }



    }






}
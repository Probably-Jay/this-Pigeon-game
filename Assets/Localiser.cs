using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Localisation
{

    public enum Language 
    { 
        English,
        Pirate_Speak
    }

    public enum TextID 
    { 
        MainMenu_Play,
        MainMenu_PlayAlt,
        MainMenu_Settings,
        MainMenu_Quit,

        Settings_ResetTutorials,
        Settings_Done,


    }


    public class Localiser : MonoBehaviour
    {

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

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

        public static string GetText(TextID id)
        {
            if (!Inited)
            {
                Init(); // lazy intitialiseations
            }

            Dictionary<TextID, string> languageDict = text[currentLanguage];
            string value = languageDict[id];

            return value;
        }



    }






}
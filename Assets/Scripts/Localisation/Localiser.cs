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

        //

        Settings_ResetTutorials,
        Settings_Done,

        //

        GameSelect_NewGameButton,
        GameSelect_BackButton,

        GameSelect_PlayerSlot_Fill_Error,
        GameSelect_PlayerSlot_Fill_CompanionTurn,
        GameSelect_PlayerSlot_Fill_Veiw,
        GameSelect_PlayerSlot_Fill_YourTurn,
        GameSelect_PlayerSlot_Fill_Play,

        GameSelect_Interface_Login_LoggingIn,
        GameSelect_Interface_LoginSucess,
        GameSelect_Interface_LoginFailure,
        GameSelect_Interface_GetGames_Gathering,
        GameSelect_Interface_GamesGatheredSucess_MemberOf,
        GameSelect_Interface_GamesGatheredButMemberOfNoGames,
        GameSelect_Interface_GamesGatheredGamesGatheredFailure,
        GameSelect_Interface_JoinNewGameCall_FindingGames,
        GameSelect_Interface_NewGameJoinedSucess,
        GameSelect_Interface_NewGameJoinedFailure_TooManyActiveGames,
        GameSelect_Interface_NewGameJoinedFailure_AboveOpenGamesLimit,
        GameSelect_Interface_NewGameJoinedFailure_ItIsTheOtherPlayersTurn,
        GameSelect_Interface_NewGameJoinedFailure_GameNotBegun,
        GameSelect_Interface_UnknownError,

        //

        MoodSelect_Welcome,
        MoodSelect_Info_0,
        MoodSelect_Info_1,
        MoodSelect_ContinueButton,
        MoodSelect_BackButton,

        MoodSelect_Petal_Lonley,
        MoodSelect_Petal_Stressed,
        MoodSelect_Petal_Excited,
        MoodSelect_Petal_Loving,

        //

        GameScene_GardenNamePannel_MyGarden,
        GameScene_GardenNamePannel_CompanionsGarden,
        GameScene_ToolboxButton,
        GameScene_SeedBag_GetASeed,
        GameScene_SeedBag_GiftASeed,
        GameScene_Toolbox_CloseToolbox,
        GameScene_Menu_MusicVolume,
        GameScene_Menu_AmbientVolume,
        GameScene_Menu_SFXVolume,
        GameScene_Menu_Quit,
        GameScene_Menu_Credits,
        GameScene_Menu_Back,
        GameScene_SeedBag_Close,
        GameScene_SeedBag_Plant,
        GameScene_EndTurn,


        Joyful,
        Painful,
        Energetic,
        Social,

        Settings_Language,
        Settings_ApplyLanguage,

        GameScene_PlayingText_1,
        GameScene_PlayingText_2,
        GameScene_SpectatingText_1,
        GameScene_SpectatingText_2,

        Settings_LiveUpdateOff,
        Settings_LiveUpdateOn,
        Settings_LiveUpdateWarning,

    }


    public class Localiser : MonoBehaviour
    {

        private void Awake()
        {
            if (PlayerPrefs.HasKey(LangagePrefsKey))
            {
                SetLanguage((Language)PlayerPrefs.GetInt(LangagePrefsKey));
            }
            DontDestroyOnLoad(gameObject);
        }

        public const string LangagePrefsKey = "Language";

        static Dictionary<Language, Dictionary<TextID, string>> text = null;

        static bool? InitedFlag = false;

        static bool Inited = InitedFlag ?? false && (text != null);

        public static Language CurrentLanguage { get; private set; } = (Language)(-1);

        public static void SetLanguage(Language language)
        {
            CurrentLanguage = language;
            PlayerPrefs.SetInt(LangagePrefsKey, (int)language);
        }

        public static void Init()
        {
            CSVLoader loader = new CSVLoader();
           

            text = new Dictionary<Language, Dictionary<TextID, string>>();
            try
            {
                foreach (var language in Helper.Utility.GetEnumValues<Language>())
                {
                    LoadLanguage(loader, language);
                }
                Inited = true;
            }
            catch (System.Exception e)
            {
                text = null;
                InitedFlag = null;
                throw e;
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



            if (!InitedFlag.HasValue)
            {
                return $"Error({id})"; // already had error
            }

            if (!Inited)
            {                
                Init(); // lazy intitialiseations
            }

            if (!InitedFlag.HasValue || !Inited || !text.ContainsKey(CurrentLanguage) )
            {
                return $"Error({id})";
            }

            Dictionary<TextID, string> languageDict = text[CurrentLanguage];
            string value = languageDict[id];

            return value;
        }



    }






}
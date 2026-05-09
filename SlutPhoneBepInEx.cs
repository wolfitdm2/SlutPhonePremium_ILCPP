using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Generator.MetadataAccess;
using Il2CppInterop.Generator.Runners;
using Il2CppInterop.Runtime;
using Il2CppSystem.Collections.Generic;
using System;
using BepInEx.Configuration;
using Input = UnityEngine.Input;
using KeyCode = UnityEngine.KeyCode;
using MethodInfo = System.Reflection.MethodInfo;

namespace SlutPhoneBepInEx_IL2CPP
{
    [BepInPlugin("com.wolfitdm.SlutPhoneBepInEx", "SlutPhoneBepInEx Plugin_IL2CPP", "1.0.0.0")]
    public class SlutPhoneBepInEx : BasePlugin
    {
        internal static new ManualLogSource Log;
        internal static new ManualLogSource Logger;

        private static KeyCode KeyCodeP = KeyCode.P;
        private static KeyCode KeyCodeG = KeyCode.G;
        private static KeyCode KeyCodeD = KeyCode.D;
        private static KeyCode KeyCodeS = KeyCode.S;
        private static KeyCode KeyCodeB = KeyCode.B;
        private static KeyCode KeyCodeN = KeyCode.N;
        private static KeyCode KeyCodeJ = KeyCode.J;
        private static KeyCode KeyCodeK = KeyCode.K;
        private static KeyCode KeyCodeC = KeyCode.C;
        private static KeyCode KeyCodeV = KeyCode.V;
        private static KeyCode KeyCodeU = KeyCode.U;
        private static KeyCode KeyCodeL = KeyCode.L;
        private static KeyCode KeyCodeM = KeyCode.M;

        private static bool useMaxValues = false;

        private static ConfigEntry<KeyCode> configKeyCodeP;
        private static ConfigEntry<KeyCode> configKeyCodeG;
        private static ConfigEntry<KeyCode> configKeyCodeD;
        private static ConfigEntry<KeyCode> configKeyCodeS;
        private static ConfigEntry<KeyCode> configKeyCodeB;
        private static ConfigEntry<KeyCode> configKeyCodeN;
        private static ConfigEntry<KeyCode> configKeyCodeJ;
        private static ConfigEntry<KeyCode> configKeyCodeK;
        private static ConfigEntry<KeyCode> configKeyCodeC;
        private static ConfigEntry<KeyCode> configKeyCodeV;
        private static ConfigEntry<KeyCode> configKeyCodeL;
        private static ConfigEntry<KeyCode> configKeyCodeM;
        private static ConfigEntry<KeyCode> configKeyCodeU;
        private static ConfigEntry<bool> configUseMaxValues;

        private static int minValue(int value, int min = 0, int max = 17)
        {
            if (value <= min) 
            {
                value = min;
            }
            
            if (value >= max)
            {
                value = max;
            }

            return value;
        }

        private void initConfig()
        {
            configKeyCodeP = Config.Bind("General",
                       "KeyCodeP",
                        KeyCode.P,
                       "KeyCode to press the premium button, default P");

            configKeyCodeG = Config.Bind("General",
                       "KeyCodeG",
                       KeyCode.G,
                       "KeyCode to unlock the full gallery, default G");

            configKeyCodeD = Config.Bind("General",
                       "KeyCodeD",
                        KeyCode.D,
                       "KeyCode to increase the relationship from dad, default D");

            configKeyCodeS = Config.Bind("General",
                       "KeyCodeS",
                       KeyCode.S,
                       "KeyCode to decrease the relationship from dad, default S");

            configKeyCodeB = Config.Bind("General",
                       "KeyCodeB",
                        KeyCode.B,
                       "KeyCode to increase the relationship from babe, default B");

            configKeyCodeN = Config.Bind("General",
                       "KeyCode",
                       KeyCode.N,
                       "KeyCode to decrease the relationship from babe, default N");

            configKeyCodeJ = Config.Bind("General",
                       "KeyCodeJ",
                        KeyCode.J,
                       "KeyCode to increase the relationship from john, default J");

            configKeyCodeK = Config.Bind("General",
                       "KeyCodeK",
                       KeyCode.K,
                       "KeyCode to decrease the relationship from john, default K");

            configKeyCodeC = Config.Bind("General",
                       "KeyCodeC",
                        KeyCode.C,
                       "KeyCode to increase corruption, default C");

            configKeyCodeV = Config.Bind("General",
                       "KeyCodeV",
                       KeyCode.V,
                       "KeyCode to decrease corruption, default V");

            configKeyCodeL = Config.Bind("General",
                       "KeyCodeL",
                       KeyCode.L,
                       "KeyCode to set corruption to 0, default L");

            configKeyCodeM = Config.Bind("General",
                       "KeyCodeM",
                       KeyCode.M,
                       "KeyCode to set corruption to 100, default M");

            configKeyCodeU = Config.Bind("General",
                       "KeyCodeU",
                       KeyCode.U,
                       "KeyCode to show vars, default U");

            configUseMaxValues = Config.Bind("General",
                       "UseMaxValues",
                       false,
                       "Use Max Values, default false");

            KeyCodeP = configKeyCodeP.Value;
            KeyCodeG = configKeyCodeG.Value;
            KeyCodeD = configKeyCodeD.Value;
            KeyCodeS = configKeyCodeS.Value;
            KeyCodeB = configKeyCodeB.Value;
            KeyCodeN = configKeyCodeN.Value;
            KeyCodeJ = configKeyCodeJ.Value;
            KeyCodeK = configKeyCodeK.Value;
            KeyCodeC = configKeyCodeC.Value;
            KeyCodeV = configKeyCodeV.Value;
            KeyCodeU = configKeyCodeU.Value;
            KeyCodeL = configKeyCodeL.Value;
            KeyCodeM = configKeyCodeM.Value;

            useMaxValues = configUseMaxValues.Value;
        }

        public SlutPhoneBepInEx()
        {
        }

        public static Type MyGetType(string originalClassName)
        {
            return Type.GetType(originalClassName + ",Assembly-CSharp");
        }

        public static IntPtr MyGetTypeIL2CPP(string originalClassName)
        {
            return IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", originalClassName);
        }


        public static Type oldMyGetType(string originalClassName)
        {
            Type originalClass = null;

            try
            {
                switch (originalClassName)
                {
                    /*case "ActionManager":
                        {
                            originalClass = typeof(ActionManager);
                        }
                        break;
                    */

                    default:
                        {
                            originalClass = MyGetType(originalClassName);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                originalClass = null;
            }


            if (originalClass == null)
            {
                Logger.LogInfo($"GetType {originalClassName} == null");
                return null;
            }

            return originalClass;
        }
		
        public static void PatchHarmonyMethod(Type originalClass, string originalMethodName, string patchedMethodName, bool usePrefix, bool usePostfix)
        {
            // Create a new Harmony instance with a unique ID
            var harmony = new Harmony("com.wolfitdm.SlutPhoneBepInEx");

            string originalClassName = "";

            if (originalClass == null)
            {
                Log.LogInfo($"GetType {originalClassName} == null");
                return;
            }

            // Or apply patches manually
            MethodInfo original = AccessTools.Method(originalClass, originalMethodName);

            if (original == null)
            {
                Log.LogInfo($"AccessTool.Method original {originalClassName} == null");
                return;
            }

            MethodInfo patched = AccessTools.Method(typeof(SlutPhoneBepInEx), patchedMethodName);

            if (patched == null)
            {
                Log.LogInfo($"AccessTool.Method patched {patchedMethodName} == null");
                return;

            }

            HarmonyMethod patchedMethod = new HarmonyMethod(patched);
            var prefixMethod = usePrefix ? patchedMethod : null;
            var postfixMethod = usePostfix ? patchedMethod : null;

            harmony.Patch(original,
                prefix: prefixMethod, 
                postfix: postfixMethod);
        }

        public static void patchHarmonyMethods()
        {
            try
            {
                PatchHarmonyMethod(typeof(SecondaryApps), "DisplayChoices", "SecondaryApps_DisplayChoices", true, false);
            }
            catch (Exception e)
            {
            }

            try
            {
                PatchHarmonyMethod(typeof(SecondaryApps), "DisplayChoices", "SecondaryApps_DisplayChoices_Postfix", false, true);
            }
            catch (Exception e)
            {
            }

            try
            {
                PatchHarmonyMethod(typeof(SecondaryApps), "Start", "SecondaryApps_Start", false, true);
            }
            catch (Exception e)
            {
            }

            try
            {
                PatchHarmonyMethod(typeof(ChatManager), "DisplayChoices", "ChatManager_DisplayChoices", true, false);
            }
            catch (Exception e)
            {
            }

            try
            {
                PatchHarmonyMethod(typeof(ChatManager), "DisplayChoices", "ChatManager_DisplayChoices_Postfix", false, true);
            }
            catch (Exception e)
            {
            }

            try
            {
                PatchHarmonyMethod(typeof(MainGame), "Update", "MainGame_Update", true, false);
            }
            catch (Exception e)
            {
            }

            try
            {
                PatchHarmonyMethod(typeof(ChatManager), "Start", "ChatManager_Start", false, true);
            }
            catch (Exception e)
            {
            }

            try
            {
                PatchHarmonyMethod(typeof(Gallery), "Initialize", "Gallery_Initialize", false, true);
            }
            catch (Exception e)
            {
            }

            try
            {
                PatchHarmonyMethod(typeof(ProgressionSystem), "Initialize", "ProgressionSystem_Initialize", false, true);
            }
            catch (Exception e)
            {
            }
        }

        private static Gallery galleryInstance = null;
        public static void Gallery_Initialize(Gallery __instance)
        {
            if (galleryInstance != null)
            {
                return;
            }
            galleryInstance = __instance;
            return;
        }

        private static ProgressionSystem progressionSystemInstance = null;
        public static void ProgressionSystem_Initialize(ProgressionSystem __instance)
        {
            if (progressionSystemInstance != null)
            {
                return;
            }
            progressionSystemInstance = __instance;
            return;
        }

        public static void setChoices(List<Choice> choices)
        {
            if (choices == null)
            {
                return;
            }

            if (choices.Count == 0)
            {
                return; 
            }

            foreach (Choice choice in choices)
            {
                if (choice == null)
                {
                    continue;
                }

                if (choice.isPremium && Input.GetKeyUp(KeyCodeU))
                {
                    Logger.LogInfo("choice: " + choice.choiceText);
                    Logger.LogInfo("choice: " + choice.outcomeText);

                    Logger.LogInfo($"{choice.outcomeMessages.ToString()}");
                }

                choice.isPremium = false;

                DialogueSegment next = choice.next;
                NoteSegment cnext = choice.cnext;

                while (next != null) {
                    if (next != null && next.choices != null && next.choices.Count != 0)
                    {
                        setChoices(next.choices);
                    }
                    next = next.next;
                }

                while (cnext != null)
                {
                    if (cnext != null && cnext.choices != null && cnext.choices.Count != 0)
                    {
                        setChoices(cnext.choices);
                    }
                    cnext = cnext.next;
                }

                if (choice.subChoices == null)
                {
                    continue;
                }

                if (choice.subChoices.Count == 0)
                {
                    continue; 
                }

                setChoices(choice.subChoices);
            }
        }

        private static SecondaryApps secondaryAppsInstance = null;
        public static void SecondaryApps_Start(SecondaryApps __instance)
        {
            if (!activateCheats)
            {
                return;
            }

            if (secondaryAppsInstance == null)
            {
                secondaryAppsInstance = __instance;
            }

            return;
        }
        public static bool SecondaryApps_DisplayChoices(List<Choice> choices, SecondaryApps __instance) {
            
            if (!activateCheats)
            {
                return true;
            }

            if (secondaryAppsInstance == null)
            {
                secondaryAppsInstance = __instance;
            }

            setChoices(choices);

            return true;
        }
        public static void SecondaryApps_DisplayChoices_Postfix(List<Choice> choices, SecondaryApps __instance)
        {
            if (!activateCheats)
            {
                return;
            }

            setChoices(choices);
        }


        private static ChatManager chatManagerInstance = null;
        private static VariableManager variableManager = null;
        public static void ChatManager_Start(object __instance)
        {
            ChatManager _this = (ChatManager)__instance;

            if (galleryInstance == null)
            {
                galleryInstance = _this.gallery;
            }

            if (chatManagerInstance == null)
            {
                chatManagerInstance = _this;
            }

            if (variableManager == null)
            {
                variableManager = _this.variableManager;
            }
        }
        public static bool ChatManager_DisplayChoices(List<Choice> choices, string chat, int mainID, object __instance)
        {
            if (!activateCheats)
            {
                return true;
            }

            setChoices(choices);

            ChatManager _this = (ChatManager)__instance;

            if (galleryInstance == null) {
                galleryInstance = _this.gallery;
            }

            if (chatManagerInstance == null)
            {
                chatManagerInstance = _this;
            }

            if (variableManager == null)
            {
                variableManager = _this.variableManager;
            }

            if (_this.variableManager != null && Input.GetKeyUp(KeyCodeU))
            {
                Logger.LogInfo("---bool vars---");
                foreach (string keyx in _this.variableManager.boolVariables.Keys)
                {
                    Logger.LogInfo(keyx);
                }

                Logger.LogInfo("---int vars---");
                foreach (string keyx in _this.variableManager.intVariables.Keys)
                {
                    Logger.LogInfo(keyx);
                }


                Logger.LogInfo("---float vars---");
                foreach (string keyx in _this.variableManager.floatVariables.Keys)
                {
                    Logger.LogInfo(keyx);
                }
            }

            return true;
        }
        public static void ChatManager_DisplayChoices_Postfix(List<Choice> choices, string chat, int mainID, object __instance)
        {
            if (!activateCheats)
            {
                return;
            }

            setChoices(choices);
        }

        private static bool activateCheats = false;
        public static bool MainGame_Update(object __instance)
        {
            MainGame _this = (MainGame)__instance;

            if (Input.GetKeyUp(KeyCode.Alpha0))
            {
                Logger.LogInfo("Cheats Activated!!!!");
                activateCheats = true;
            }

            if (!activateCheats)
            {
                return true;
            }

            if (Input.GetKeyUp(KeyCodeG))
            {
                if (galleryInstance != null)
                {
                    List<String> list = galleryInstance.unlocked;

                    for(int i = 0; i < list.Count; i++)
                    {
                        //Logger.LogInfo($"list: {list[i]}");
                    }

                    for (int i = 0; i < 300; i++)
                    {
                        try
                        {
                            string id = $"{i}";

                            if (!galleryInstance.exists(id))
                            {
                                continue;
                            }

                            if (galleryInstance.isUnlocked(id))
                            {
                                continue;
                            }
                            galleryInstance.unlock(id);
                        }
                        catch { }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCodeD))
            {
                if (variableManager != null)
                {
                    if (variableManager.intVariables.TryGetValue("Dad", out int dad))
                    {
                        dad++;
                        dad = minValue(dad);
                        variableManager.SetVariable("Dad", dad);
                        Logger.LogInfo($"Dad relationship increased: {dad}");
                        try
                        {
                            variableManager.intVariables["Dad"] = dad;

                            if (progressionSystemInstance != null)
                            {
                                progressionSystemInstance.changeStat("Dad", dad);
                                progressionSystemInstance.setVariable("Dad", dad);
                                progressionSystemInstance.changeStat("dad", dad);
                                progressionSystemInstance.setVariable("dad", dad);
                                progressionSystemInstance.changeStat("maxDad", dad);
                                progressionSystemInstance.setVariable("maxDad", dad);

                            }

                            if (secondaryAppsInstance != null)
                            {
                                secondaryAppsInstance.ChangeStat("Dad", dad);
                                secondaryAppsInstance.ChangeStat("dad", dad);
                                secondaryAppsInstance.ChangeStat("maxDad", dad);
                            }

                            if (useMaxValues)
                               MaxValues.maxDad = dad;
                        }
                        catch { }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCodeS))
            {
                if (variableManager != null)
                {
                    if (variableManager.intVariables.TryGetValue("Dad", out int dad))
                    {
                        dad--;
                        dad = minValue(dad);
                        variableManager.SetVariable("Dad", dad);
                        Logger.LogInfo($"Dad relationship decreased: {dad}");
                        try
                        {
                            variableManager.intVariables["Dad"] = dad;

                            if (progressionSystemInstance != null)
                            {
                                progressionSystemInstance.changeStat("Dad", dad);
                                progressionSystemInstance.setVariable("Dad", dad);
                                progressionSystemInstance.changeStat("dad", dad);
                                progressionSystemInstance.setVariable("dad", dad);
                                progressionSystemInstance.changeStat("maxDad", dad);
                                progressionSystemInstance.setVariable("maxDad", dad);

                            }

                            if (secondaryAppsInstance != null)
                            {
                                secondaryAppsInstance.ChangeStat("Dad", dad);
                                secondaryAppsInstance.ChangeStat("dad", dad);
                                secondaryAppsInstance.ChangeStat("maxDad", dad);
                            }

                            if (useMaxValues)
                               MaxValues.maxDad = dad;
                        }
                        catch { }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCodeB))
            {
                if (variableManager != null)
                {
                    if (variableManager.intVariables.TryGetValue("Babe", out int babe))
                    {
                        babe++;
                        babe = minValue(babe);
                        variableManager.SetVariable("Babe", babe);
                        Logger.LogInfo($"Babe relationship increased: {babe}");
                        try
                        {
                            variableManager.intVariables["Babe"] = babe;

                            if (progressionSystemInstance != null)
                            {
                                progressionSystemInstance.changeStat("Babe", babe);
                                progressionSystemInstance.setVariable("Babe", babe);
                                progressionSystemInstance.changeStat("babe", babe);
                                progressionSystemInstance.setVariable("babe", babe);
                                progressionSystemInstance.changeStat("maxBabe", babe);
                                progressionSystemInstance.setVariable("maxBabe", babe);

                            }

                            if (secondaryAppsInstance != null)
                            {
                                secondaryAppsInstance.ChangeStat("Babe", babe);
                                secondaryAppsInstance.ChangeStat("babe", babe);
                                secondaryAppsInstance.ChangeStat("maxBabe", babe);
                            }

                            if (useMaxValues)
                               MaxValues.maxBabe = babe;
                        }
                        catch { }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCodeN))
            {
                if (variableManager != null)
                {
                    if (variableManager.intVariables.TryGetValue("Babe", out int babe))
                    {
                        babe--;
                        babe = minValue(babe);
                        variableManager.SetVariable("Babe", babe);
                        Logger.LogInfo($"Babe relationship decreased: {babe}");
                        try
                        {
                            variableManager.intVariables["Babe"] = babe;

                            if (progressionSystemInstance != null)
                            {
                                progressionSystemInstance.changeStat("Babe", babe);
                                progressionSystemInstance.setVariable("Babe", babe);
                                progressionSystemInstance.changeStat("babe", babe);
                                progressionSystemInstance.setVariable("babe", babe);
                                progressionSystemInstance.changeStat("maxBabe", babe);
                                progressionSystemInstance.setVariable("maxBabe", babe);

                            }

                            if (secondaryAppsInstance != null)
                            {
                                secondaryAppsInstance.ChangeStat("Babe", babe);
                                secondaryAppsInstance.ChangeStat("babe", babe);
                                secondaryAppsInstance.ChangeStat("maxBabe", babe);
                            }

                            if (useMaxValues)
                               MaxValues.maxBabe = babe;
                        }
                        catch { }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCodeJ))
            {
                if (variableManager != null)
                {
                    if (variableManager.intVariables.TryGetValue("John", out int john))
                    {
                        john++;
                        john = minValue(john);
                        variableManager.SetVariable("John", john);
                        Logger.LogInfo($"John relationship increased: {john}");
                        try
                        {
                            variableManager.intVariables["John"] = john;

                            if (progressionSystemInstance != null)
                            {
                                progressionSystemInstance.changeStat("John", john);
                                progressionSystemInstance.setVariable("John", john);
                                progressionSystemInstance.changeStat("john", john);
                                progressionSystemInstance.setVariable("john", john);
                                progressionSystemInstance.changeStat("maxJohn", john);
                                progressionSystemInstance.setVariable("maxJohn", john);

                            }

                            if (secondaryAppsInstance != null)
                            {
                                secondaryAppsInstance.ChangeStat("John", john);
                                secondaryAppsInstance.ChangeStat("john", john);
                                secondaryAppsInstance.ChangeStat("maxJohn", john);
                            }

                            if (useMaxValues)
                               MaxValues.maxJohn = john;
                        }
                        catch { }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCodeK))
            {
                if (variableManager != null)
                {
                    if (variableManager.intVariables.TryGetValue("John", out int john))
                    {
                        john--;
                        john = minValue(john);
                        variableManager.SetVariable("John", john);
                        Logger.LogInfo($"John relationship decreased: {john}");
                        try
                        {
                            variableManager.intVariables["John"] = john;
                            
                            if (progressionSystemInstance != null)
                            {
                                progressionSystemInstance.changeStat("John", john);
                                progressionSystemInstance.setVariable("John", john);
                                progressionSystemInstance.changeStat("john", john);
                                progressionSystemInstance.setVariable("john", john);
                                progressionSystemInstance.changeStat("maxJohn", john);
                                progressionSystemInstance.setVariable("maxJohn", john);

                            }

                            if (secondaryAppsInstance != null)
                            {
                                secondaryAppsInstance.ChangeStat("John", john);
                                secondaryAppsInstance.ChangeStat("john", john);
                                secondaryAppsInstance.ChangeStat("maxJohn", john);
                            }

                            if (useMaxValues)
                               MaxValues.maxJohn = john;
                        }
                        catch { }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCodeC))
            {
                if (variableManager != null)
                {
                    if (variableManager.intVariables.TryGetValue("Corruption", out int corr))
                    {
                        corr++;
                        corr = minValue(corr, 0, 12);
                        variableManager.SetVariable("Corruption", corr);
                        Logger.LogInfo($"Corruption increased: {corr}");
                        try
                        {
                            variableManager.intVariables["Corruption"] = corr;

                            if (progressionSystemInstance != null)
                            {
                                progressionSystemInstance.changeCorruption(corr);
                                progressionSystemInstance.changeStat("Corruption", corr);
                            }

                            if (secondaryAppsInstance != null)
                            {
                                secondaryAppsInstance.ChangeCorruption(corr);
                                secondaryAppsInstance.ChangeStat("Corruption", corr);
                            }

                            if (useMaxValues)
                               MaxValues.maxCorruption = corr;
                        }
                        catch { }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCodeV))
            {
                if (variableManager != null)
                {
                    if (variableManager.intVariables.TryGetValue("Corruption", out int corr))
                    {
                        corr--;
                        corr = minValue(corr, 0, 12);
                        variableManager.SetVariable("Corruption", corr);
                        Logger.LogInfo($"Corruption decreased: {corr}");
                        try
                        {
                            variableManager.intVariables["Corruption"] = corr;

                            if (progressionSystemInstance != null)
                            {
                                progressionSystemInstance.changeCorruption(corr);
                                progressionSystemInstance.setVariable("Corruption", corr);
                                progressionSystemInstance.changeStat("Corruption", corr);
                            }

                            if (secondaryAppsInstance != null)
                            {
                                secondaryAppsInstance.ChangeCorruption(corr);
                                secondaryAppsInstance.ChangeStat("Corruption", corr);
                            }

                            if (useMaxValues)
                               MaxValues.maxCorruption = corr;
                        }
                        catch { }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCodeL))
            {
                if (variableManager != null)
                {
                    if (variableManager.intVariables.TryGetValue("Corruption", out int corr))
                    {
                        corr = 0;
                        variableManager.SetVariable("Corruption", corr);
                        Logger.LogInfo($"Corruption decreased: {corr}");
                        try
                        {
                            variableManager.intVariables["Corruption"] = corr;

                            if (progressionSystemInstance != null)
                            {
                                progressionSystemInstance.changeCorruption(corr);
                                progressionSystemInstance.setVariable("Corruption", corr);
                                progressionSystemInstance.changeStat("Corruption", corr);
                            }

                            if (secondaryAppsInstance != null)
                            {
                                secondaryAppsInstance.ChangeCorruption(corr);
                                secondaryAppsInstance.ChangeStat("Corruption", corr);
                            }

                            if (useMaxValues)
                                MaxValues.maxCorruption = corr;
                        }
                        catch { }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCodeM))
            {
                if (variableManager != null)
                {
                    if (variableManager.intVariables.TryGetValue("Corruption", out int corr))
                    {
                        corr = 17;
                        variableManager.SetVariable("Corruption", corr);
                        Logger.LogInfo($"Corruption decreased: {corr}");
                        try
                        {
                            variableManager.intVariables["Corruption"] = corr;

                            if (progressionSystemInstance != null)
                            {
                                progressionSystemInstance.changeCorruption(corr);
                                progressionSystemInstance.setVariable("Corruption", corr);
                                progressionSystemInstance.changeStat("Corruption", corr);
                            }

                            if (secondaryAppsInstance != null)
                            {
                                secondaryAppsInstance.ChangeCorruption(corr);
                                secondaryAppsInstance.ChangeStat("Corruption", corr);
                            }

                            if (useMaxValues)
                                MaxValues.maxCorruption = corr;
                        }
                        catch { }
                    }
                }
            }
            
            return true;
        }
        public override void Load()
        {
            Logger = base.Log;
            Log = base.Log;
            initConfig();
            patchHarmonyMethods();
        }     
    }
}
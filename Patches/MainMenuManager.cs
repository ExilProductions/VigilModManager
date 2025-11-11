using System;
using HarmonyLib;
using FMODUnity;

namespace VMM.Patches
{
    internal static class MainMenuManager
    {
        private static readonly System.Reflection.FieldInfo field = AccessTools.Field(typeof(global::MainMenuManager), "buttonPress");

        public static EventReference GetButtonPress(global::MainMenuManager instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (field == null)
            {
                Core.Logger.Error("Could not find field 'buttonPress' in MainMenuManager!");
                return default;
            }

            return (EventReference)field.GetValue(instance);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using LuaScriptingEngine;

namespace LuaLoader
{
    public class LuaLoader
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class __pregeneratedbuildings
        {
            public static void Prefix()
            {
                // Load script from [modpath]\[dllname]\init.lua
                string scriptpath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(".dll", "");
                new ScriptingCore("init", "loaded", scriptpath);
            }
        }
    }
}

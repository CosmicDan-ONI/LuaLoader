using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using LuaScriptingEngine;

namespace ModpackTemplate
{
    public class ModpackTemplate
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class __pregeneratedbuildings
        {
            public static void Prefix()
            {
                // PHASE 1: Load script from [modpath]\scripts\init.lua

                string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string modDir = Path.GetDirectoryName(@dllPath);
                string modName = dllPath.Replace(modDir, "").Replace(".dll", "").Remove(0, 1); // also need to trim the first \ off
                string luaScriptPath = modDir + "\\scripts";
                Debug.Log("[" + modName + "] [#] Attempting to load " + luaScriptPath + "\\init.lua ...");

                // HACK: modify the scriptpath in Lua Scripting Engine before running, since it's hardcoded to look in a fully-qualified path
                // get Lua Scripting Engine field first via reflection
                Type type = typeof(ScriptingCore);
                FieldInfo field = type.GetField("scriptpath", BindingFlags.NonPublic | BindingFlags.Static);
                // backup original scriptpath
                string scriptpathOrg = field.GetValue(null) as string;
                // set new scriptpath for out mod
                field.SetValue(null, luaScriptPath);
                // call modpack init script
                new ScriptingCore("init", "loaded");
                // restore original scriptpath to Lua Scripting Engine
                field.SetValue(null, scriptpathOrg);
            }
        }
    }
}

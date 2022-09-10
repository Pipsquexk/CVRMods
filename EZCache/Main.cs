using System;
using System.Collections;

using MelonLoader;
using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Savior;


namespace EZCache
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            LoggerInstance.Msg(ConsoleColor.Blue, "EZCache is starting.....");
            MelonCoroutines.Start(WaitForMainMenuView());
            Utilities.LoadConfig();
            Patches.ApplyPatches();
            LoggerInstance.Msg(ConsoleColor.Green, "EZCache has started!");
        }




        /// <summary>
        /// Called to wait for the menu view to initialize and setup EZCache ui options.
        /// </summary>
        /// <returns>Nothing</returns>
        private IEnumerator WaitForMainMenuView()
        {
            while (ViewManager.Instance == null)
                yield return null;
            while (ViewManager.Instance.gameMenuView == null)
                yield return null;
            while (ViewManager.Instance.gameMenuView.Listener == null)
                yield return null;

            ViewManager.Instance.gameMenuView.Listener.ReadyForBindings += () =>
            {
                ViewManager.Instance.gameMenuView.View.RegisterForEvent("EZCache_ClearALLCache", new Action(Utilities.ClearAllCache));
                ViewManager.Instance.gameMenuView.View.RegisterForEvent("EZCache_ClearAvatarCache", new Action(() => Utilities.ClearAvatarCache(true)));
                ViewManager.Instance.gameMenuView.View.RegisterForEvent("EZCache_ClearPropCache", new Action(() => Utilities.ClearPropCache(true)));
                ViewManager.Instance.gameMenuView.View.RegisterForEvent("EZCache_ClearWorldCache", new Action(() => Utilities.ClearWorldCache(true)));
                ViewManager.Instance.gameMenuView.View.RegisterForEvent("EZCache_RestartGame", new Action(Utilities.RestartGame));
                ViewManager.Instance.gameMenuView.View.RegisterForEvent("EZCache_RefreshCacheValues", new Action(Utilities.RefreshCacheValues));
                
                ViewManager.Instance.gameMenuView.View.BindCall("EZCache_AddValue", new Action<string, int>(Utilities.AddValue));
            };
            
            ViewManager.Instance.gameMenuView.Listener.FinishLoad += (_) =>
            {
                ViewManager.Instance.quickMenuView.View.ExecuteScript(@"﻿{
            var l_block = document.createElement('div');

            function addValue(_str, _int) {
                engine.call('EZCache_AddValue', _str, _int);
            }


            l_block.innerHTML = `

                                    

            <div class=""settings-subcategory"">
                <div class=""subcategory-name"">EZCache</div>
                <div class=""subcategory-description"">Sex</div>
            </div>

            <div class=""row-wrapper"">
                <div class=""inp_button"" onclick=""addValue('aviCache', -1);"">-1</div>
                <div id=""EZCache_aviCacheSizeDisplay"" class=""option-caption"">Avatar Cache Max Size: " + Convert.ToInt32(Utilities.settings["aviCache"].Value) + @"GB</div>
                <div class=""inp_button"" onclick=""addValue('aviCache', 1);"">+1</div>
            </div>

            <div class=""row-wrapper"">
                <div class=""inp_button"" onclick=""addValue('propCache', -1);"">-1</div>
                <div id=""EZCache_propCacheSizeDisplay"" class=""option-caption"">Prop Cache Max Size: " + Convert.ToInt32(Utilities.settings["propCache"].Value) + @"GB</div>
                <div class=""inp_button"" onclick=""addValue('propCache', 1);"">+1</div>
            </div>

            <div class=""row-wrapper"">
                <div class=""inp_button"" onclick=""addValue('wrldCache', -1);"">-1</div>
                <div id=""EZCache_wrldCacheSizeDisplay"" class=""option-caption"">World Cache Max Size: " + Convert.ToInt32(Utilities.settings["wrldCache"].Value) + @"GB</div>
                <div class=""inp_button"" onclick=""addValue('wrldCache', 1);"">+1</div>
            </div>

            <div class=""row-wrapper"">
                <div id=""EZCache_aviCacheDisplay"" class=""option-caption"">Current avatar cache size: 0GB</div>
            </div>

            <div class=""row-wrapper"">
                <div id=""EZCache_propCacheDisplay"" class=""option-caption"">Current prop cache size: 0GB</div>
            </div>

            <div class=""row-wrapper"">
                <div id=""EZCache_wrldCacheDisplay"" class=""option-caption"">Current world cache size: 0GB</div>
            </div>

            <div class=""row-wrapper"">
                <div class=""content-btn button"" onclick=""engine.trigger('EZCache_RefreshCacheValues');"">Refresh Cache values</div>
            </div>

            <div class=""row-wrapper"">
                <div class=""content-btn button"" onclick=""engine.trigger('EZCache_ClearALLCache');"">Clear ALL Cache</div>
            </div>
            <div class=""row-wrapper"">
                <div class=""content-btn button"" onclick=""engine.trigger('EZCache_ClearAvatarCache');"">Clear Avatar Cache</div>
            </div>
            <div class=""row-wrapper"">
                <div class=""content-btn button"" onclick=""engine.trigger('EZCache_ClearPropCache');"">Clear Prop Cache</div>
            </div>
            <div class=""row-wrapper"">
                <div class=""content-btn button"" onclick=""engine.trigger('EZCache_ClearWorldCache');"">Clear World Cache</div>
            </div>
            <div class=""row-wrapper"">
                <div class=""content-btn button"" onclick=""engine.trigger('EZCache_RestartGame');"">Restart Game (Not Reliable)</div>
            </div>
            <div class=""row-wrapper""></div>
            `;
            document.getElementById('settings-implementation').appendChild(l_block);
    }");
            };

            Utilities.RefreshCacheValues();
        }
    }
}
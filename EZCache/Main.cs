using System;
using System.Collections;

using MelonLoader;
using ABI_RC.Core.InteractionSystem;


namespace EZCache
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            LoggerInstance.Msg(ConsoleColor.Blue, "EZCache is starting.....");
            MelonCoroutines.Start(WaitForMainMenuView());
            LoggerInstance.Msg(ConsoleColor.Green, "EZCache has started!");
        }

        /// <summary>
        /// Called to wait for the menu view to initialize and setup EZCache ui options.
        /// </summary>
        /// <returns>Nothing</returns>
        public IEnumerator WaitForMainMenuView()
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
            };

            ViewManager.Instance.gameMenuView.Listener.FinishLoad += (_) =>
            {
                ViewManager.Instance.quickMenuView.View.ExecuteScript(@"﻿{
            var l_block = document.createElement('div');



            l_block.innerHTML = `

            <div class=""settings-subcategory"">
                <div class=""subcategory-name"">EZCache</div>
                <div class=""subcategory-description""></div>
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
        }
    }
}
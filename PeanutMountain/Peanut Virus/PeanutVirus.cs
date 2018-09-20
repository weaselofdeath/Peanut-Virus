using Smod.PVirus;
using Smod2;
using Smod2.Attributes;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Collections.Generic;
using Smod2.Lang;

namespace SetNickname
{
    [PluginDetails(
        author = "WeaselOfDeath",
        name = "Peanut Virus",
        description = "Peanut Virus - 1 SCP all D (Using PlagueCurse as template)",
        id = "rex.gamemode.pvirus",
        version = "1.1",
        SmodMajor = 3,
        SmodMinor = 1,
        SmodRevision = 17
        )]
    class GiveItem : Plugin
    {
        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
            this.Info("Peanut Virus has loaded :)");
        }

        public override void Register()
        {
            // Register Events
            this.AddEventHandlers(new RoundStartHandler(this), Priority.Highest);
            this.AddEventHandlers(new OnSpawnHandler(this), Priority.Highest);
            this.AddEventHandlers(new OnRoundRestartHandler(this), Priority.Highest);
            this.AddEventHandlers(new OnRoundStartHandler(this), Priority.Highest);
            this.AddEventHandlers(new ClassDEscapeHandler(this), Priority.Highest);
            this.AddEventHandlers(new PlayerHurtHandler(this), Priority.Highest);
			GamemodeManager.GamemodeManager.RegisterMode(this, "01111111111111111111");

            Dictionary<string, string> translations = new Dictionary<string, string>
            {
                { "PVI_YOU_ARE", "You are" },
                { "PVI_GOAL", "Goal" },

                { "PVI_SCP_173", "SCP-173" },
                { "PVI_SCP_173_GOAL", "Kill all Class-Ds to convert them to SCP-173. If even one escapes, you lose." },

                { "PVI_CLASSD", "Civilian" },
                { "PVI_CLASSD_GOAL", "Escape the Facility. If you die by SCP-173, you become SCP-173." }
            };

            foreach (KeyValuePair<string, string> translation in translations)
            {
                this.AddTranslation(new LangSetting(translation.Key, translation.Value, "gamemode_pvirus"));
            }
        }
    }
}
using Smod2;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System;
using Smod2.EventSystem.Events;

namespace Smod.PVirus
{
    class RoundStartHandler : IEventHandlerSetSCPConfig, IEventHandlerTeamRespawn, IEventHandlerInitialAssignTeam, IEventHandlerSetRole, IEventHandlerSetRoleMaxHP
    {
        private Plugin plugin;

        public RoundStartHandler(Plugin plugin)
        {
            this.plugin = plugin;
        }

		public void OnSetRoleMaxHP(SetRoleMaxHPEvent ev)
		{
			if (GamemodeManager.GamemodeManager.GetCurrentMode().Equals(plugin))
			{
				if (ev.Role == Role.SCP_173)
				{
                    ev.MaxHP = 300000;
				}
			}
		}

        public void OnSetSCPConfig(SetSCPConfigEvent ev)
        {
            if (GamemodeManager.GamemodeManager.GetCurrentMode().Equals(plugin))
            {
				ev.Ban049 = true;
				ev.Ban079 = true;
				ev.Ban096 = true;
				ev.Ban106 = true;
				ev.Ban173 = false;
				ev.Ban939_53 = true;
				ev.Ban939_89 = true;
            }
        }

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (GamemodeManager.GamemodeManager.GetCurrentMode().Equals(plugin))
			{
                if (ev.Player.TeamRole.Role == Role.CLASSD)
                {
                    ev.Player.GetInventory().Clear();
                    ev.Items.Add(ItemType.JANITOR_KEYCARD);
                    ev.Items.Add(ItemType.COM15);
                    ev.Player.SetAmmo(AmmoType.DROPPED_5, 1000);
                    ev.Player.SetAmmo(AmmoType.DROPPED_7, 1000);
                    ev.Player.SetAmmo(AmmoType.DROPPED_9, 1000);
                }
			}
		}

        public void OnTeamRespawn(TeamRespawnEvent ev)
        {
            if (GamemodeManager.GamemodeManager.GetCurrentMode().Equals(plugin))
            {
				ev.SpawnChaos = false;
            }
        }

		public void OnAssignTeam(PlayerInitialAssignTeamEvent ev)
		{
			if (GamemodeManager.GamemodeManager.GetCurrentMode().Equals(plugin))
			{
                if ((ev.Team != Team.CLASSD) && (ev.Team != Team.SCP))
                {
                    if ((ev.Team == Team.CHAOS_INSURGENCY) || (ev.Team == Team.NINETAILFOX))
                    {
                        ev.Team = Team.SCP;
                        ev.Player.Teleport(plugin.pluginManager.Server.Map.GetRandomSpawnPoint(Role.SCP_173));
                        ev.Player.AddHealth(300000);
                    }
                    if ((ev.Team != Team.NINETAILFOX) || (ev.Team != Team.CHAOS_INSURGENCY))
                    {
                        ev.Team = Team.CLASSD;
                        ev.Player.Teleport(plugin.pluginManager.Server.Map.GetRandomSpawnPoint(Role.CLASSD));
                    }
                }
			}
		}
    }

    class OnRoundRestartHandler : IEventHandlerRoundRestart
    {
        private Plugin plugin;
        public OnRoundRestartHandler(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnRoundRestart(RoundRestartEvent ev)
        {
        }
    }

    class OnSpawnHandler : IEventHandlerSpawn
    {
        private Plugin plugin;
        public OnSpawnHandler(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnSpawn(PlayerSpawnEvent ev)
        {
            if (GamemodeManager.GamemodeManager.GetCurrentMode().Equals(plugin))
            {
                if (ev.Player.TeamRole.Role != Role.CLASSD && ev.Player.TeamRole.Role != Role.SCP_173)
                {
                    if (ev.Player.TeamRole.Role == Role.FACILITY_GUARD || ev.Player.TeamRole.Role == Role.SCIENTIST)
                    {
                        ev.Player.ChangeRole(Role.CLASSD);
                        ev.Player.Teleport(plugin.pluginManager.Server.Map.GetRandomSpawnPoint(Role.CLASSD));
                    }
                    else
                    {
                        ev.Player.ChangeRole(Role.SCP_173);
                        ev.Player.Teleport(plugin.pluginManager.Server.Map.GetRandomSpawnPoint(Role.SCP_173));
                        ev.Player.AddHealth(300000);
                    }
                }
            }
        }
    }

    class OnRoundStartHandler : IEventHandlerRoundStart
    {
        private Plugin plugin;
        public OnRoundStartHandler(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            if (GamemodeManager.GamemodeManager.GetCurrentMode().Equals(plugin))
            {
                ev.Server.Map.GetItems(ItemType.MAJOR_SCIENTIST_KEYCARD, false).ForEach(i => { ev.Server.Map.SpawnItem(ItemType.MTF_LIEUTENANT_KEYCARD, i.GetPosition(), i.GetPosition()); i.Remove(); });
                ev.Server.Map.GetItems(ItemType.SCIENTIST_KEYCARD, false).ForEach(i => { ev.Server.Map.SpawnItem(ItemType.MTF_LIEUTENANT_KEYCARD, i.GetPosition(), i.GetPosition()); i.Remove(); });
                ev.Server.Map.GetItems(ItemType.ZONE_MANAGER_KEYCARD, false).ForEach(i => { ev.Server.Map.SpawnItem(ItemType.MTF_LIEUTENANT_KEYCARD, i.GetPosition(), i.GetPosition()); i.Remove(); });
            }

            foreach (Player player in ev.Server.GetPlayers())
            {
                if (player.TeamRole.Role == Role.SCP_173)
                {
                    player.SendConsoleMessage(Environment.NewLine + plugin.GetTranslation("PVI_YOU_ARE") + ": " + plugin.GetTranslation("PVI_SCP_173") + Environment.NewLine + plugin.GetTranslation("PVI_GOAL") + ": " + plugin.GetTranslation("PVI_SCP_173_GOAL"), "red");
                }
                else if (player.TeamRole.Role == Role.CLASSD)
                {
                    player.SendConsoleMessage(Environment.NewLine + plugin.GetTranslation("PVI_YOU_ARE") + ": " + plugin.GetTranslation("PVI_CLASSD") + Environment.NewLine + plugin.GetTranslation("PVI_GOAL") + ": " + plugin.GetTranslation("PVI_CLASSD_GOAL")+Environment.NewLine+"914 might mutate you...", "red");
                }
            }
        }
    }

    class ClassDEscapeHandler : IEventHandlerCheckEscape
    {
        private Plugin plugin;

        public ClassDEscapeHandler(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnCheckEscape(PlayerCheckEscapeEvent ev)
        {
            if (GamemodeManager.GamemodeManager.GetCurrentMode().Equals(plugin))
            {
                if (ev.Player != null && ev.Player.TeamRole.Role == Role.CLASSD)
                {
                    foreach (Player player in plugin.pluginManager.Server.GetPlayers())
                    {
                        if ((player.TeamRole.Team == Team.SCP) || (player.TeamRole.Team == Team.CHAOS_INSURGENCY) || (player.TeamRole.Team == Team.NINETAILFOX))
                        {
                            player.Kill();
                        }
                    }
                    plugin.pluginManager.Server.Round.EndRound();
                }
            }
        }
    }


    class PlayerHurtHandler : IEventHandlerPlayerHurt
    {
        private Plugin plugin;

        public PlayerHurtHandler(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            if (GamemodeManager.GamemodeManager.GetCurrentMode().Equals(plugin) && !plugin.pluginManager.Server.Map.WarheadDetonated)
            {
                if (ev.Attacker != null && ev.Player != null && ev.Attacker.TeamRole.Role == Role.CLASSD && ev.Player.TeamRole.Role == Role.CLASSD && ev.Attacker != ev.Player)
                {
                    ev.Attacker.Damage(Convert.ToInt32(Math.Floor(ev.Damage)), ev.DamageType);
                }
                if (ev.Attacker != null && ev.Player != null && ev.Attacker.TeamRole.Role == Role.SCP_173 && ev.Attacker != ev.Player)
                {
                    ev.Damage = 0.0f;
                    ev.Player.ChangeRole(Role.SCP_173);
                    ev.Player.AddHealth(300000);
                }
            }
        }
    }
}
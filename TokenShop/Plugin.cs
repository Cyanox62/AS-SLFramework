using Exiled.API.Features;
using Exiled.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TokenShop.Commands;
using TokenShop.Perks;

namespace TokenShop
{
    public class Plugin : Plugin<Config, Translation>
    {
        internal static Plugin singleton;

        internal static string FolderFilePath = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "TokenShop");

        private EventHandlers ev;

        public override void OnEnabled()
        {
            base.OnEnabled();

            if (!Directory.Exists(FolderFilePath)) Directory.CreateDirectory(FolderFilePath);

            singleton = this;

            Shop.ShopString.Append("\nASTRIOS STUDIOS SCP:SL SERVER SHOP\n");
            Shop.ShopString.Append("To purchase an item, type .shop buy (item #)");

            // Parse shop
            var perks = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.Namespace == "TokenShop.Perks");
            for (int i = 0; i < Config.ShopItems.Count; i++)
            {
                var entry = Config.ShopItems.ElementAt(i);
                if (entry.Count == 4)
                {
                    if (entry[2] == "P" || entry[2] == "N")
                    {
                        bool isPermanent = entry[2] == "P";
                        if (int.TryParse(entry[3], out int tokens))
                        {
                            if (int.TryParse(entry[0], out int id))
                            {
                                Type customPerk = perks.FirstOrDefault(x => x.Name == entry[1]);
                                if (Enum.TryParse(entry[1], out ItemType type))
                                {
                                    Perk perk = new ParamaterizedItem(type, isPermanent);
                                    AddShopItem(id - 1, $"{(isPermanent ? "Permanent " : string.Empty)}{entry[1]}", isPermanent, perk, tokens);
                                }
                                else if (customPerk != null)
                                {
                                    Perk perk = (Perk)Activator.CreateInstance(customPerk);
                                    AddShopItem(id - 1, entry[1], isPermanent, perk, tokens);
                                }
                                else
                                {
                                    Log.Error($"Failed to parse perk \"{entry[1]}\", shop item will not be loaded.");
                                }
                            }
                            else
                            {
                                Log.Error($"Failed to parse id \"{entry[0]}\", shop item will not be loaded.");
                            }
                        }
                        else
                        {
                            Log.Error($"Failed to parse token value \"{entry[3]}\", shop item will not be loaded.");
                        }
                    }
                    else
                    {
                        Log.Error($"Failed to parse persistence \"{entry[2]}\", shop item will not be loaded.");
                    }
                }
                else
                {
                    Log.Error($"Shop item \"{entry}\" is missing arguments, shop item will not be loaded.");
                }
            }

            ev = new EventHandlers();

            Exiled.Events.Handlers.Player.Verified += ev.OnPlayerVerified;
            Exiled.Events.Handlers.Player.PickingUpItem += ev.OnPickingUpItem;
            Exiled.Events.Handlers.Player.ChangingRole += ev.OnSetRole;

            Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += ev.OnRoundEnd;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            Exiled.Events.Handlers.Player.Verified -= ev.OnPlayerVerified;
            Exiled.Events.Handlers.Player.PickingUpItem -= ev.OnPickingUpItem;
            Exiled.Events.Handlers.Player.ChangingRole -= ev.OnSetRole;

            Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= ev.OnRoundEnd;

            ev = null;
        }

        private void AddShopItem(int id, string perkName, bool isPermanent, Perk perk, int tokens)
        {
            Shop.ShopString.Append(Environment.NewLine);
            Shop.ShopItems.Add(new ShopItem()
            {
                id = id,
                perk = perk,
                price = tokens
            });
            Shop.ShopString.Append($"#{id + 1} | {perkName} | {tokens} tokens");
            EventHandlers.Log($"Loaded shop item: {perkName}");
        }

        internal static void AccessHintSystem(Player p, string hint, float time)
        {
            Loader.Plugins.FirstOrDefault(pl => pl.Name == "TipSystem")?.Assembly?.GetType("TipSystem.API.System")?.GetMethod("ShowHint", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, new object[] { p, hint, time });
        }

        public override string Author => "Cyanox";
        public override string Name => "TokenShop";
    }
}

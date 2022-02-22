using Exiled.API.Features;
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

            // Parse shop
            var perks = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.Namespace == "TokenShop.Perks");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Config.ShopItems.Count; i++)
            {
                var entry = Config.ShopItems.ElementAt(i);
                if (entry.Count == 3)
				{
                    if (entry[1] == "P" || entry[1] == "N")
                    {
                        bool isPermanent = entry[1] == "P";
                        if (int.TryParse(entry[2], out int tokens)) 
                        {
                            Type customPerk = perks.FirstOrDefault(x => x.Name == entry[0]);
                            if (Enum.TryParse(entry[0], out ItemType type))
                            {
                                Perk perk = new ParamaterizedItem(type, isPermanent);
                                AddShopItem(i, $"{(isPermanent ? "Permanent" : string.Empty)} {entry[0]}", isPermanent, perk, tokens);
                            }
                            else if (customPerk != null)
                            {
                                Perk perk = (Perk)Activator.CreateInstance(customPerk);
                                AddShopItem(i, entry[0], isPermanent, perk, tokens);
                            }
                            else
							{
                                Log.Error($"Failed to parse perk \"{entry[0]}\", shop item will not be loaded.");
                            }
						}
                        else
                        {
                            Log.Error($"Failed to parse token value \"{entry[2]}\", shop item will not be loaded.");
                        }
                    }
                    else
                    {
                        Log.Error($"Failed to parse persistence \"{entry[1]}\", shop item will not be loaded.");
                    }
                }
                else
				{
                    Log.Error($"Shop item \"{entry}\" is missing arguments, shop item will not be loaded.");
                }
            }
            Shop.ShopString = sb.ToString();

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
            if (Shop.ShopItems.Count != 0) Shop.ShopString += Environment.NewLine;
            Shop.ShopItems.Add(id, new ShopItem()
            {
                perk = perk,
                price = tokens
            });
            Shop.ShopString += $"#{id + 1} | {(isPermanent ? "Permanent" : string.Empty)} {perkName} | {tokens} tokens";
        }

        public override string Author => "Cyanox";
        public override string Name => "TokenShop";
    }

    public static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class, IComparable<T>
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            objects.Sort();
            return objects;
        }
    }
}

using Exiled.API.Interfaces;
using System.ComponentModel;

namespace ServerStatistics
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		// Ban Log

		[Description("The avatar URL for the ban log webhook.")]
		public string BanLogAvatarURL { get; set; } = "https://i.natgeofe.com/k/7ce14b7f-df35-4881-95ae-650bce0adf4d/mallard-male-standing_3x2.jpg";

		[Description("The name for the ban log webhook.")]
		public string BanLogName { get; set; } = "Ban Logger";

		[Description("The webhook URL for the ban log webhook.")]
		public string BanLogWebhook { get; set; } = "https://discord.com/api/webhooks/941751558197088287/qUEslgb4fSOOlgTgqKCmjAjpIDjeN08nSemUh1Ec254eEQeOU6J_Pk6srBDjCcbBgK-e";

		// Command Log

		[Description("The avatar URL for the command log webhook.")]
		public string CommandLogAvatarURL { get; set; } = "https://i.natgeofe.com/k/7ce14b7f-df35-4881-95ae-650bce0adf4d/mallard-male-standing_3x2.jpg";

		[Description("The name for the command log webhook.")]
		public string CommandLogName { get; set; } = "Command Logger";

		[Description("The webhook URL for the command log webhook.")]
		public string CommandLogWebhook { get; set; } = "https://discord.com/api/webhooks/941751558197088287/qUEslgb4fSOOlgTgqKCmjAjpIDjeN08nSemUh1Ec254eEQeOU6J_Pk6srBDjCcbBgK-e";

		// Server Event

		[Description("The avatar URL for the server event webhook.")]
		public string ServerEventAvatarURL { get; set; } = "https://i.natgeofe.com/k/7ce14b7f-df35-4881-95ae-650bce0adf4d/mallard-male-standing_3x2.jpg";

		[Description("The name for the server event webhook.")]
		public string ServerEventName { get; set; } = "Server Events";

		[Description("The webhook URL for the server event webhook.")]
		public string ServerEventWebhook { get; set; } = "https://discord.com/api/webhooks/941751558197088287/qUEslgb4fSOOlgTgqKCmjAjpIDjeN08nSemUh1Ec254eEQeOU6J_Pk6srBDjCcbBgK-e";
	}
}

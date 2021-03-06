﻿using System;
using System.Configuration;
using System.Threading.Tasks;
using Redbridge.SDK;

namespace Redbridge.Configuration
{
public class SystemNotificationConfigurationSection : ConfigurationSection
{
	public static string SectionName => "systemNotifications";

	[ConfigurationProperty("enabled", DefaultValue = "true", IsRequired = false)]
	public bool Enabled
	{
		get { return (bool)this["enabled"]; }
		set { this["enabled"] = value; }
	}

	[ConfigurationProperty("environment", DefaultValue = "Development", IsRequired = false)]
	public string Environment
	{
		get { return (string)this["environment"]; }
		set { this["environment"] = value; }
	}

	[ConfigurationProperty("notifiers", IsDefaultCollection = true)]
	[ConfigurationCollection(typeof(NotifierCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
	public NotifierCollection Notifiers
	{
		get
		{
			var notifierCollection = (NotifierCollection)base["notifiers"];
			return notifierCollection;
		}
		set
		{
			var notifierCollection = value;
			base["notifiers"] = notifierCollection;
		}
	}

	public async Task NotifyAllAsync(NotificationMessage message)
	{
		if (message == null) throw new ArgumentNullException(nameof(message));
		if (Notifiers != null)
		{
			foreach (NotifierConfigurationElement notifier in Notifiers)
			{
				await notifier.NotifyAsync(message);
			}
		}
	} }
}

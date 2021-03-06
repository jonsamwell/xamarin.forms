﻿using System;
using System.Configuration;

namespace Redbridge.Configuration
{
    public class WindowsApplicationSettingsRepository : IApplicationSettingsRepository
	{
		private void ValidateKey(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key));
			}
		}

		public Guid GetGuidValue(string key)
		{
			return Guid.Parse(GetStringValue(key));
		}

		public int GetInt32Value(string key)
		{
			ValidateKey(key);

			var stringValue = ConfigurationManager.AppSettings[key];
			int value;

			if (!int.TryParse(stringValue, out value))
			{
				throw new InvalidConfigurationRepositoryValueException($"Unable to return the application key '{key}' from the configuration or the returned value is not convertible to an Int32 type.");
			}

			return value;
		}

		public bool GetBooleanValue(string key)
		{
			ValidateKey(key);

			var stringValue = ConfigurationManager.AppSettings[key];
			bool value;

			if (!bool.TryParse(stringValue, out value))
			{
				throw new InvalidConfigurationRepositoryValueException(
					$"Unable to return the application key '{key}' from the configuration or the returned value is not convertible to an Boolean type.");
			}

			return value;
		}

		public string GetStringValue(string key)
		{
			ValidateKey(key);

			var stringValue = ConfigurationManager.AppSettings[key];
			return stringValue;
		}

		public Uri GetUrl(string key)
		{
			ValidateKey(key);

			var stringValue = ConfigurationManager.AppSettings[key];
			return new Uri(stringValue);
		}

		public T GetSection<T>(string name) where T : class
		{
			var section = ConfigurationManager.GetSection(name);
			if (section == null) throw new InvalidConfigurationRepositoryValueException($"Unable to locate section {name} in the configuration file.");
			return (T)section;
		}
	}
}

﻿using System;
using Redbridge.SDK;

namespace Redbridge.Security
{
	public class UserCredentials
	{
        public static UserCredentials FromRefreshToken (string token, string authenticationMethod)
        {
            return new UserCredentials()
            {
                AuthenticationMethod = authenticationMethod,
                RefreshToken = token,
            };
        }

        public static UserCredentials AsNonInteractive ()
        {
            return new UserCredentials() { AuthenticationMethod = RegistrationMethods.NonInteractiveDaemon };
        }

        public static UserCredentials FromPassword (string username, string password)
        {
            return new UserCredentials()
            {
                Username = username,
                Password = password,
                AuthenticationMethod = RegistrationMethods.UsernamePassword,
            };
		}

		public string AuthenticationMethod { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
        public string RefreshToken { get; set; }

		public bool IsValid
        {
            get
            {
                // Any refresh token source should act as a valid credential.
                // We won't know it's out of date until we try to use it.
                if ( !string.IsNullOrWhiteSpace(RefreshToken) )
                    return true;

                if ( AuthenticationMethod == RegistrationMethods.UsernamePassword )
                    return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
                else
                    return true;
            }
        }

        internal static UserCredentials Empty(string authenticationMethod)
        {
            return new UserCredentials() { AuthenticationMethod = authenticationMethod };
        }

        internal static UserCredentials New()
        {
            return new UserCredentials();
        }
    }
}

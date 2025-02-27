﻿using ArkEcho.Core;
using System;
using System.Threading.Tasks;

namespace ArkEcho.WebPage
{
    public class Authentication
    {
        public User AuthenticatedUser { get; private set; } = null;

        private Rest rest = null;
        private ILocalStorage localStorage = null;

        public Authentication(ILocalStorage localStorage, Rest rest)
        {
            this.localStorage = localStorage;
            this.rest = rest;
        }

        public async Task<bool> GetAuthenticationState()
        {
            Guid accessToken = Guid.Empty;

            try
            {
                accessToken = await localStorage.GetItemAsync<Guid>("accessToken");

                AuthenticatedUser = await rest.CheckUserToken(accessToken);

                return AuthenticatedUser != null;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public async Task<bool> AuthenticateUserForLogin(string username, string password)
        {
            AuthenticatedUser = await rest.AuthenticateUserForLogin(new User() { UserName = username, Password = password });

            if (AuthenticatedUser != null)
                await localStorage.SetItemAsync("accessToken", AuthenticatedUser.AccessToken);

            return AuthenticatedUser != null;
        }

        public async Task MarkUserAsLoggedOut()
        {
            try
            {
                await localStorage.RemoveItemAsync("accessToken");
                AuthenticatedUser = null;
            }
            catch (Exception)
            {
            }
        }
    }
}
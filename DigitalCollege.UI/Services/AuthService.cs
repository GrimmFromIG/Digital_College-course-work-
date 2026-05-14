using DigitalCollege.UI.Models;
using System;

namespace DigitalCollege.UI.Services
{
    public class AuthService
    {
        public UserModel CurrentUser { get; private set; }
        public event Action OnChange;

        public void Login(string role, int id, string name)
        {
            CurrentUser = new UserModel 
            { 
                Role = role, 
                Id = id, 
                FullName = name,
                Username = role.ToLower() 
            };
            NotifyStateChanged();
        }

        public void Logout()
        {
            CurrentUser = null;
            NotifyStateChanged();
        }

        public bool IsInRole(string role)
        {
            return CurrentUser?.Role == role;
        }

        public bool IsAuthenticated => CurrentUser != null;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
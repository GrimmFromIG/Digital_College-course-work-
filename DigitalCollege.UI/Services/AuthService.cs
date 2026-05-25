using DigitalCollege.UI.Models;
using Microsoft.JSInterop;

namespace DigitalCollege.UI.Services
{
    public class AuthService
    {
        private readonly ApiClient _apiClient;
        private readonly IJSRuntime _jsRuntime;

        public UserModel CurrentUser { get; private set; }
        public event Action OnChange;

        public AuthService(ApiClient apiClient, IJSRuntime jsRuntime)
        {
            _apiClient = apiClient;
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            var role = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userRole");
            var idStr = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userId");
            var fullName = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userFullName");

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(role))
            {
                _apiClient.SetToken(token);
                int.TryParse(idStr, out int id);
                
                CurrentUser = new UserModel 
                { 
                    Role = role, 
                    Id = id, 
                    FullName = fullName 
                };
                NotifyStateChanged();
            }
        }

        public async Task<bool> LoginWithCredentialsAsync(string email, string password)
        {
            var response = await _apiClient.LoginAsync(new LoginDto { Email = email, Password = password });
            
            if (response != null && !string.IsNullOrEmpty(response.Token))
            {
                int userId = response.Role == "Teacher" ? (response.TeacherId ?? 0) : 
                             response.Role == "Student" ? (response.StudentId ?? 0) : 1;

                CurrentUser = new UserModel 
                { 
                    Role = response.Role, 
                    Id = userId, 
                    FullName = response.FullName,
                    Username = email
                };

                _apiClient.SetToken(response.Token);

                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", response.Token);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userRole", response.Role);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userId", userId.ToString());
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userFullName", response.FullName);

                NotifyStateChanged();
                return true;
            }
            
            return false;
        }

        public async Task LoginAsGuestAsync()
        {
            CurrentUser = new UserModel { Role = "Unregistered", Id = 0, FullName = "Гість", Username = "guest" };
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            _apiClient.SetToken(null);
            NotifyStateChanged();
        }

        public async Task LogoutAsync()
        {
            CurrentUser = null;
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userRole");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userId");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userFullName");
            _apiClient.SetToken(null);
            NotifyStateChanged();
        }

        public bool IsInRole(string role) => CurrentUser?.Role == role;
        public bool IsAuthenticated => CurrentUser != null && CurrentUser.Role != "Unregistered";

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
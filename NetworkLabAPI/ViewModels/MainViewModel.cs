using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using NetworkLabAPI.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Input;
using System;

namespace NetworkLabAPI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _searchText = string.Empty;
        private string _errorMessage = string.Empty;
        private ObservableCollection<Country> _countries = [];
        private static readonly HttpClient _httpClient = new();
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public ObservableCollection<Country> Countries
        {
            get => _countries;
            set
            {
                _countries = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public AsyncRelayCommand SearchCommand { get; }

        public MainWindowViewModel()
        {
            SearchCommand = new AsyncRelayCommand(ExecuteSearchAsync);
        }

        private async Task ExecuteSearchAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                ErrorMessage = "Please enter a country name";
                return;
            }

            try
            {
                var response = await _httpClient.GetAsync(
                    $"https://restcountries.com/v3.1/name/{SearchText}");

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var countries = JsonSerializer.Deserialize<List<Country>>(json, _jsonSerializerOptions);

                Countries.Clear();
                foreach (var country in countries)
                {
                    Countries.Add(country);
                }
                OnPropertyChanged("Countries");
                ErrorMessage = string.Empty;
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = $"Error fetching data: {ex.Message}";
                Countries.Clear();
            }
            catch (JsonException)
            {
                ErrorMessage = "Error parsing country data";
                Countries.Clear();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => canExecute?.Invoke() ?? true;

        public async void Execute(object? parameter) => await execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
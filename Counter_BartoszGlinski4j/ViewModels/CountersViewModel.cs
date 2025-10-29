using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Counter_BartoszGlinski4j.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Counter_BartoszGlinski4j.ViewModels
{
    internal class CountersViewModel : IQueryAttributable
    {
        public ObservableCollection<CounterViewModel> AllCounters { get; }
        public ICommand NewCommand { get; }

        public CountersViewModel()
        {
            AllCounters = new ObservableCollection<CounterViewModel>(
                Counter.LoadAll().Select(c => new CounterViewModel(c))
            );
            NewCommand = new AsyncRelayCommand(NewCounterAsync);
        }

        private async Task NewCounterAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.CounterPage));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string counterId = query["deleted"].ToString();
                var matched = AllCounters.FirstOrDefault(c => c.Identifier == counterId);
                if (matched != null)
                    AllCounters.Remove(matched);
            }
            else if (query.ContainsKey("saved"))
            {
                string counterId = query["saved"].ToString();
                var matched = AllCounters.FirstOrDefault(c => c.Identifier == counterId);

                if (matched != null)
                {
                    matched.Reload();
                    AllCounters.Move(AllCounters.IndexOf(matched), 0);
                }
                else
                {
                    AllCounters.Insert(0, new CounterViewModel(Models.Counter.Load(counterId)));
                }
            }
        }

    }
}

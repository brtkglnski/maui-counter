using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Counter_BartoszGlinski4j.ViewModels
{
    internal class CounterViewModel : ObservableObject, IQueryAttributable
    {
        private Models.Counter _counter;

        public ICommand SaveCommand { get; private set; }
        public ICommand IncrementCommand { get; private set; }
        public ICommand DecrementCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }

        public int Amount
        {
            get => _counter.Amount;
            set
            {
                if (_counter.Amount != value)
                {
                    _counter.Amount = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _counter.Name;
            set
            {
                if (_counter.Name != value)
                {
                    _counter.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public int InitialAmount
        {
            get => _counter.InitialAmount;
            set
            {
                if (_counter.InitialAmount != value)
                {
                    _counter.InitialAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InitialAmountString
        {
            get => _counter.InitialAmount.ToString();
            set
            {
                if (int.TryParse(value, out int result))
                {
                    _counter.InitialAmount = result;
                    _counter.Amount = result;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Amount));
                }
            }
        }

        private void IncrementAmount()
        {
            Amount++;
            _counter.Save();
        }

        private void DecrementAmount()
        {
            Amount--;
            _counter.Save();
            
        }

        private void ResetAmount()
        {
            Amount = _counter.InitialAmount;
            _counter.Save();
        }

        private async Task RemoveCounterAsync()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, _counter.Filename);
            if (File.Exists(path))
                File.Delete(path);

            await Shell.Current.GoToAsync($"..?deleted={_counter.Filename}");
        }

        public DateTime Date => _counter.Date;
        public string Identifier => _counter.Filename;

        public CounterViewModel()
        {
            _counter = new Models.Counter();
            SaveCommand = new AsyncRelayCommand(Save);
            IncrementCommand = new RelayCommand(IncrementAmount);
            DecrementCommand = new RelayCommand(DecrementAmount);
            ResetCommand = new RelayCommand(ResetAmount);
            RemoveCommand = new AsyncRelayCommand(RemoveCounterAsync);
        }

        public CounterViewModel(Models.Counter counter)
        {
            _counter = counter;
            SaveCommand = new AsyncRelayCommand(Save);
            IncrementCommand = new RelayCommand(IncrementAmount);
            DecrementCommand = new RelayCommand(DecrementAmount);
            ResetCommand = new RelayCommand(ResetAmount);
            RemoveCommand = new AsyncRelayCommand(RemoveCounterAsync);
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("load"))
            {
                _counter = Models.Counter.Load(query["load"].ToString());
                RefreshProperties();
            }
        }

        private async Task Save()
        {
            _counter.Save();
            await Shell.Current.GoToAsync($"..?saved={_counter.Filename}");
        }

        public void Reload()
        {
            _counter = Models.Counter.Load(_counter.Filename);
            RefreshProperties();
        }

        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(Amount));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(InitialAmount));
            OnPropertyChanged(nameof(InitialAmountString));
        }
    }
}

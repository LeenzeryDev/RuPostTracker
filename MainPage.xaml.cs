using RPTApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace RuPostTracker
{
    public class Recording
    {
        public string OperationName { get; set; }
        public string OperationAddress { get; set; }
        public string OperationDate { get; set; }
        public Recording()
        {
            this.OperationName = "Wolfgang Amadeus Mozart";
            this.OperationAddress = "Andante in C for Piano";
            this.OperationDate = "cock";
        }
        public string OneLineSummary
        {
            get
            {
                return $"{this.OperationAddress} by {this.OperationName}, released: "
                    + this.OperationDate;
            }
        }
    }
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        RuPostApi api = new RuPostApi("gclqOxkSdmxfSc", "SqnGjTVOvT1b");
        private ObservableCollection<Recording> recordings = new ObservableCollection<Recording>();
        public ObservableCollection<Recording> Recordings { get { return this.recordings; } }
        private async void GetButton_Click(object sender, RoutedEventArgs e)
        {
            try { 
                recordings.Clear();
                var records = await api.GetOperationsHistoryAsync(barcodeBox.Text);
                //Info.Text = $"{records.OperationHistoryData[5].ItemParameters.MailType.Name} • {records.OperationHistoryData[0].ItemParameters.Mass}г";
                //Sender.Text = $"{records.OperationHistoryData[5].UserParameters.Sndr}";
                //Recipient.Text = $"{records.OperationHistoryData[5].UserParameters.Rcpn}";
                //Address.Text = $"{records.OperationHistoryData[5].AddressParameters.DestinationAddress.Description}";
                foreach (var record in records.OperationHistoryData)
                {
                    recordings.Add(new Recording()
                    {
                        OperationName = $"{record.OperationParameters.OperAttr.Name}",
                        OperationAddress = $"{record.AddressParameters.OperationAddress.Description}",
                        OperationDate = $"{record.OperationParameters.OperDate.ToLocalTime().ToString()}"
                    });
                }
            }
            catch(Exception ex)
            {
                var dialog = new MessageDialog(ex.ToString());
                await dialog.ShowAsync();
            }
        }
    }
}

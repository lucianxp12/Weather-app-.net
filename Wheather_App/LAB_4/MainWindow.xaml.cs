using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LAB_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class DateVreme
    {
        public const string WeatherApiUrl = "http://api.openweathermap.org/data/2.5/weather?q=";
        public const string WeatherApiKey = "52972d2e39f6c6d5b5266f1a0c8be216";
    }

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        HttpClient hClient;
        public MainWindow()
        {
            hClient = new HttpClient();
            this.DataContext = this;
            InitializeComponent();
        }
        private string _cityName;
        public string CityName
        {
            get { return _cityName; }
            set
            {
                if (value != _cityName)
                {
                    _cityName = value;
                    PropertyChanged?.Invoke(this, new
                    PropertyChangedEventArgs(nameof(CityName)));
                }
            }
        }
        private string _temperature;
        public string Temperature
        {
            get { return _temperature; }
            set
            {
                if (value != _temperature)
                {
                    _temperature = value;
                    PropertyChanged?.Invoke(this, new
                    PropertyChangedEventArgs(nameof(Temperature)));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private async void btnGetWeather_Click(object sender, RoutedEventArgs e)
        {
            
            var url = $"{DateVreme.WeatherApiUrl}{_cityName}&appid={DateVreme.WeatherApiKey}";
            var resString = "";
            try
            {
                resString = await hClient.GetStringAsync(url);
                var resObject = JsonConvert.DeserializeObject<RootObject>(resString);
                Double temp_1 = resObject.main.temp;
                Double windSpeed = resObject.wind.speed;
                Double pressure = resObject.main.pressure;
                Double humidity = resObject.main.humidity;
                List<Weather> wat = resObject.weather;
                string miuTemp = "";
                foreach (Weather war in wat)
                {
                    miuTemp += war.icon;
                }
                miuTemp = selectImage(miuTemp);



                if (Radio_GradeCelsius.IsChecked.Value)
                {
                    Temperature = ConvertToCelsius(temp_1);
                }
                else
                {
                    Temperature = ConvertToFarenheit(temp_1);
                }

                RxWindSpeed.Text = windSpeed + " m/s";
                RxHumidity.Text = humidity + "%";
                RxPressure.Text = pressure + "hPa";
                RxImage.Source = new BitmapImage(new Uri(miuTemp, UriKind.Relative));
            }
            catch (Exception)
            {

                Temperature = "City name Not found";
            }
            

            
        }

        private string selectImage(string image)
        {

            string path = "/Icons/"+ image + ".png";

            return path;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private String ConvertToCelsius(double temp)
        {
            double c = temp - 273.15;
            c = Math.Round((Double)c, 2);

            return c + " °C";
        }

        private String ConvertToFarenheit(double temp)
        {
            double c = (temp * 9 / 5) - 459.67;
            c = Math.Round((Double)c, 2);
            return c + " °F";
        }

        private void TxWindSpeed_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

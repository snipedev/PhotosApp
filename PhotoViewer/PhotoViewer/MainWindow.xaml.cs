using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ClientApp;

namespace PhotoViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Client _client;
        
        public MainWindow()
        {
            InitializeComponent();
            _client = new Client();
            ExceptionHandler handler = new();
            handler.SetupExceptionHandling();
        }

        

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            sPanel?.Children.Clear();

            var response = await _client.GetPhotosByTagAsync(txtSearch.Text);
            if (response != null && sPanel is not null)
            {
                foreach (var photo in response)
                {
                    Viewbox myvBox = new Viewbox();
                    myvBox.Height = 100;
                    myvBox.Width = 100;
                    Image myImage = new Image();
                    myImage.Margin = new Thickness(5, 2, 5, 0);
                    BitmapImage myBmapImage = new BitmapImage();
                    myBmapImage.BeginInit();
                    myBmapImage.UriSource = new Uri(photo.ThumbnailUrl);
                    myBmapImage.DecodePixelWidth = 200;
                    myBmapImage.EndInit();
                    myImage.Source = myBmapImage;
                    myImage.MouseDown += (object sender, MouseButtonEventArgs e) => { Image_Click(sender, e, photo.LargeUrl); };
                    myvBox.Child = myImage;
                    //myvBox.MouseRightButtonDown
                    sPanel.Children.Add(myvBox);
                }
            }
            else
            {
                Label myLbl = new();
                myLbl.Content = $"Sorry! The Photos are not available for {txtSearch.Text}";
            }
        }

        private void Image_Click(Object sender, MouseButtonEventArgs e,string Url)
        {
                pictureVBox.Child = null;
                Image myImage = new();
                BitmapImage myBmapImage = new BitmapImage();
                myBmapImage.BeginInit();
                myBmapImage.UriSource = new Uri(Url);
                myBmapImage.EndInit();
                myImage.Source = myBmapImage;
                pictureVBox.Child = myImage;
                //grdPhotoViewer.Background = new System.Windows.Media.ImageBrush() { ImageSource = myBmapImage };
                sPanel.Visibility = Visibility.Hidden;
                txtSearch.Visibility = Visibility.Hidden;
                btnSearch.Visibility = Visibility.Hidden;
                grdPhotoViewer.Visibility = Visibility.Visible;
            
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            sPanel.Visibility = Visibility.Visible;
            txtSearch.Visibility = Visibility.Visible;
            btnSearch.Visibility = Visibility.Visible;
            grdPhotoViewer.Visibility = Visibility.Hidden;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            pictureVBox.Height = e.NewValue;
            pictureVBox.Width = e.NewValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ClientApp;
using ClientApp.Model;

namespace PhotoViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Client _client;
        private Queue<PhotoResponse> histories = new();
        private PhotoResponse? history = new();

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
            var photoResponse = new PhotoResponse();
            photoResponse.Photos = await _client.GetPhotosByTagAsync(txtSearch.Text);
            photoResponse.Text = txtSearch.Text;
            if (photoResponse.Photos != null && sPanel is not null)
            {
                PopulateImages(photoResponse);
                if (histories.Count == 1)
                {
                    history = histories.Dequeue();
                    histories.Enqueue(photoResponse);
                    btnPrevious.IsEnabled = true;
                }
                else if(histories.Count < 1)
                {
                    histories.Enqueue(photoResponse);
                }

            }
            else
            {
                Label myLbl = new();
                myLbl.Content = $"Sorry! The Photos are not available for {txtSearch.Text}";
            }
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

        #region Helper Methods
        /// <summary>
        /// This method shows the selected image enlarged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="Url"></param>

        private void Image_Click(Object sender, MouseButtonEventArgs e, string Url)
        {
            pictureVBox.Child = null;
            var myImage = CreateImage(Url);
            pictureVBox.Child = myImage;
            sPanel.Visibility = Visibility.Hidden;
            txtSearch.Visibility = Visibility.Hidden;
            btnSearch.Visibility = Visibility.Hidden;
            btnPrevious.Visibility = Visibility.Hidden;
            grdPhotoViewer.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Method for creating the image
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private Image CreateImage(string url)
        {
            Image myImage = new();
            BitmapImage myBmapImage = new BitmapImage();
            myBmapImage.BeginInit();
            myBmapImage.UriSource = new Uri(url);
            myBmapImage.EndInit();
            myImage.Source = myBmapImage;
            return myImage;
        }

        /// <summary>
        /// Method for populating the Image grid
        /// </summary>
        /// <param name="response"></param>

        private void PopulateImages(PhotoResponse response)
        {
            if (response.Photos is not null)
            {
                foreach (var photo in response.Photos)
                {
                    Viewbox myvBox = new Viewbox();
                    myvBox.Height = 100;
                    myvBox.Width = 100;
                    var myImage = CreateImage(photo.ThumbnailUrl);
                    myImage.Margin = new Thickness(5, 2, 5, 0);
                    myImage.MouseDown += (object sender, MouseButtonEventArgs e) => { Image_Click(sender, e, photo.LargeUrl); };
                    myvBox.Child = myImage;
                    sPanel.Children.Add(myvBox);
                }
            }
        }

        /// <summary>
        /// Handles the previous button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (history is not null)
            {
                sPanel?.Children.Clear();
                PopulateImages(history);
                txtSearch.Text = history.Text;
                history = null;
                btnPrevious.IsEnabled = false;
            }
        }
    #endregion
    }
}

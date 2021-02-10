using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.IO;

namespace SzanyiLevente_AudioLejatszo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        readonly MediaPlayer mediaPlayer = new MediaPlayer();
        readonly DispatcherTimer timer = new DispatcherTimer();
        int positionSliderIsMoving = 0;
        int isPlaying = 0;
        int playing = -1;
        int repeatType = 0;

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 files (*.mp3)|*.mp3",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    if (!lista.Items.Contains(file))
                    {
                        lista.Items.Add(file);
                    }
                }
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
           if (lista.Items.Count != 0)
            {
                if (repeatType == 0)
                {
                    if (lista.SelectedIndex != -1)
                    {
                        string file = lista.Items[lista.SelectedIndex].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.Play();
                        repeatType = 2;
                        playing = Convert.ToInt32(lista.SelectedIndex);

    
                    }
                    else
                    {

                        lista.SelectedIndex = 0;
                        string file = lista.Items[0].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.Play();
                        repeatType = 2;
                        playing = 0;
                    }
                }
                else if (repeatType == 1)
                {
                    mediaPlayer.Play();
                    repeatType = 2;
                }
                else
                {
                    mediaPlayer.Pause();
                    repeatType = 1;
                }
            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            PositionSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            PositionSlider.IsEnabled = true;
            PositionSlider.Value = mediaPlayer.Position.TotalSeconds;
            if (PositionSlider.Value == mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds)
            {
                timer.Stop();
                PositionSlider.Value = 0;
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
        }




        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)volumeSlider.Value;
        }

        private void PositionSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            positionSliderIsMoving = 1;
        }

        private void PositionSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            positionSliderIsMoving = 0;
            mediaPlayer.Position = TimeSpan.FromSeconds(PositionSlider.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PositionLabel1.Content = String.Format("{0}/{1}", TimeSpan.FromSeconds(PositionSlider.Value).ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
        }

        private void Box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            playing = playing - 1;
            lista.SelectedIndex = playing;
            string file = lista.Items[lista.SelectedIndex].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            PositionSlider.IsEnabled = true;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            playing++;
            lista.SelectedIndex = playing;
            string file = lista.Items[lista.SelectedIndex].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            PositionSlider.IsEnabled = true;
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (repeatType == 0)
            {
                repeatType = 1;
            }
            else if (repeatType == 1)
            {
                repeatType = 2;
                Repeat.Content = "ON";
            }
            else
            {
                repeatType = 0;
                Repeat.Content = "OFF";

            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Items.Count == 0)
            {
                
            }
            else
            {
                if (lista.SelectedIndex == -1)
                {
                    
                }
                else
                {
                    if (lista.SelectedIndex == playing)
                    {
                        lista.Items.Remove(lista.SelectedItem);
                        lista.SelectedIndex = -1;
                        mediaPlayer.Close();
                        playing = -1;
                        repeatType = 0;
                    }
                    else
                    {
                        if (lista.SelectedIndex > playing)
                        {
                            lista.Items.Remove(lista.SelectedItem);
                            lista.SelectedIndex = playing;
                        }
                        else
                        {
                            playing--;
                            lista.Items.Remove(lista.SelectedItem);
                            lista.SelectedIndex = playing;
                        }
                    }
                }
            }
        }

        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lista_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string file = lista.Items[lista.SelectedIndex].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            playing = Convert.ToInt32(lista.SelectedIndex);
            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Start();
            PositionSlider.IsEnabled = true;

        }

       
    }
}

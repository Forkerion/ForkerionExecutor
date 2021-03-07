using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Forkerion;

namespace Forkerion
{
    /// <summary>
    /// Interaction logic for Window8.xaml
    /// </summary>
    public partial class ErrorMessage : Window
    {
        Storyboard StoryBoard = new Storyboard();
        TimeSpan duration = TimeSpan.FromMilliseconds(500);
        TimeSpan duration2 = TimeSpan.FromMilliseconds(1000);

        private IEasingFunction Smooth
        {
            get;
            set;
        }
        = new QuarticEase
        {
            EasingMode = EasingMode.EaseInOut
        };

        public void Fade(DependencyObject Object)
        {
            DoubleAnimation FadeIn = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(duration),
            };
            Storyboard.SetTarget(FadeIn, Object);
            Storyboard.SetTargetProperty(FadeIn, new PropertyPath("Opacity", 1));
            StoryBoard.Children.Add(FadeIn);
            StoryBoard.Begin();
        }

        public void FadeOut(DependencyObject Object)
        {// but my tits are cold oof ok
            DoubleAnimation Fade = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(duration),
            };
            Storyboard.SetTarget(Fade, Object);
            Storyboard.SetTargetProperty(Fade, new PropertyPath("Opacity", 1));
            StoryBoard.Children.Add(Fade);
            StoryBoard.Begin();
        }

        public void ObjectShift(DependencyObject Object, Thickness Get, Thickness Set)
        {
            ThicknessAnimation Animation = new ThicknessAnimation()
            {
                From = Get,
                To = Set,
                Duration = duration2,
                EasingFunction = Smooth,
            };
            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(MarginProperty));
            StoryBoard.Children.Add(Animation);
            StoryBoard.Begin();
        }
        public ErrorMessage()
        {//hold on
            InitializeComponent();
        }
        
        //lets see what happens

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CocoMessageBox.Text = Forkerion.Properties.Settings.Default.ErrorMessage;
            Fade(CocoMessageBox);
            Fade(Button1);
            ObjectShift(CocoMessageBox, CocoMessageBox.Margin, new Thickness(210, 149, 0, 0));
            ObjectShift(Button1, Button1.Margin, new Thickness(415, 256, 0, 119));
        }

        private void MainBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discord.gg/s5mEGRZ");
        }
    }
}

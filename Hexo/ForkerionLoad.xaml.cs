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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Forkerion
{
    /// <summary>
    /// Interaction logic for ForkerionLoad.xaml
    /// </summary>
    public partial class ForkerionLoad : Window
    {
        MainWindow Main = new MainWindow();
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
        {
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

        public ForkerionLoad()
        {
            InitializeComponent();
        }

        private async void Forkerion_Loaded(object sender, RoutedEventArgs e)
        {
            Fade(ForkerionBG);
            Fade(ForkerionText);
            ObjectShift(ForkerionBG, ForkerionBG.Margin, new Thickness(0, 0, 0, 0));
            ObjectShift(ForkerionText, ForkerionText.Margin, new Thickness(0, 0, 0, 0));
            await Task.Delay(1500);
            FadeOut(ForkerionBG);
            FadeOut(ForkerionText);
            ObjectShift(ForkerionBG, ForkerionBG.Margin, new Thickness(472, 0, 0, 0));
            ObjectShift(ForkerionText, ForkerionText.Margin, new Thickness(-472, 0, 0, 0));
            await Task.Delay(1000);
            this.Hide();
            Main.Show();
        }
    }
}

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Forkerion;
using Forkerion;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace Forkerion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ErrorMessage Error = new ErrorMessage();
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
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            TextEditor.ShowLineNumbers = true;
            TextEditor.Options.EnableHyperlinks = false;
            TextEditor.Options.ShowSpaces = false;
            TextEditor.Options.ShowTabs = false;
            TextEditor.TextArea.TextView.ElementGenerators.Add(new NoLongLines());
            Stream stream = File.OpenRead("./bin/lua.xshd");
            XmlTextReader reader = new XmlTextReader(stream);
            TextEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            ItemAdd(ListBox, "./scripts", "*.txt");
            ItemAdd(ListBox, "./scripts", "*.lua");
            TextEditor.Text = "-- Forkerion Executor\n-- Discord: https://discord.gg/YFXBSdD7cJ \nprint(\"Executed.\")";

            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += timer_Tick;

        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            Process[] rbProcess = Process.GetProcessesByName("RobloxPlayerBeta");
            if (rbProcess.Length >= 1)
            {
                if (API.isOXygenUAttached() == false)
                {
                    await Task.Delay(5000);
                    API.Attach();
                }
            }
        }
        private void Forkerion_Loaded(object sender, RoutedEventArgs e)
        {
            Fade(MainBorder);
            Fade(TopBorder);
            Fade(ButtonBorders);
            ObjectShift(MainBorder, MainBorder.Margin, new Thickness(0, 0, 0, 0));
            ObjectShift(TopBorder, TopBorder.Margin, new Thickness(0, 0, 0, 0));
            ObjectShift(TextEditor, TextEditor.Margin, new Thickness(2, 28, 116, 33));
            ObjectShift(ListBox, ListBox.Margin, new Thickness(458, 28, 2, 33));
            ObjectShift(ButtonBorders, ButtonBorders.Margin, new Thickness(2, 273, 0, 0));
        }

        private void TopBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public static void ItemAdd(System.Windows.Controls.ListBox ListBox, string Folder, string FileType)
        {
            DirectoryInfo Directory = new DirectoryInfo(Folder);
            FileInfo[] Files = Directory.GetFiles(FileType);
            foreach (FileInfo file in Files)
            {
                ListBox.Items.Add(file.Name);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox.SelectedIndex != -1)
            {
                TextEditor.Text = File.ReadAllText("Scripts\\" + ListBox.SelectedItem.ToString());
            }
        }
        ForkerionAPI.Client API = new ForkerionAPI.Client();
        private async void InjectButton_Click(object sender, RoutedEventArgs e)
        {
            API.Attach();
            await Task.Delay(8000);
            API.Execute("game.StarterGui:SetCore(\"SendNotification\", {Title = \"Forkerion Injected.\"; Text = \"Good grammar is sexy as fuck.\"; Icon = \"rbxassetid://57254792\";Duration = 5;})");
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            API.Execute(TextEditor.Text);   
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog Script = new OpenFileDialog();
            Script.Filter = "Text files (*.txt;*.lua)|*.txt;*.lua|All files (*.*)|*.*";
            if (Script.ShowDialog() == true)
            {
                string s = File.ReadAllText(Script.FileName);
                TextEditor.Text = s;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog Script = new SaveFileDialog();
            Script.Filter = "Text files (*.txt;*.lua)|*.txt;*.lua|All files (*.*)|*.*";
            if (Script.ShowDialog() == true)
            {
                File.WriteAllText(Script.FileName, TextEditor.Text);
            }
        }

        private void EraseButton_Click(object sender, RoutedEventArgs e)
        {
            TextEditor.Text = "";
        }

        private async void SettingsExit(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {

                FadeOut(SettingsTopBorder);
                FadeOut(Settings);
                await Task.Delay(1000);
                Settings.Visibility = Visibility.Hidden;
            }
        }

        private async void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
                
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {

                FadeOut(MainBorder);
                FadeOut(TopBorder);
                FadeOut(ButtonBorders);
                ObjectShift(MainBorder, MainBorder.Margin, new Thickness(49, 70, 49, 26));
                ObjectShift(TopBorder, TopBorder.Margin, new Thickness(0, -28, 0, 0));
                ObjectShift(TextEditor, TextEditor.Margin, new Thickness(-50, 28, 168, 33));
                ObjectShift(ListBox, ListBox.Margin, new Thickness(510, 28, -50, 33));
                ObjectShift(ButtonBorders, ButtonBorders.Margin, new Thickness(2, 304, 0, -28));
                await Task.Delay(1000);
                Environment.Exit(0);
            }
        }

        private void MinimizeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

            Settings.Visibility = Visibility.Visible;
            Fade(Settings);
            Fade(SettingsTopBorder);
        }


        private void ConvBrn_Click(object sender, RoutedEventArgs e)
        {
            LSMTB.Text = $"loadstring(game:HttpGet(\"{LSMTB.Text}\", true))()";
        }

        private void topMostCB_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void topMostCB_Click(object sender, RoutedEventArgs e)
        {
            if(topMostCB.IsChecked == true)
            {
                this.Topmost = true;
            }
            else
            {
                this.Topmost = false;
            }
        }

        private void autoAttachCB_Click(object sender, RoutedEventArgs e)
        {
            if (autoAttachCB.IsChecked == true)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }
    }
}

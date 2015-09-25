using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using IMPressive.Properties;

namespace IMPressive
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Version AssemblyVersion => Assembly.GetEntryAssembly().GetName().Version;

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            foreach (
                var f in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory).Where(x => x.Contains("Texture")))
            {
                File.Delete(f);
            }
        }

        public static ResourceDictionary CurrentTheme { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SwitchTheme(Settings.Default.Theme);
        }

        public static void SwitchTheme(string nom)
        {
            string url;
            switch (nom)
            {
                case "Windows 8":
                    url = "Windows8";
                    break;
                case "Système":
                    url = "SystemTheme";
                    break;
                default:
                    url = "Windows7";
                    break;
            }
            var uri = new Uri("/IMPressive;component/Themes/" + url + ".xaml", UriKind.Relative);

            var resourceDictionary = LoadComponent(uri) as ResourceDictionary;
            if (resourceDictionary != null)
            {
                Current.Resources.MergedDictionaries[0].MergedDictionaries.Clear();
                Current.Resources.MergedDictionaries[0].MergedDictionaries.Add(resourceDictionary);
                CurrentTheme = resourceDictionary;
            }
        }
    }

    public enum Theme
    {
        [Description("Windows 7")] Windows7,
        [Description("Windows 8")] Windows8,
        [Description("Système")] System
    }
}
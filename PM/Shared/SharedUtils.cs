using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using Newtonsoft.Json;


namespace PM.Shared
{
    public class SharedUtils
    {
        #region Singleton Implementation
        private static SharedUtils _instance = null;
        private SharedUtils()
        {
            AppDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Load App Settings
            var configFile = Path.Combine(AppDirectory, Properties.Settings.Default.AppSettingsFilename);
            if (File.Exists(configFile))
            {
                var json = File.ReadAllText(configFile);
                AppSettings = JsonConvert.DeserializeObject<AppSetting>(json);
            }
            else
                throw new FileNotFoundException($"Application settings file '{configFile}' not found.");

            // Populate Default Properties
            UserName = Environment.UserName;
            RegisteredName = AppSettings.RegisteredName;
        }
        public static SharedUtils Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SharedUtils();
                return _instance;
            }
        }
        #endregion

        public AppSetting AppSettings { get; private set; }
        public string UserName { get; }
        public string AppDirectory { get; }
        public string RegisteredName { get; }

        public T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public ImageSource GetDXImageSource (string imageName)
        {
            return new DXImageExtension() { Image = new DXImageConverter().ConvertFrom(null, null, imageName) as DXImageInfo }.ProvideValue(null) as ImageSource;
        }

        public ImageSource ConvertBitmapToImageSource(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

    }
}

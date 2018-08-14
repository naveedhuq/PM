﻿using System;
using System.IO;
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

        public AppSetting AppSettings { get; }
        public string UserName { get; }
        public string AppDirectory { get; }
        public string RegisteredName { get; }

    }
}

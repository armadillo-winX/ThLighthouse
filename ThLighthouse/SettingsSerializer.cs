using System.Xml.Serialization;

namespace ThLighthouse
{
    public class SettingsSerializer
    {
        public string? ThtkDirectory { get; set; }

        public static void SaveSettings()
        {
            SettingsSerializer serializer = new SettingsSerializer();
            serializer.ThtkDirectory = Settings.ThtkDirectory;

            string? settingsFilePath = PathInfo.SettingsFile;

            XmlSerializer settingsSerializer = new(typeof(SettingsSerializer));
            FileStream fileStream = new(settingsFilePath, FileMode.Create);
            settingsSerializer.Serialize(fileStream, serializer);
            fileStream.Close();
        }

        public static void ConfigureSettings()
        {
            string? settingsFile = PathInfo.SettingsFile;

            if (File.Exists(settingsFile))
            {
                XmlSerializer settingsSerializer = new(typeof(SettingsSerializer));
                FileStream fs = new(settingsFile, FileMode.Open);

                SettingsSerializer settings = (SettingsSerializer)settingsSerializer.Deserialize(fs);
                fs.Close();

                Settings.ThtkDirectory = settings.ThtkDirectory;
            }
            else
            {
                Settings.ThtkDirectory = "";
            }
        }
    }
}

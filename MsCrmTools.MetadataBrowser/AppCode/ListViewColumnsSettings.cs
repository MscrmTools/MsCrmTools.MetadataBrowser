using System;
using System.IO;
using System.Reflection;

namespace MsCrmTools.MetadataBrowser.AppCode
{
    public class ListViewColumnsSettings : ICloneable
    {
        private static readonly string[] attributeFirstColumns = { "LogicalName", "SchemaName", "AttributeType" };
        private static readonly string[] entityAttributesToIgnore = { "Attributes", "Privileges", "OneToManyRelationships", "ManyToOneRelationships", "ManyToManyRelationships", "Keys" };
        private static readonly string[] entityFirstColumns = { "LogicalName", "SchemaName", "ObjectTypeCode" };
        private static readonly string[] privFirstColumns = { "Name" };
        private static readonly string[] relFirstColumns = { "SchemaName" };
        private static readonly string[] keyFirstColumns = { "SchemaName" };

        public static string[] AttributeFirstColumns
        {
            get { return attributeFirstColumns; }
        }

        public static string[] EntityAttributesToIgnore
        {
            get { return entityAttributesToIgnore; }
        }

        public static string[] EntityFirstColumns
        {
            get { return entityFirstColumns; }
        }

        public static string[] PrivFirstColumns
        {
            get { return privFirstColumns; }
        }

        public static string[] KeyFirstColumns
        {
            get { return keyFirstColumns; }
        }

        public static string[] RelFirstColumns
        {
            get { return relFirstColumns; }
        }

        public string[] AttributeSelectedAttributes { get; set; }
        public string[] EntitySelectedAttributes { get; set; }
        public string[] KeySelectedAttributes { get; internal set; }
        public string[] MtmRelSelectedAttributes { get; set; }
        public string[] OtmRelSelectedAttributes { get; set; }
        public string[] PrivSelectedAttributes { get; set; }

        public static ListViewColumnsSettings LoadSettings()
        {
            string fileName = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "MscrmTools.MetadataBrowser.config");
            if (File.Exists(fileName))
            {
                using (var reader = new StreamReader(fileName))
                {
                    var settingsString = reader.ReadToEnd();

                    var settings = settingsString.Split('|');

                    return new ListViewColumnsSettings
                    {
                        EntitySelectedAttributes = string.IsNullOrEmpty(settings[0]) ? null : settings[0].Split(';'),
                        AttributeSelectedAttributes = string.IsNullOrEmpty(settings[1]) ? null : settings[1].Split(';'),
                        OtmRelSelectedAttributes = string.IsNullOrEmpty(settings[2]) ? null : settings[2].Split(';'),
                        MtmRelSelectedAttributes = string.IsNullOrEmpty(settings[3]) ? null : settings[3].Split(';'),
                        PrivSelectedAttributes = string.IsNullOrEmpty(settings[4]) ? null : settings[4].Split(';'),
                        KeySelectedAttributes = string.IsNullOrEmpty(settings.Length == 6 ? settings[5] : null) ? null : settings[5].Split(';'),
                    };
                }
            }

            return new ListViewColumnsSettings();
        }

        public object Clone()
        {
            return new ListViewColumnsSettings
            {
                AttributeSelectedAttributes = AttributeSelectedAttributes == null ? null : (string[])AttributeSelectedAttributes.Clone(),
                EntitySelectedAttributes = EntitySelectedAttributes == null ? null : (string[])EntitySelectedAttributes.Clone(),
                MtmRelSelectedAttributes = MtmRelSelectedAttributes == null ? null : (string[])MtmRelSelectedAttributes.Clone(),
                OtmRelSelectedAttributes = OtmRelSelectedAttributes == null ? null : (string[])OtmRelSelectedAttributes.Clone(),
                PrivSelectedAttributes = PrivSelectedAttributes == null ? null : (string[])PrivSelectedAttributes.Clone(),
                KeySelectedAttributes = KeySelectedAttributes == null ? null : (string[])KeySelectedAttributes.Clone()
            };
        }

        public void SaveSettings()
        {
            var fileName = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "MscrmTools.MetadataBrowser.config");

            var settingsString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                EntitySelectedAttributes != null ? String.Join(";", EntitySelectedAttributes) : "",
                AttributeSelectedAttributes != null ? String.Join(";", AttributeSelectedAttributes) : "",
                OtmRelSelectedAttributes != null ? String.Join(";", OtmRelSelectedAttributes) : "",
                MtmRelSelectedAttributes != null ? String.Join(";", MtmRelSelectedAttributes) : "",
                PrivSelectedAttributes != null ? String.Join(";", PrivSelectedAttributes) : "",
                KeySelectedAttributes != null ? String.Join(";", KeySelectedAttributes) : "");

            using (var writer = new StreamWriter(fileName, false))
            {
                writer.Write(settingsString);
            }
        }
    }
}
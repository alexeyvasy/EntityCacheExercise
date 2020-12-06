using CsvHelper;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EntityCacheExercise
{
    class CSVRepoProvider : RepoProvider    // specific provider to write a csv file entries.
    {
        private string m_filepath;
        public PropertyInfo[] propertiesInfo { get; set; }
        private bool m_firstTimeAdding;

        public CSVRepoProvider(string filepath = @"c:\temp\test.csv")        // constructor for creating an empty file
        {
            m_firstTimeAdding = true;
            m_filepath = filepath;
            if (!File.Exists(m_filepath))
            {
                using (File.Create(m_filepath));
            }
        }

        public void Add(Dictionary<PropertyInfo, object> props)
        {
            if (m_firstTimeAdding == true)
            {
                propertiesInfo = props.Keys.ToArray();      // need to be performed only first time
                m_firstTimeAdding = false;
            }

            using (var writer = new StreamWriter(m_filepath, true))
            using (var csvWriter = new CsvWriter(writer))
            { 
                foreach (var field in props)
                {
                    csvWriter.WriteField(field.Value);
                }
                csvWriter.NextRecord(); 
            }
        }
        public void Update(Dictionary<PropertyInfo, object> props)
        {
            int id = (int)(long)props.First().Value;
            string tempFile = Path.GetTempFileName();

            using (StreamReader sr = new StreamReader(m_filepath))
            using (StreamWriter sw = new StreamWriter(tempFile))            
            using (var csvWriter = new CsvWriter(sw))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {

                    string[] properties = currentLine.Split(',');
                    int entryId;
                    bool idExists = int.TryParse(properties[0], out entryId);

                    if (entryId == id)
                    {
                        foreach (var field in props)
                        {
                            csvWriter.WriteField(field.Value);
                        }
                        csvWriter.NextRecord();
                    }
                    else
                    {
                        sw.WriteLine(currentLine);
                    }
                }          
            }

            File.Delete(m_filepath);
            File.Move(tempFile, m_filepath);
        }
        public void Remove(int id)
        {
            string tempFile = Path.GetTempFileName();

            using (StreamReader sr = new StreamReader(m_filepath))
            using (StreamWriter sw = new StreamWriter(tempFile))
            using (var csvWriter = new CsvWriter(sw))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {

                    string[] properties = currentLine.Split(',');
                    int entryId;
                    bool idExists = int.TryParse(properties[0], out entryId);

                    if (entryId == id)
                    {
                        continue;
                    }
                    else
                    {
                        sw.WriteLine(currentLine);
                    }
                }
            }

            File.Delete(m_filepath);
            File.Move(tempFile, m_filepath);
        }
        public Dictionary<PropertyInfo, object> Get(int id)
        {
            Dictionary<PropertyInfo, object> props = null;
            
            using (StreamReader sr = new StreamReader(m_filepath))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {

                    string[] properties = currentLine.Split(',');
                    int entryId;
                    bool idExists = int.TryParse(properties[0], out entryId);
                    
                    if (entryId == id)
                    {
                        props = new Dictionary<PropertyInfo, object>();

                        int i = 0;
                        foreach (PropertyInfo info in propertiesInfo)
                        {
                            props.Add(info, Convert.ChangeType(properties[i++], info.PropertyType));
                        }
                    }
                }
            }

            return props;
        }
        public Dictionary<int, Dictionary<PropertyInfo, object>> GetAll()
        {
            Dictionary<int, Dictionary<PropertyInfo, object>> repo = new Dictionary<int, Dictionary<PropertyInfo, object>>();

            Dictionary<PropertyInfo, object> props = null;

            using (StreamReader sr = new StreamReader(m_filepath))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    string[] properties = currentLine.Split(',');                    
                    props = new Dictionary<PropertyInfo, object>();
                    int i = 0;

                    foreach (PropertyInfo info in propertiesInfo)
                    {
                        props.Add(info, Convert.ChangeType(properties[i++], info.PropertyType));
                    }

                    repo.Add((int)(long)props.First().Value, props);
                }
            }

            return repo;
        }
    }
}

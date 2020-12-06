using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EntityCacheExercise
{
    interface RepoProvider
    {
        PropertyInfo[] propertiesInfo { get; set; }
        void Add(Dictionary<PropertyInfo, object> props);
        void Update(Dictionary<PropertyInfo, object> props);
        void Remove(int id);
        Dictionary<PropertyInfo, object> Get(int id);
        Dictionary<int, Dictionary<PropertyInfo, object>> GetAll();
    }
}

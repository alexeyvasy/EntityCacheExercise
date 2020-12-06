using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EntityCacheExercise
{
    class Adaptor 
    {
        public static Dictionary<PropertyInfo, object> Extract(Entity entity)
        {
            Dictionary<PropertyInfo, object> properties = new Dictionary<PropertyInfo, object>();

            PropertyInfo[] props = entity.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                properties.Add(prop, prop.GetValue(entity));
            }

            return properties;
        }

        public static Entity Fill(Dictionary<PropertyInfo, object> properties , Type typeOfEntity)
        {
            object entity = Activator.CreateInstance(typeOfEntity);
            PropertyInfo[] props = entity.GetType().GetProperties();


            foreach(PropertyInfo prop in props)
            {
                prop.SetValue(entity, properties[prop]);
            }

            return (Entity)Convert.ChangeType(entity, typeOfEntity);
        }
    }
}

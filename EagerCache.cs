using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EntityCacheExercise
{
    class EagerCache<T> : Cache<T> where T : Entity
    {
        public EagerCache(RepoProvider repo) : base(repo)
        {
            Dictionary<int, Dictionary<PropertyInfo, object>> db = repo.GetAll();

            foreach (KeyValuePair<int, Dictionary<PropertyInfo, object>> entity in db)
            {
                m_cache.Add(entity.Key, (T)Adaptor.Fill(entity.Value, typeof(T)));
            }
        }

        public override T Get(int id)
        {
            T entity;
            m_cache.TryGetValue(id, out entity);

            return entity;
        }
    }
}

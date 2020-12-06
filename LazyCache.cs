using System;
using System.Collections.Generic;
using System.Text;

namespace EntityCacheExercise
{
    class LazyCache<T> : Cache<T> where T : Entity
    {
        public LazyCache(RepoProvider repo) : base(repo)
        {
            
        }

        public override T Get(int id)
        {
            T entity;
            
            m_cache.TryGetValue(id, out entity);

            if (entity == null)
            {
                try
                {
                    var properties = m_repository.Get(id);

                    if (properties != null )
                    {
                        entity = (T)Adaptor.Fill(properties, typeof(T));
                        m_cache.Add(entity.getId(), entity);
                    }                    
                }
                catch (Exception e)
                {
                    throw e;                    
                }                
            }
            
            return entity;
        }

    }
}

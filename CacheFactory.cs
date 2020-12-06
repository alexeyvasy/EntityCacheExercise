using System;
using System.Collections.Generic;
using System.Text;

namespace EntityCacheExercise
{
    class CacheFactory <T> where T : Entity
    {
        public static Cache<T> GetCacheInstance(RepoProvider provider, CacheMode mode)  // factory method 
        { 
            Cache<T> instance = null;

            if (mode == CacheMode.eager)
            {
                instance = new EagerCache<T>(provider);
            }
            else 
            {
                instance = new LazyCache<T>(provider);
            }

            return instance;
        }
    }
}

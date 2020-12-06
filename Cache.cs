using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


// my base assumptions are:  
//
//
//                         1) if user trying to Add an entity of which Id already exists in the system, it will throw an Exception
//                         2) similarily if user triyng to update an entity which does not exists it also throws an Exception
//                         3) trying to Remove non existing entity gives Exception
//                         4) trying to Get non existing entity returns null, without exceptions.
//

namespace EntityCacheExercise
{
    enum CacheMode
    { 
        eager,
        lazy
    }

    enum ActionType
    { 
        Add,
        Update,
        Remove   
    }

    interface Observer
    {
        void update(ActionType action, Entity e);
    }

    interface Entity
    {
        int getId();
    }

    abstract class Cache<T> where T : Entity
    {
        protected Dictionary<int, T> m_cache;
        protected RepoProvider m_repository;
        private List<Observer> m_subscribers;

        public Cache(RepoProvider repository)
        {
            m_cache = new Dictionary<int, T>();
            m_subscribers = new List<Observer>();
            m_repository = repository;
            m_repository.propertiesInfo = typeof(T).GetProperties();
        }
        public void Subscribe(Observer observer)
        {
            // optionally checking first and some exception handling... 
            m_subscribers.Add(observer);
        }
        public void Unsubscribe(Observer observer)
        {
            m_subscribers.Remove(observer);
        }
        public void NotifyAll(ActionType action, T entity)
        {
            foreach (Observer observer in m_subscribers)
            {
                observer.update(action, entity);
            }
        }

        public abstract T Get(int id);
        public void Add(T entity)
        {
            if(Get(entity.getId()) == null && m_repository.Get(entity.getId()) == null) // check if not exists
            {
                Dictionary<PropertyInfo, object> props = Adaptor.Extract(entity);

                try
                {
                    m_repository.Add(props);
                }
                catch(Exception e)
                {
                    throw new Exception("DB couldnt add entity, please try again \n\n" + e.Message);
                }

                m_cache.Add(entity.getId(), entity);  
                NotifyAll(ActionType.Add, entity);
            }
            else
            {
                throw new Exception("There is already an Entity with this Id, try using 'Update' method instead");
            }
        }
        public void Update(T entity)
        {
            if (Get(entity.getId()) != null || m_repository.Get(entity.getId()) != null)
            {
                Dictionary<PropertyInfo, object> props = Adaptor.Extract(entity);

                try
                {
                    m_repository.Update(props);
                }
                catch (Exception e)
                {
                    throw new Exception("couldnt Update entity, please try again \n\n" + e.Message);
                }

                m_cache[entity.getId()] = entity; // updates the value with same key in the dictionary
                NotifyAll(ActionType.Update, entity);
            }
            else 
            {
                throw new Exception("There is no Entity with this Id, try using 'Add' method instead");
            }
        }
        public void Remove(int id)
        {
            T cachedEntity = Get(id);

            if (cachedEntity != null || m_repository.Get(id) != null)
            {
                try
                {
                    m_repository.Remove(id);
                }
                catch (Exception e)
                {
                    throw new Exception("couldnt Remove entity from file, please try again \n\n" + e.Message); ;
                }

                m_cache.Remove(id);
                NotifyAll(ActionType.Remove, cachedEntity);
            }
            else
            {
                throw new Exception("There is no Entity with this Id to Remove");
            }

        }
    }
}

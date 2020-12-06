using System;
using System.Collections.Generic;
using System.Text;

namespace EntityCacheExercise
{
    class Person : Entity
    {
        public long m_Id { get; set; }
        public string m_Firstname { get; set; }
        public string m_Lastname { get; set; }
        public double m_FavoriteNumber { get; set; }

        public Person() { }
        public Person(int id, string firstname, string lastname, double favnum)
        {
            m_FavoriteNumber = favnum;
            m_Firstname = firstname;
            m_Lastname = lastname;
            m_Id = id;
        }
        public int getId() { return (int)m_Id; }

        public override bool Equals(object obj)
        {
            Person p = (Person) obj;

            if (p.m_Id == m_Id && p.m_Firstname == m_Firstname && p.m_Lastname == m_Lastname && p.m_FavoriteNumber == m_FavoriteNumber)
                return true;
            else
                return false;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace EntityCacheExercise
{
    class Car : Entity
    {
        public long m_Id { get; set; }
        public string m_Modelname { get; set; }
        public string m_Color { get; set; }

        public Car() { }
        public Car(int id, string model, string color)
        {
            m_Modelname = model;
            m_Color = color;
            m_Id = id;
        }
        public int getId() { return (int)m_Id; }

        public override bool Equals(object obj)
        {
            Car p = (Car)obj;

            if (p.m_Id == m_Id && p.m_Color == m_Color && p.m_Modelname == m_Modelname)
                return true;
            else
                return false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Utils
{
    public class UtilEnum
    {
        /// <summary>
        /// Retorna el valor dentro de los posibles valores del Enum T dado que corresponde al valor "valor" dado
        /// Ejemplo: Si el valor de un enum es {A = 0, B = 5, C = 8}
        /// Se dice que el estado arranca en el valor dado al estado y termina en el estado siguiente (-1)
        /// Es decir que un el valor 3 retornará el estado A, el valor 5 retornará B.
        /// Este método es válido solo para enums que tengan valores crecientes. Otros enums darán un comportamiento extraño.
        /// </summary>
        /// <typeparam name="T">Tipo Enum donde están los estados</typeparam>
        /// <param name="valor">valor dentro de los estados</param>
        /// <returns>El valor entero al que corresponde el valor dado dentro del enum. Se debe castear al enum necesario T</returns>
        public static  T GetStateFromEnum<T>(int valor) where T:struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(T).IsEnum)
                throw new Exception("para usar el método GetStateFromEnum se debe especificar un tipo que sea enum");
            int lenghtEnum = Enum.GetNames(typeof(T)).Length;
            IEnumerable<T> values = Enum.GetValues(typeof(T)).Cast<T>();
            Array enteros = Enum.GetValues(typeof(T));

            IEnumerator<T> e = values.GetEnumerator();          

            for (int i = 0; i < lenghtEnum - 1; i++)
            {
                e.MoveNext();
                int valorEnum1 = (int)enteros.GetValue(i);
                int valorEnum2 = (int)enteros.GetValue(i+1);
                if (valorEnum1 > valorEnum2)
                    throw new Exception("Error en GEtStateFronEnum El tipo T es un tipo que no contiene una secuencia de valores creciente");
                if (valor >= valorEnum1 && valor < valorEnum2)
                {
                    return e.Current;
                }
            }


            return default(T);
        }
      
    }
}

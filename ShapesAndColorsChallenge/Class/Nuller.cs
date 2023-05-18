/***********************************************************************
* DESCRIPTION :
*
*
* NOTES :
* 
* 
* WARNINGS :
* 
* 
* OPTIMIZE IMPORTS : NO
* EXCEPTION CONTROL : NO
* DISPOSE CONTROL : STATIC
* 
*
* AUTHOR :
*
*
* CHANGES :
*
*
*/

using System;
using System.Collections.Generic;

namespace ShapesAndColorsChallenge.Class
{
    internal static class Nuller
    {
        #region METHODS

        /// <summary>
        /// Anula una lista de cualquier tipo.
        /// Si el tipo implementa IDisposable invocará Dispose.
        /// Si el tipo es nullable lo anulará.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        internal static void Null<T>(ref List<T> list)
        {
            if (list is null)
                return;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    continue;

                if (IsNullable(list[i]))
                    if (list[i] is IDisposable disposable)
                        disposable.Dispose();

                list[i] = default;/*Si es un tipo nullable lo anula, en caso contrario lo resetea a su valor por defecto*/
            }

            list.Clear();
            //list = null;/*Las colecciones no se anulan ya que pueden dar problemas al estar en un bucle de update o de draw*/
        }

        /// <summary>
        /// Anula el objeto pasado por referencia si es nullable, en caso contrario lo pone a su valor por defecto.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal static void Null<T>(ref T value)
        {
            if (value == null)
                return;

            if (IsNullable(value))
                if (value is IDisposable disposable)
                    disposable.Dispose();

            value = default;/*Si es un tipo nullable lo anula, en caso contrario lo resetea a su valor por defecto*/
        }

        /// <summary>
        /// Comprueba si un objeto es nullable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        static bool IsNullable<T>(T obj)
        {
            if (obj == null) 
                return true;

            Type type = typeof(T);

            if (!type.IsValueType) 
                return true; // Tipo por referencia

            if (Nullable.GetUnderlyingType(type) != null) 
                return true; // Nullable<T>
            
            return false; // Tipo por valor
        }

        #endregion
    }
}
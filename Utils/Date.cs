using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Utils
{
    public static class Date
    {
        /// <summary>
        /// Retorna String com a data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="withTime"></param>
        /// <returns></returns>
        public static string GetDate(DateTime data, bool withTime = false)
        {
            //if (data == null) return "";
            string ano = data.Year.ToString("0000");
            string mes = data.Month.ToString("00");
            string dia = data.Day.ToString("00");

            if (!withTime)
                return ano + "-" + mes + "-" + dia;

            string hora = data.Hour.ToString("00");
            string minuto = data.Minute.ToString("00");
            // não pega os segundos

            return ano + "-" + mes + "-" + dia + " " + hora + ":" + minuto + ":00";
        }
    }
}

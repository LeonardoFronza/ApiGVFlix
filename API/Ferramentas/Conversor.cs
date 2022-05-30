using System;

namespace API.Ferramentas
{
    public class Conversor : IConversor
    {
        #region String.

        public string ConverterTextoIniciaisParaMaiusculo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return string.Empty;
            }

            var textoSeparado = texto.Trim().Split(" ");
            for(int i = 0; i < textoSeparado.Length; i++)
            {
                textoSeparado[i] = textoSeparado[i].Substring(0, 1).ToUpper() + textoSeparado[i].Substring(1).ToLower();
            }

            var textoFormatado = string.Join(" ", textoSeparado);
            return textoFormatado;
        }

        public string ConverterTextoParaMinusculo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return string.Empty;
            }

            return texto.Trim().ToLower();
        }

        #endregion

        #region DateTime - DateTimeOffset.

        public string ConverterDateTimeParaDataString(DateTime dateTime)
        {
            var dia = dateTime.Day > 9
                ? Convert.ToString(dateTime.Day)
                : "0" + dateTime.Day;

            var mes = dateTime.Month > 9
                ? Convert.ToString(dateTime.Month)
                : "0" + dateTime.Month;

            return $"{dia}/{mes}/{dateTime.Year}";
        }

        public string ConverterDateTimeOffsetParaDataHoraString(DateTimeOffset dateTimeOffset)
        {
            var dateTime = ConverterDateTimeParaDataString(dateTimeOffset.DateTime);

            return $"{dateTime} {dateTimeOffset.Hour}:{dateTimeOffset.Minute}:{dateTimeOffset.Second}";
        }

        #endregion

        #region Enumerador.

        public string ConverterEnumeradorParaString(Type enumType, object value)
        {
            string descricaoEnumerador;

            try
            {
                descricaoEnumerador = Enum.GetName(enumType, value);
            }
            catch(Exception)
            {
                // Não gera erro.
                descricaoEnumerador = "<Falha na conversão>";
            }

            return descricaoEnumerador;
        }

        public bool ValidaExistenciaEnumerador(Type enumType, object value)
        {
            var existeEnumerador = Enum.IsDefined(enumType, value);
            return existeEnumerador;
        }

        #endregion
    }
}

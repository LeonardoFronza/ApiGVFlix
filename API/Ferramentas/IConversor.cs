using System;

namespace API.Ferramentas
{
    public interface IConversor
    {
        #region String.

        string ConverterTextoIniciaisParaMaiusculo(string texto);
        string ConverterTextoParaMinusculo(string texto);

        #endregion

        #region DateTime - DateTimeOffset.

        string ConverterDateTimeParaDataString(DateTime dateTime);
        string ConverterDateTimeOffsetParaDataHoraString(DateTimeOffset dateTimeOffset);

        #endregion

        #region Enumerador.

        string ConverterEnumeradorParaString(Type enumType, object value);
        bool ValidaExistenciaEnumerador(Type enumType, object value);

        #endregion
    }
}
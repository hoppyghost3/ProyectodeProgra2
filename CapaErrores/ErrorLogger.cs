using System;
using System.IO;

namespace CapaErrores
{
    public static class ErrorLogger
    {
        // Ruta donde se guardara el archivo de errores
        private static string rutaLog = "errores.log";

        // Metodo que recibe excepcion, clase y metodo donde ocurrio el error
        public static void RegistrarError(Exception ex, string nombreClase, string nombreMetodo)
        {
            try
            {
                // Construimos el mensaje con el formato solicitado
                string mensaje = $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] Clase: {nombreClase} - Metodo: {nombreMetodo} - Error: {ex.Message}";

                // Guardamos en archivo agregando una nueva linea
                File.AppendAllText(rutaLog, mensaje + Environment.NewLine);
            }
            catch
            {
                // En caso de fallo en la escritura del log, no propagamos el error
                // para evitar que la aplicacion se caiga solo por problemas del logger
            }
        }
    }
}

using System;
using System.IO;

namespace CapaNegocio
{
    public static class ErrorLogger
    {
        private static string rutaLog = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Logs",
            "errores.log"
        );

        public static void RegistrarError(Exception ex, string nombreClase, string nombreMetodo)
        {
            try
            {

                string directorio = Path.GetDirectoryName(rutaLog);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                string mensaje = $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] Clase: {nombreClase} - Método: {nombreMetodo} - Error: {ex.Message}";

                mensaje += $"{Environment.NewLine}StackTrace: {ex.StackTrace}{Environment.NewLine}";
                mensaje += new string('-', 100) + Environment.NewLine;

                File.AppendAllText(rutaLog, mensaje);
                Console.WriteLine($"Error registrado en: {Path.GetFullPath(rutaLog)}");
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Error al registrar log: {logEx.Message}");
            }
        }
    }
}
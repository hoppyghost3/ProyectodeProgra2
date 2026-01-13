using System;
using System.IO;

namespace CapaUtilidades  // O "CapaEntidad" si elegiste Opción B
{
    public static class ErrorLogger
    {
        // Ruta absoluta donde se guardará el log
        private static string rutaLog = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Logs",
            "errores.log"
        );

        public static void RegistrarError(Exception ex, string nombreClase, string nombreMetodo)
        {
            try
            {
                // Asegurarse de que la carpeta Logs existe
                string directorio = Path.GetDirectoryName(rutaLog);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                // Construir el mensaje
                string mensaje = $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] Clase: {nombreClase} - Método: {nombreMetodo} - Error: {ex.Message}";

                // Agregar stack trace para mejor debugging
                mensaje += $"{Environment.NewLine}StackTrace: {ex.StackTrace}{Environment.NewLine}";
                mensaje += new string('-', 100) + Environment.NewLine;

                // Guardar en archivo
                File.AppendAllText(rutaLog, mensaje);

                // TEMPORAL: Para debugging, mostrar dónde se guardó
                Console.WriteLine($"Error registrado en: {Path.GetFullPath(rutaLog)}");
            }
            catch (Exception logEx)
            {
                // Si falla el logger, al menos mostramos en consola
                Console.WriteLine($"Error al registrar log: {logEx.Message}");
            }
        }
    }
}
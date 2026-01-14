using System;
using System.Collections.Generic;
using System.Linq;
using CapaDatos;
using CapaEntidad;
using CapaUtilidades;

namespace CapaNegocio
{
    public class GestionAcademicaService
    {
        private InscripcionRepositorioJson inscripcionRepo;
        private CalificacionRepositorioJson calificacionRepo;
        private TareaRepositorioJson tareaRepo;
        private EntregaTareaRepositorioJson entregaRepo;
        private CursoRepositorioJson cursoRepo;
        private EstudianteRepositorioJson estudianteRepo;

        public GestionAcademicaService()
        {
            inscripcionRepo = new InscripcionRepositorioJson();
            calificacionRepo = new CalificacionRepositorioJson();
            tareaRepo = new TareaRepositorioJson();
            entregaRepo = new EntregaTareaRepositorioJson();
            cursoRepo = new CursoRepositorioJson();
            estudianteRepo = new EstudianteRepositorioJson();
        }

        // ============ INSCRIPCIONES ============
        public (bool exito, string mensaje) InscribirEstudianteACurso(int estudianteId, int cursoId)
        {
            try
            {
                // Verificar si ya está inscrito
                if (inscripcionRepo.ExisteInscripcion(estudianteId, cursoId))
                    return (false, "El estudiante ya está inscrito en este curso");

                // Obtener curso
                var curso = cursoRepo.BuscarPorId(cursoId);
                if (curso == null)
                    return (false, "Curso no encontrado");

                if (!curso.Activo)
                    return (false, "El curso no está activo");

                if (curso.CupoDisponible <= 0)
                    return (false, "No hay cupos disponibles");

                // Obtener estudiante
                var estudiante = estudianteRepo.BuscarPorId(estudianteId);
                if (estudiante == null)
                    return (false, "Estudiante no encontrado");

                // Crear inscripción
                var inscripcion = new Inscripcion
                {
                    EstudianteId = estudianteId,
                    NombreEstudiante = estudiante.NombreCompleto,
                    CursoId = cursoId,
                    NombreCurso = curso.Nombre,
                    FechaInscripcion = DateTime.Now,
                    Estado = "Activo"
                };

                inscripcionRepo.Agregar(inscripcion);

                // Actualizar cupo del curso
                curso.CupoDisponible--;
                if (!curso.EstudiantesInscritos.Contains(estudianteId))
                    curso.EstudiantesInscritos.Add(estudianteId);
                cursoRepo.Modificar(curso);

                // Actualizar cursos del estudiante
                if (!estudiante.CursosInscritos.Contains(cursoId))
                {
                    estudiante.CursosInscritos.Add(cursoId);
                    estudianteRepo.Modificar(estudiante);
                }

                return (true, "Inscripción realizada exitosamente");
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(GestionAcademicaService), nameof(InscribirEstudianteACurso));
                return (false, "Error al procesar la inscripción: " + ex.Message);
            }
        }

        public List<Inscripcion> ObtenerInscripcionesPorEstudiante(int estudianteId)
        {
            return inscripcionRepo.ObtenerPorEstudiante(estudianteId);
        }

        public List<Inscripcion> ObtenerInscripcionesPorCurso(int cursoId)
        {
            return inscripcionRepo.ObtenerPorCurso(cursoId);
        }

        // ============ CALIFICACIONES ============
        public (bool exito, string mensaje) RegistrarCalificacion(Calificacion calificacion)
        {
            try
            {
                if (calificacion == null)
                    return (false, "Calificación inválida");

                if (calificacion.Nota < 0 || calificacion.Nota > 100)
                    return (false, "La nota debe estar entre 0 y 100");

                // Verificar que el estudiante esté inscrito en el curso
                if (!inscripcionRepo.ExisteInscripcion(calificacion.EstudianteId, calificacion.CursoId))
                    return (false, "El estudiante no está inscrito en este curso");

                calificacionRepo.Agregar(calificacion);
                return (true, "Calificación registrada exitosamente");
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(GestionAcademicaService), nameof(RegistrarCalificacion));
                return (false, "Error al registrar calificación: " + ex.Message);
            }
        }

        public List<Calificacion> ObtenerCalificacionesPorEstudiante(int estudianteId)
        {
            return calificacionRepo.ObtenerPorEstudiante(estudianteId);
        }

        public List<Calificacion> ObtenerCalificacionesPorCurso(int cursoId)
        {
            return calificacionRepo.ObtenerPorCurso(cursoId);
        }

        public double ObtenerPromedioEstudiante(int estudianteId, int cursoId)
        {
            var calificaciones = calificacionRepo.ObtenerPorEstudiante(estudianteId)
                                                 .Where(c => c.CursoId == cursoId)
                                                 .ToList();

            if (calificaciones.Count == 0)
                return 0;

            return calificaciones.Average(c => c.Nota);
        }

        // ============ TAREAS ============
        public (bool exito, string mensaje) CrearTarea(Tarea tarea)
        {
            try
            {
                if (tarea == null)
                    return (false, "Tarea inválida");

                if (string.IsNullOrWhiteSpace(tarea.Titulo))
                    return (false, "El título es obligatorio");

                if (tarea.FechaEntrega <= DateTime.Now)
                    return (false, "La fecha de entrega debe ser futura");

                tareaRepo.Agregar(tarea);
                return (true, "Tarea creada exitosamente");
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(GestionAcademicaService), nameof(CrearTarea));
                return (false, "Error al crear tarea: " + ex.Message);
            }
        }

        public List<Tarea> ObtenerTareasPorCurso(int cursoId)
        {
            return tareaRepo.ObtenerPorCurso(cursoId);
        }

        public (bool exito, string mensaje) EntregarTarea(EntregaTarea entrega)
        {
            try
            {
                if (entrega == null)
                    return (false, "Entrega inválida");

                var tarea = tareaRepo.BuscarPorId(entrega.TareaId);
                if (tarea == null)
                    return (false, "Tarea no encontrada");

                entrega.EntregadaTarde = DateTime.Now > tarea.FechaEntrega;
                entregaRepo.Agregar(entrega);

                return (true, entrega.EntregadaTarde ?
                    "Tarea entregada con retraso" :
                    "Tarea entregada exitosamente");
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(GestionAcademicaService), nameof(EntregarTarea));
                return (false, "Error al entregar tarea: " + ex.Message);
            }
        }

        public List<EntregaTarea> ObtenerEntregasPorTarea(int tareaId)
        {
            return entregaRepo.ObtenerPorTarea(tareaId);
        }

        public List<EntregaTarea> ObtenerEntregasPorEstudiante(int estudianteId)
        {
            return entregaRepo.ObtenerPorEstudiante(estudianteId);
        }

        public (bool exito, string mensaje) CalificarTarea(int entregaId, double calificacion, string retroalimentacion)
        {
            try
            {
                if (calificacion < 0 || calificacion > 100)
                    return (false, "La calificación debe estar entre 0 y 100");

                entregaRepo.Calificar(entregaId, calificacion, retroalimentacion);
                return (true, "Tarea calificada exitosamente");
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(GestionAcademicaService), nameof(CalificarTarea));
                return (false, "Error al calificar tarea: " + ex.Message);
            }
        }
    }
}
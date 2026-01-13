using System.Collections.Generic;

namespace CapaDatos
{
    public interface IRepositorioEntidad<T>
    {
        void Agregar(T entidad);
        void Modificar(T entidad);
        void Eliminar(int id);
        List<T> Listar();
        T BuscarPorId(int id);
    }
}


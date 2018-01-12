using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using ADato;

namespace LN
{
    public class LNUsuario
    {
        public List<Usuario> Mapear(System.Data.Common.DbDataReader Datos)
        {
            List<Usuario> list = new List<Usuario>();


            while (Datos.Read())
            {
                Usuario item = new Usuario();

                item.id = Convert.ToInt32(Datos.GetValue(0));
                item.usuario = Convert.ToString(Datos.GetValue(1));
                item.contrasena = Convert.ToString(Datos.GetValue(2));

                list.Add(item);
            }
            return list;
        }

        public Usuario UsuarioId(Usuario Usuario)
        {
            Usuario grup = null;
            BaseDatos bd = new BaseDatos();
            bd.Conectar();
            bd.CrearComandoStrSql("select * from usuario where id=@id");
            bd.AsignarParametroInt("@id", Usuario.id);

            foreach (Usuario item in Mapear(bd.EjecutarConsulta()))
            {
                grup = item;

            }
            bd.Desconectar();

            return grup;
        }


        public List<Usuario> MostrarUsuario()
        {
            List<Usuario> list = new List<Usuario>();
            BaseDatos bd = new BaseDatos();
            bd.Conectar();
            bd.CrearComandoStrSql("select * from usuario");
            list = Mapear(bd.EjecutarConsulta());


            bd.Desconectar();
            return list;
        }

    }
}

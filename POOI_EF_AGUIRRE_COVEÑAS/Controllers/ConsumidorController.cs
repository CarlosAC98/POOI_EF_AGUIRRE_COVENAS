using POOI_EF_AGUIRRE_COVEÑAS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POOI_EF_AGUIRRE_COVEÑAS.Controllers
{
    public class ConsumidorController : Controller
    {
        string cadenaConexion = ConfigurationManager.ConnectionStrings["cadenaConexion"].ConnectionString;

        IEnumerable<Consumidor> listarConsumidores()
        {
            List<Consumidor> listaConsumidores = new List<Consumidor>();
            // 1. Crear la conexión
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            // 2. Aperturar la conexión
            sqlConnection.Open();
            // 3. Crear el comando indicando el procedure
            SqlCommand sqlCommand = new SqlCommand("usp_listar_consumidor", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            // 4. Ejecutar y recorrer el contenido
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                listaConsumidores.Add(new Consumidor(
                    sqlDataReader.GetString(0),
                    sqlDataReader.GetString(1),
                    sqlDataReader.GetString(2),
                    sqlDataReader.IsDBNull(3) ? "" : sqlDataReader.GetString(3),
                    sqlDataReader.GetInt32(4)
                    )
                    {
                        Nompais = sqlDataReader.GetString(5)
                    }
                    );
            }
            sqlDataReader.Close();
            sqlConnection.Close();
            return listaConsumidores;
        }

        IEnumerable<SelectListItem> listarPaises()
        {
            List<SelectListItem> listaPaises = new List<SelectListItem>();
            // 1. Crear la conexión
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            // 2. Aperturar la conexión
            sqlConnection.Open();
            // 3. Crear el query
            SqlCommand sqlCommand = new SqlCommand("select idpais, nompais from tb_paises", sqlConnection);
            // 4. Ejecutar y recorrer el contenido
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                listaPaises.Add(new SelectListItem()
                {
                    Value = sqlDataReader.GetInt32(0).ToString(),
                    Text = sqlDataReader.GetString(1)
                });
            }
            sqlDataReader.Close();
            sqlConnection.Close();
            return listaPaises;
        }

        string agregarConsumidor(Consumidor consumidor)
        {
            // 1. Crear la conexión
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            // 2. Aperturar la conexión
            sqlConnection.Open();
            // 3. Crear el comando indicando el procedure
            SqlCommand sqlCommand = new SqlCommand("usp_agrega_consumidor", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            // 4. Asignar valores a los parámetros de entrada
            sqlCommand.Parameters.AddWithValue("@dnicont", (object)consumidor.Dnicont ?? "");
            sqlCommand.Parameters.AddWithValue("@nomcont", (object)consumidor.Nomcont ?? "");
            sqlCommand.Parameters.AddWithValue("@apecont", (object)consumidor.Apecont ?? "");
            sqlCommand.Parameters.AddWithValue("@emailcont", (object)consumidor.Emailcont ?? "");
            sqlCommand.Parameters.AddWithValue("@idpais", consumidor.Idpais);
            // 5. Crear el parámetro de salida
            SqlParameter mensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 100);
            mensaje.Direction = ParameterDirection.Output;
            sqlCommand.Parameters.Add(mensaje);
            // 6. Ejecutar
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            // 7. Retornar el mensaje
            return mensaje.Value.ToString();
        }

        Consumidor buscarConsumidor(string dni)
        {
            Consumidor consumidor = null;
            // 1. Crear la conexión
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            // 2. Aperturar la conexión
            sqlConnection.Open();
            // 3. Crear el comando indicando el procedure
            SqlCommand sqlCommand = new SqlCommand("usp_buscar_consumidor", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            // 4. Ingresar el valor del parámetro
            sqlCommand.Parameters.AddWithValue("@dnicont", dni);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                consumidor = new Consumidor(
                    sqlDataReader.GetString(0),
                    sqlDataReader.GetString(1),
                    sqlDataReader.GetString(2),
                    sqlDataReader.IsDBNull(3) ? "" : sqlDataReader.GetString(3),
                    sqlDataReader.GetInt32(4)
                    )
                    {
                        Nompais = sqlDataReader.GetString(5)
                    };

            }
            sqlDataReader.Close();
            sqlConnection.Close();
            return consumidor;
        }

        string actualizarConsumidor(Consumidor consumidor)
        {
            // 1. Crear la conexión
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            // 2. Aperturar la conexión
            sqlConnection.Open();
            // 3. Crear el comando indicando el procedure
            SqlCommand sqlCommand = new SqlCommand("usp_actualizar_consumidor", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            // 4. Asignar valores a los parámetros de entrada
            sqlCommand.Parameters.AddWithValue("@dnicont", consumidor.Dnicont);
            sqlCommand.Parameters.AddWithValue("@nomcont", (object)consumidor.Nomcont ?? "");
            sqlCommand.Parameters.AddWithValue("@apecont", (object)consumidor.Apecont ?? "");
            sqlCommand.Parameters.AddWithValue("@emailcont", (object)consumidor.Emailcont ?? "");
            sqlCommand.Parameters.AddWithValue("@idpais", (object)consumidor.Idpais ?? "");
            // 5. Crear el parámetro de salida
            SqlParameter mensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 100);
            mensaje.Direction = ParameterDirection.Output;
            sqlCommand.Parameters.Add(mensaje);
            // 6. Ejecutar
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            // 7. Retornar el mensaje
            return mensaje.Value.ToString();
        }

        string eliminarConsumidor(string dni)
        {
            // 1. Crear la conexión
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            // 2. Aperturar la conexión
            sqlConnection.Open();
            // 3. Crear el comando indicando el procedure
            SqlCommand sqlCommand = new SqlCommand("usp_eliminar_consumidor", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            // 4. Asignar valor al parámetro de entrada
            sqlCommand.Parameters.AddWithValue("@dnicont", dni);
            // 5. Crear el parámetro de salida
            SqlParameter mensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 100);
            mensaje.Direction = ParameterDirection.Output;
            sqlCommand.Parameters.Add(mensaje);
            // 6. Ejecutar
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            // 7. Retornar el mensaje
            return mensaje.Value.ToString();
        }

        public ActionResult Index()
        {
            return View(listarConsumidores());
        }

        public ActionResult Create()
        {
            ViewBag.paises = listarPaises();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Consumidor consumidor)
        {
            // 1. Guardar en BD
            string mensaje = agregarConsumidor(consumidor);
            ViewBag.mensaje = mensaje;
            // 2. Volver a cargar la lista de países y mostrar el form
            ViewBag.paises = listarPaises();
            return View(consumidor);
        }

        public ActionResult Details(string id)
        {
            // 1. Recupera el consumidor por DNI
            Consumidor consumidorExistente = buscarConsumidor(id);
            // 2. Mandar hacia la vista
            return View(consumidorExistente);
        }

        // Actualizar
        public ActionResult Update(string id)
        {
            Consumidor consumidorExistente = buscarConsumidor(id);
            ViewBag.paises = listarPaises();
            return View(consumidorExistente);
        }

        [HttpPost]
        public ActionResult Update(Consumidor consumidor)
        {
            string mensaje = actualizarConsumidor(consumidor);
            ViewBag.mensaje = mensaje;
            ViewBag.paises = listarPaises();
            return View(consumidor);
        }

        // Eliminar
        public ActionResult Delete(string id)
        {
            Consumidor consumidorExistente = buscarConsumidor(id);
            return View(consumidorExistente);
        }

        [HttpPost]
        public ActionResult Delete(Consumidor consumidor)
        {
            string mensaje = eliminarConsumidor(consumidor.Dnicont);
            TempData["mensaje"] = mensaje;
            return RedirectToAction("Index");
        }
    }
}
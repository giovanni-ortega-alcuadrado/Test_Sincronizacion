using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace A2OYD_Servicios_API.ContextosDB
{
    public class ContextoDbOyd: DbContext
    {
        public ContextoDbOyd(DbContextOptions<ContextoDbOyd> opciones): base (opciones)
        {

        }

        #region GENERICOS
            public DbQuery<Entidades.Genericas.MensajeRespuesta> mensajerespuesta { get; set; }
            public DbQuery<Entidades.Genericas.ValorporDefecto> valorespordefecto { get; set; }
            public DbQuery<Entidades.Genericas.CodigoDescripcion> codigodescripcion { get; set; }
            public DbQuery<Entidades.Genericas.IntCodigoDescripcion> intodigodescripcion { get; set; }
        #endregion

        #region MAESTROS
        public DbQuery<Entidades.Maestros.Pais> paises { get; set; }
            public DbQuery<Entidades.Maestros.Ciudad> ciudades { get; set; }
            public DbQuery<Entidades.Maestros.Departamento> departamentos { get; set; }
        #endregion

        #region PERSONAS
            public DbQuery<Entidades.Personas.ClienteCuentaBanco> clientecuentasbancarias { get; set; }
            public DbQuery<Entidades.Personas.ClienteCodigoOyD> clientecodigosoyd { get; set; }
            public DbQuery<Entidades.Personas.ClienteDatosCuenta> clientesdatoscuentas { get; set; }
            public DbQuery<Entidades.Personas.ClienteEncargo> clienteencargos { get; set; }
        #endregion

        #region FINANCIERO
        public DbQuery<Entidades.Financiero.ClienteMovimientoCuenta> clientesmovimientoscuenta { get; set; }
        public DbQuery<Entidades.Financiero.ClienteTenencia> clientestenencia { get; set; }
        public DbQuery<Entidades.Financiero.ClienteSaldo> clientessaldo { get; set; }
        #endregion

        #region ORDENES
        public DbQuery<Entidades.Ordenes.GastoOrden> gastosorden { get; set; }
            public DbQuery<Entidades.Ordenes.EstadoOrden> estadosorden  { get; set; }
        #endregion

        #region FONDOS
            public DbQuery<Entidades.Fondos.EstadoOrdenFondos> estadoordenesfondos { get; set; }
        #endregion

        #region RENTABILIDAD
        public DbQuery<Entidades.Rentabilidad.Rentabilidad> rentabilidad { get; set; }
        #endregion  

    }
}

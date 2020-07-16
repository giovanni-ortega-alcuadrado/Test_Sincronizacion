using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2OYD_Servicios_API.Entidades.Fondos;

namespace A2OYD_Servicios_API.Models.DTO.Entidades.Fondos
{
    public class FondosFirmaDTO
    {
        /// <summary>
        /// ódigo del fondo (valor único)
        /// </summary>
        public int codigo { get; set; }

        /// <summary>
        /// descripción del fondo
        /// </summary>
        public string descripcion { get; set; }

        /// <summary>
        /// fecha de creacion del fondo
        /// </summary>
        public DateTime fechacreacion { get; set; }

        /// <summary>
        /// valor de la unidad
        /// </summary>
        public double? valorcuotaparte { get; set; }

        /// <summary>
        /// numero de unidades
        /// </summary>
        public double? cantidadcuptaparte { get; set; }

        /// <summary>
        /// Cantidad de dinero que tiene el fondo en su cartera
        /// </summary>
        public double? patrimonioneto { get; set; }

        /// <summary>
        /// rendimiento del fondo en lo que va del mes
        /// </summary>
        public double? performancemonthtoday { get; set; }

        /// <summary>
        /// rendimiento del fondo en lo que va se cuatrimestre (últimos 4 meses)
        /// </summary>
        public double? performancequartertoday { get; set; }

        /// <summary>
        /// rendimiento del fondo desde el 1 de enero a la fecha
        /// </summary>
        public double? performanceyeartodate { get; set; }

        /// <summary>
        /// rendimiento del fondo desde un año en el pasado hasta hoy
        /// </summary>
        public double? performancelastyear { get; set; }

        /// <summary>
        /// cantidad total de meses en los cuales el fondo tuvo rentabilidad positiva
        /// </summary>
        public int? mesespositivos { get; set; }

        /// <summary>
        /// cantidad total de meses en los cuales el fondo tuvo rentabilidad negativa
        /// </summary>
        public int? mesesnegativos { get; set; }

        /// <summary>
        /// porcentaje máximo de crecimiento del valor de la unidad
        /// </summary>
        public double? maximasuba { get; set; }

        /// <summary>
        /// porcentaje mínimo de crecimiento del valor de la unidad
        /// </summary>
        public double? maximabaja { get; set; }

        public double? volatilidaddiaria { get; set; }

        public double? volatilidadanualizada { get; set; }

        /// <summary>
        /// como esta compuesto el fondo
        /// </summary>
        public List<ComposicionFondo> composicionfondolista { get; set; }

        /// <summary>
        /// fecha de cierre del fondo
        /// </summary>
        public DateTime? fechavalorcuotaparte { get; set; }

        /// <summary>
        /// código de la moneda en la cual cotiza el fondo
        /// </summary>
        public string moneda { get; set; }

        /// <summary>
        /// porcentaje de variación del valor de la unidad entre la los últimos 2 días de cotización
        /// </summary>
        public double? porcentajevariacion { get; set; }
    }
}

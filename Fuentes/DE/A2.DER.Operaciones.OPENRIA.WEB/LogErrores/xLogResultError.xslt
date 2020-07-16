<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <head>
			<title>Alcuadrado S.A.</title>
		</head>
		<script language="javascript" src="tablefilter.js"></script>
<style type="text/css">
.cssTextoNormal
{ 
	font-family:Verdana;
	font-size:xx-small;
	color:Black;
}

.cssTextoHeader
{
	font-family:Verdana;
	font-size:xx-small;
	color:Black;
	 align:left;
}
</style>  
	<body onload="attachFilter(document.getElementById('grdData'), 1)">
    <font face="Verdana"><h4>Log Errores</h4></font>
    <table id="grdData" border="1" cellpadding="1" cellspacing="1" width="100%">
    <tr bgcolor="#eef3fb">
      <th class="cssTextoHeader">Fecha</th>
      <th class="cssTextoHeader">Hora</th>
      <th class="cssTextoHeader">Usuario</th>
      <th class="cssTextoHeader">Sistema</th>
      <th class="cssTextoHeader">Versión</th>
      <th class="cssTextoHeader">Módulo</th>
      <th class="cssTextoHeader">Función</th>
      <th class="cssTextoHeader">Tipo</th>
      <th class="cssTextoHeader">Mensaje</th>
      <th class="cssTextoHeader">Detalle</th>
    </tr>
    <xsl:for-each select="xml/Log">
    <tr>
      <td class="cssTextoNormal"><xsl:value-of select="Fecha"/></td>
      <td class="cssTextoNormal"><xsl:value-of select="Hora"/></td>
      <td class="cssTextoNormal"><xsl:value-of select="Usuario"/></td>
      <td class="cssTextoNormal"><xsl:value-of select="Sistema"/></td>
      <td class="cssTextoNormal"><xsl:value-of select="Version"/></td>
      <td class="cssTextoNormal"><xsl:value-of select="Modulo"/></td>
      <td class="cssTextoNormal"><xsl:value-of select="Funcion"/></td>
      <td class="cssTextoNormal"><xsl:value-of select="Tipo"/></td>
      <td class="cssTextoNormal"><xsl:value-of select="Mensaje"/></td>
      <td class="cssTextoNormal"><xsl:value-of select="Detalle"/></td>
    </tr>
    </xsl:for-each>
    </table>
  </body>
  </html>
</xsl:template>

</xsl:stylesheet>

  
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DatosVisorPage.aspx.vb" Inherits="A2.OyD.OYDServer.Services.DatosVisorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="jquery-1.8.3.js"></script>
	<script type="text/javascript">
	    $.extend($.expr[":"], {
	        containsExact: $.expr.createPseudo ?
          $.expr.createPseudo(function (text) {
              return function (elem) {
                  return $.trim(elem.innerHTML.toLowerCase()) === text.toLowerCase();
              };
          }) :
	                // support: jQuery <1.8
          function (elem, i, match) {
              return $.trim(elem.innerHTML.toLowerCase()) === match[3].toLowerCase();
          },

	                containsExactCase: $.expr.createPseudo ?
          $.expr.createPseudo(function (text) {
              return function (elem) {
                  return $.trim(elem.innerHTML) === text;
              };
          }) :
	                // support: jQuery <1.8
          function (elem, i, match) {
              return $.trim(elem.innerHTML) === match[3];
          },

	                containsRegex: $.expr.createPseudo ?
          $.expr.createPseudo(function (text) {
              var reg = /^\/((?:\\\/|[^\/]) )\/([mig]{0,3})$/.exec(text);
              return function (elem) {
                  return RegExp(reg[1], reg[2]).test($.trim(elem.innerHTML));
              };
          }) :
	                // support: jQuery <1.8
          function (elem, i, match) {
              var reg = /^\/((?:\\\/|[^\/]) )\/([mig]{0,3})$/.exec(match[3]);
              return RegExp(reg[1], reg[2]).test($.trim(elem.innerHTML));
          }

	    });

      function GetURLParameter(sParam) 
        {
            var sPageURL = window.location.search.substring(1).replace(/%C3%B3/g, "ó").replace(/%20/g, " ");

	        var sURLVariables = sPageURL.split('&');
	        for (var i = 0; i < sURLVariables.length; i++) 
            {
	            var sParameterName = sURLVariables[i].split('=');
	            if (sParameterName[0] == sParam) {
	                return sParameterName[1];
	            }
	        }
	    }

	    $(document).ready(function () {

	        var campos = GetURLParameter('cambios');

	        $('#cambios').html(campos);

	        if (campos) {
	            var cmbs = campos.split(";");

	            jQuery.each(cmbs, function (i, val) {

	                var searchterm = val;

	                if (searchterm.length > 3) {
	                    var match = $('span:containsExactCase("' + searchterm + '")').closest("tr");
	                    //match.addClass('selected');
	                    match.css({ color: "red", "font-weight": "bolder" });
	                    $('#cambios').html('Se presentaron cambios en los campos <br /><font color="blue">' + cmbs.join(', ') + '</font>');
	                } else {
	                    $('tr').css("display", "");
	                    $('tr').removeClass('selected');
	                    $('#cambios').html('');
	                }
	            });
	        }
	    });	
	</script>
	
	<style type="text/css">
	    .selected {
		    font-weight: bold;
		    color: red;
	    }	
	</style>

</head>
<body style="background-color: #EEEEEE;font-family:Calibri; font-size:12px" >
    <form id="form1" runat="server" style="float: left; top: 0px; right: 0px; bottom: 0px; left: 0px">
    <div>    
        <asp:GridView ID="gdVisor" runat="server"  AutoGenerateColumns="False" 
           ShowHeader="False" BorderStyle="None" BorderWidth="1px" CellSpacing="2" GridLines="None" DataSourceID="dsDatosVisor">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Label ID="lblNombre" runat="server" Text='<%# Bind("Nombre") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblDato" runat="server" Text='<%# Bind("Dato") %>'></asp:Label>                        
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#0000A9" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#000065" />
        </asp:GridView>
        <br />
        <span id="cambios"></span>
        <asp:SqlDataSource ID="dsDatosVisor" runat="server" 
            SelectCommand="[OYDPLUSOrdenes].[uspOyDNet_Ordenes_Obtener_Datos_Visor_Seteador]" 
            SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Name="pintIdOrdenes" Type="Int32"/>
            </SelectParameters>
        </asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>

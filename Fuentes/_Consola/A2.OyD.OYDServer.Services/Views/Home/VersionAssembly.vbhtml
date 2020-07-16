@ModelType IEnumerable(Of A2.OyD.OYDServer.Services.clsInformacionAssembly)

<div class="container">
    <div class="py-5 text-center">
        <img class="d-block mx-auto mb-4" src="~/Imagenes/Alcuadrado.png" alt="" width="40%">
        <h2>
            OYDServiciosRIA - Versión assemblys
        </h2>
        <p class="lead">
        <p>
            @Html.ActionLink("Retornar", "Index")
        </p>
        <table class="table text-center">
            <tr>
                <th class="text-left">
                    @Html.Raw("Nombre archivo")
                </th>
                <th class="text-left">
                    @Html.Raw("Versión")
                </th>
                <th></th>
            </tr>

            @For Each item In Model
                @<tr>
                    <td class="text-left">
                        @Html.DisplayFor(Function(modelItem) item.pstrNombreArchivo)
                    </td>
                    <td class="text-left">
                        @Html.DisplayFor(Function(modelItem) item.pstrVersionArchivo)
                    </td>
                </tr>
            Next

        </table>
        <p>
            @Html.ActionLink("Retornar", "Index")
        </p>
    </div>
    <div class="py-5 text-center">
        <a href="www.alcuadrado.com" target="_blank">Alcuadrado S.A.S</a>
    </div>
</div>

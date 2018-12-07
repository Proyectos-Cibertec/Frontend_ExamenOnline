app.controller("ComprarController", function ($scope, $http) {

    $scope.verPrincipal = true;
    $scope.verFactura = false;
    //$scope.detalleCompra = [];
    let detalleCompra = [];
    $scope.totalVenta = 0;
    $scope.compra = {};
    let lstDetalleCompra = {};

    $scope.obtenerCompra = function () {

        let validacion = true;
        $scope.detalleCompra = [];

        if ($("#cantiMes").val() < 0 || $("#cantiMes").val() == "") {
            alert("Ingrese cantidad de meses valido");
            validacion = false;
        }

        if ($("#tipSus").val() == "-1") {
            alert("Ingrese cantidad de meses valido");
            validacion = false;
        }

        if (validacion) {
            lstDetalleCompra = {
                "idSuscripcion": String($("#tipSus").val().split("|")[0]),
                "descSus": String($("#tipSus").val().split("|")[1]),
                "cantidadMeses": String($("#cantiMes").val()),
                "precioPorMes": String($("#tipSus").val().split("|")[2])
            }

            detalleCompra.push(lstDetalleCompra);
            $scope.totalVenta = parseFloat(Number(lstDetalleCompra.precioPorMes) * Number(lstDetalleCompra.cantidadMeses)).toFixed(2);
            $scope.compra = {
                "idUsuario": $("#codigoUsu").val(),
                "total": $scope.totalVenta,
                "lstDetalleCompra": detalleCompra
            }

            $scope.verPrincipal = false;
            $scope.verFactura = true;
        }

    }

    $scope.enviarCompra = function () {


        var parametros = {
            "idUsuario": String($scope.compra.idUsuario),
            "total": String($scope.compra.total),
            "lstDetalleCompra": detalleCompra
        }

        swal({
            title: 'Realizando la transacción!',
            text: 'Espere hasta que se complete la compra',
            onOpen: function () {
                swal.showLoading();
            }
        })

        $http({
            method: 'POST',
            url: url + "servicioExamen/RegistrarCompra",//'../Comprar/RegistrarCompra',
            data: JSON.stringify(parametros)
        }).then(function (response) {
            console.log("En la respuesta");
            console.log(response.data);
            if (response.data.mensaje == "OK") {
                swal('Buen trabajo!', 'Ha realizado la compra! Debe volver a logearse', 'success');
                setTimeout(function () {
                    document.getElementById("btnCerrarSesion").click();
                }, 3000);
            } else {
                swal({
                    type: 'error',
                    title: 'Error',
                    text: 'Ha ocurrido un error. Intentelo más tarde!',
                });

            }
            //window.location.href = "../Examen/ListarExamenes";

        }, function (error) {
            alert("ERROR: CONTACTE CON EL ADMINISTRADOR -> ERROR");
        });
    }

    $scope.comboSuscripciones = function () {

        $http.get(
            "../Suscripcion/lstSuscripciones"
        ).then(function (response) {
            if (response.data.length != 0) {
                $scope.suscripciones = response.data;
                console.log($scope.suscripciones);
                $scope.premium = $scope.suscripciones[0].precio;
                $scope.gold = $scope.suscripciones[1].precio;

            }

        }, function (error) {
            alert("Error");
        });

    }

    $scope.comboSuscripciones();

    $scope.cancelar = function () {
        $scope.detalleCompra = [];
        $scope.verPrincipal = true;
        $scope.verFactura = false;
    }

});
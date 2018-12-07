app.controller("ComprarController", function ($scope, $http) {

    $scope.verPrincipal = true;
    $scope.verFactura = false;
    $scope.detalleCompra = [];
    $scope.totalVenta = 0;
    let compra = {};
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
                IdSuscripcion: $("#tipSus").val().split("|")[0],
                descSus: $("#tipSus").val().split("|")[1],
                CantidadMeses: $("#cantiMes").val(),
                PrecioPorMes: $("#tipSus").val().split("|")[2]
            }

            $scope.detalleCompra.push(lstDetalleCompra);
            $scope.totalVenta = parseFloat(Number(lstDetalleCompra.PrecioPorMes) * Number(lstDetalleCompra.CantidadMeses)).toFixed(2);
            compra = {
                IdUsuario: $("#codigoUsu").val(),
                Total: $scope.totalVenta,
                'lstDetalleCompra': $scope.detalleCompra
            }

            $scope.verPrincipal = false;
            $scope.verFactura = true;
        }
        
    }

    $scope.enviarCompra = function () {

        swal({
            title: 'Realizando la transacción!',
            text: 'Espere hasta que se complete la compra',
            onOpen: function () {
                swal.showLoading();
            }
        })

        $http({
            method: 'POST',
            url: '../Comprar/RegistrarCompra',
            data: JSON.stringify(compra)
        }).then(function (response) {
            console.log(response);
            if (response) {
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
            alert("ERROR: CONTACTE CON EL ADMINISTRADOR");
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
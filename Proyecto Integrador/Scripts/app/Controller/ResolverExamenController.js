app.controller("ResolverExamenController", function ($scope, $http, $sce) {

    let IdExamen = $("#IdExamen").val();
    let idExamenRealizado = $("#idExamenRealizado").val();
    let ArrayPreUsuario = []; // Esto es un arreglo para las alternativas que marca el usuario
    let ArrayExamenRealDetalle = []; // Esto es un arreglo para las alternativas que marca el usuario - sin juntar las opciones ID
    let PregCorrectas = 0;
    let seconds = 0;
    let countdownTimer;
    $scope.validacionExamen = true;

    $scope.listaGeneral = function () {
        $http.get(
            url + "servicioExamen/ObtenerExamenResolver"
            + "?IdExamen=" + IdExamen + "&idExamenRealizado=" + idExamenRealizado
            //"../Examen/ObtenerExamenResolver"
            //+ "?IdExamen=" + IdExamen + "&idExamenRealizado=" + idExamenRealizado
        ).then(function (response) {
            console.log("SE OBTUVO DE LA BASE DE DATOS:");
            $scope.objExamenRealizado = response.data; // Obtiene el Examen Realizado
            $scope.objExamen = $scope.objExamenRealizado.examen;
            seconds = $scope.objExamen.tiempoRestante;

            console.log($scope.objExamenRealizado);

            if ($scope.objExamenRealizado.validaFechaExpiracion == 0) {
                console.log("Fecha");
                $scope.validacionExamen = false;
            }

            if ($scope.objExamenRealizado.estado == false) {
                console.log("Estado");
                $scope.validacionExamen = false;
            }

            // seconds = 60 * seconds;

            countdownTimer = setInterval($scope.secondPassed, 1000);
        }, function (error) {
            alert("Error al generar la lista");
        });
    };

    $scope.listaGeneral();

    $scope.renderHtml = function (htmlCode) {
        return $sce.trustAsHtml(htmlCode);
    };

    $scope.TerminarExamen = function () {

        let PreguntasTemporal = $scope.objExamen.lstPreguntas;
        let AlterArrayTem = [];

        PreguntasTemporal.forEach(function (elemento, index) {

            let objPreAlt = {};
            objPreAlt.numero = index + 1;

            elemento.lstAlternativa.forEach(function (AltElm, AltIndex) {

                if ($('#pregunta-' + elemento.numero + '-alternativa-' + AltElm.numero).is(':checked')) {
                    AlterArrayTem.push(AltElm.descripcion);

                    let ExamDetReal = {};
                    ExamDetReal.idPregunta = elemento.idPregunta;
                    ExamDetReal.idAlternativa = AltElm.idAlternativa;
                    ArrayExamenRealDetalle.push(ExamDetReal);
                }

            });

            objPreAlt.descripcion = AlterArrayTem.join(",");
            AlterArrayTem = [];
            ArrayPreUsuario.push(objPreAlt);
        });

        //Compara con alternativas correctas

        let objExamenRealizado = {
            usuario: { idUsuario: $("#idUsuario").val() },
            totalPreguntas: $scope.objExamen.lstPreguntas.length,
            'examen': { idExamen: IdExamen },
            'lstExamenRealizadoDetalle': ArrayExamenRealDetalle,
            'objAltMarcUsua': ArrayPreUsuario
        }

        console.log(objExamenRealizado);

        $http({
            method: 'POST',
            url: url + "servicioExamen/RegistrarExamenRealizado",//'../Examen/RegistrarExamenRealizado',
            data: JSON.stringify(objExamenRealizado)
        }).then(function (response) {
            console.log("************************************************");
            console.log(response.data);
            console.log(response.data.mensaje.split(",")[1]);

            if (response.data.mensaje.split(",")[0] == "OK") {
                window.location.href = "../Examen/ExamenRealizado?idExamenRealizado=" + response.data.mensaje.split(",")[1];
            } else {
                alert("ERROR: CONTACTE CON EL ADMINISTRADOR");
            }

        }, function (error) {
            alert("ERROR: CONTACTE CON EL ADMINISTRADOR");
        });

    }

    $scope.secondPassed = function () {
        var minutes = Math.round((seconds - 30) / 60);
        var remainigSecods = seconds % 60;
        if (remainigSecods < 10) {
            remainigSecods = "0" + remainigSecods;
        }

        document.getElementById("countdown").innerHTML = minutes + ":" + remainigSecods;

        if (seconds < 300) {
            $("#countdown").removeClass("text-blue").addClass("text-red");
        }

        if (seconds == 0) {
            clearInterval(countdownTimer);
            document.getElementById("countdown").innerHTML = "Termino Examen";
            $scope.TerminarExamen();
        } else {
            seconds--;
        }
    }
});
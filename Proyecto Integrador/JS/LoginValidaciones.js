$(document).ready(function () {

    $("#idIniciarSesion").click(function () {
        var usuario = $("#idusuario").val();
        var contraseña = $("#idclave").val();

        if (usuario == "") {
            $("#menErrorusu").fadeIn();
            return false;
        }
        else {
            $("#menErrorusu").fadeOut();

            if (contraseña == "") {
                $("#menErrorContra").fadeIn();
                return false;
            }
            else {
                $("#menErrorContra").fadeOut();
            }
        }
    });
});
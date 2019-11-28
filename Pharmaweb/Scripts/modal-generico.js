    $(function () {
        $.ajaxSetup({ cache: false });
        $("a[data-modal]").on("click", function (e) {
            $('#myModalContent').load(this.href, function () {
                $('#myModal').modal({
                    keyboard: true
                }, 'show');

                bindForm(this);
            });
            return false;
        });


    });

    function bindForm(dialog) {
        $('form', dialog).submit(function () {
            //if ($(this).attr("enctype") != 'multipart/form-data') {
                $('#progress').show();
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            var res = result.res;
                            //var ventasId = result.ventasId;

                            switch (res) {
                                case "reenviar":
                                    $('#myModal').modal('hide');
                                    $('#progress').hide();
                                    location.reload();
                                    //location.href = '/Cajas/Impresion/';
                                    //var UrlFactura = '@Url.Action("Impresion", "Cajas")';
                                    //var UrlFactura = '@Url.Action("Impresion", "Cajas", null, "http")';
                                    //window.location.href = UrlFactura;
                                    window.open("/Cajas/Impresion/");
                                    
                                    break
                                //case "#IndexEvalEco":
                                //    waitingDialog.show('Procesando...');
                                //    setTimeout(location.href = '/EvaluacionEco/IndexEvalAgropecuaria/', function () { waitingDialog.hide(); });
                                //    break
                                default:
                                    //$(div).load(result.url);
                                    $('#myModal').modal('hide');
                                    $('#progress').hide();
                                    //$(result.div).load(result.url);
                                    location.reload();
                            }                            
                        } else {
                            $('#myModal').modal('hide');
                            $('#progress').hide();
                            alert("Ocurrio un error: " + result.mensajeError + " Por favor comuniquese con el Administrador.");
                            //$('#myModalContent').html(result);
                            //bindForm();
                        }
                    }
                });
                return false;
            //}
        });
    }

    //$(function ImpresionFactura() {
    //    $.ajaxSetup({ cache: false });
    //    $("a[data-modal]").on("click", function (e) {
    //        $('#myModalContent').load(this.href, function () {
    //            $('#myModal').modal({
    //                keyboard: true
    //            }, 'show');

    //            bindForm(this);
    //        });
    //        return false;
    //    });


    //});
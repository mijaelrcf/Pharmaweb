var mywindow = null;
function printPage() {
    //Print Page
    var divToPrint = document.getElementById('content_main');

    PrintElem(divToPrint.innerHTML);
}

function PrintElem(elem) {
    //Popup($('<div/>').append($(elem).clone()).html());
    Popup($('<div/>').html($(elem).clone()).html());
}

function Popup(data) {
    var head = document.getElementsByTagName('head');
    if (mywindow != null) {
        mywindow.close();

    }
    mywindow = window.open('', 'Factura', 'height=600,width=1000');
    mywindow.location = 'about:blank';
    mywindow.document.write('<html class="dxChrome dxWindowsPlatform dxWebKitFamily dxBrowserVersion-41" style="padding-top: 2px; padding-left: 2px;">' + head[0].innerHTML + '<body class="dx-adaptive-phone dx-adaptive-landscape dx-adaptive">');
    mywindow.document.write('<link href="../Content/print.css" rel="stylesheet" type="text/css" />');
    mywindow.document.write(data);
    mywindow.document.write('</body>');
    mywindow.document.write('</html>');
    mywindow.document.write("<script type='text/javascript'>  window.print(); window.close(); </script>");
    return true;
}





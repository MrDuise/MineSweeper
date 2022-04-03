$(function () {
    console.log("Page is ready");

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
        console.log("Right click. Prevent context menu from showing.")
    });

    $(document).on("mousedown", ".game-button", function (event) {
        switch (event.which) {
            case 1:
                
                event.preventDefault();
                var buttonNumber = $(this).val();
                //turn the first and 3rd element of the array into variables then convert those to intergers
                let r = buttonNumber[0];
                let c = buttonNumber[2];
                r = Number(r);
                c = Number(c);
                
                console.log("Button number " + buttonNumber + " was left clicked");
                doButtonUpdate(r,c, "/GameBoard/HandleButtonClick");
                
                break;
            case 2:
                break;
            case 3:
                event.preventDefault();
                var buttonNumber = $(this).val();
                //turn the first and 3rd element of the array into variables then convert those to intergers
                let row = buttonNumber[0];
                let col = buttonNumber[2];
                row = Number(row);
                col = Number(col);
               
               
                console.log("Button number " + buttonNumber + " was right clicked");
                doButtonUpdate(row, col, "/GameBoard/RightClickShowButton");
                break;
            default:
                alert('Nothing');
        }
    });

    function doButtonUpdate(row,col, urlString) {
        $.ajax({
            datatype: "json",
            method: 'POST',
            url: urlString,
            data: {
                "row": row,
                "col": col,
            },
            success: function (data) {
                console.log(data);
                $(".button-zone").html(data);
            }
        });
    }
});
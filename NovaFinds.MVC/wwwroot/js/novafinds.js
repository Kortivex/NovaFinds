// JQuery ready Start
$(document).ready(function () {
    // Elements to zoom product photos
    $('.xzoom, .xzoom-gallery').xzoom({zoomWidth: 280, title: true, tint: '#333', Xoffset: 15, scroll: true});
    let isTouchSupported = 'ontouchstart' in window;
    if (isTouchSupported) {
        // If is an touch device
        $('.xzoom').each(function () {
            let xzoom = $(this).data('xzoom');
            xzoom.eventunbind();
        });

        $('.xzoom').each(function () {
            let xzoom = $(this).data('xzoom');
            $(this).hammer().on("tap", function (event) {
                event.pageX = event.gesture.center.pageX;
                event.pageY = event.gesture.center.pageY;

                xzoom.eventmove = function (element) {
                    element.hammer().on('drag', function (event) {
                        event.pageX = event.gesture.center.pageX;
                        event.pageY = event.gesture.center.pageY;
                        xzoom.movezoom(event);
                        event.gesture.preventDefault();
                    });
                }

                xzoom.eventleave = function (element) {
                    element.hammer().on('tap', function (event) {
                        xzoom.closezoom();
                    });
                }
                xzoom.openzoom(event);
            });
        });
    }

    // Prevent closing from click inside dropdown
    $(document).on("click",
        ".dropdown-menu",
        function (e) {
            e.stopPropagation();
        });

    // Bootstrap Tooltip
    if ($('[data-toggle="tooltip"]').length > 0) { // check if element exists
        $('[data-toggle="tooltip"]').tooltip();
    }

    // Redirect when Filter option change
    $('#sorterMenu').change(function () {
        window.location = $(this).val();
    });

    // Elements to control the add and remove products to cart
    let quantity = $('#quantity');
    let productId = $('#productId');
    let productAvailability = $('#productAvailability');

    $('#button-plus').click(function () {
        let availability = parseInt(productAvailability.val());
        if (availability > 0) {
            quantity.val(parseInt(quantity.val()) + 1);
            $('#productAvailability').val(--availability);
        }
    });

    $('#button-minus').click(function () {
        let availability = parseInt(productAvailability.val());
        if (availability >= 0 && parseInt(quantity.val()) > 0) {
            quantity.val(parseInt(quantity.val()) - 1);
            $('#productAvailability').val(++availability);
        }
    });

    // To Add a Product to Cart
    $('#addItem').click(function () {
        if (parseInt(quantity.val()) > 0) {
            $.ajax({
                url: '/Cart/AddItem',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data.response === "OK") {
                        Toastify({
                            text: "The item had been added!",
                            gravity: "top",
                            position: "right",
                            stopOnFocus: true,
                            close: false,
                            backgroundColor: "#ff6600",
                            duration: 2500
                        }).showToast();
                    }
                },
                error: function (jqXhr) {
                    if (jqXhr.status > 400) {
                        Toastify({
                            text: "Sorry, the item can't be added!",
                            gravity: "top",
                            position: "right",
                            stopOnFocus: true,
                            close: false,
                            backgroundColor: "#ff6600",
                            duration: 3000
                        }).showToast();
                    }
                },
                data: JSON.stringify({
                    "Quantity": parseInt(quantity.val()),
                    "ProductId": parseInt(productId.val())
                })
            });
        }
    });

    // To Remove a Product from Cart
    $('.removed').click(function () {
        let id = $(this).attr('id').split("_");
        $.ajax({
            url: '/Cart/RemoveItem',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                location.reload();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                ;
                console.log(textStatus);
            },
            data: JSON.stringify({
                "Quantity": parseInt(id[1]),
                "ProductId": parseInt(id[0])
            })
        });
    });

    // To Buy Products
    $('#buyProducts').click(function () {
        let id = $(this).attr('id').split("_");
        $.ajax({
            url: '/Tpv/Buy',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                if (data.result === "OK") {
                    Toastify({
                        text: "You had buy all successfully!",
                        gravity: "top",
                        position: "right",
                        stopOnFocus: true,
                        close: false,
                        backgroundColor: "#ff6600",
                        duration: 2500
                    }).showToast();

                    setTimeout(function () {
                        location.replace("/Identity/Account/Manage/Orders");
                    }, 3000);
                }
            },
            error: function (jqXhr) {
                if (jqXhr.status >= 400) {
                    Toastify({
                        text: jqXhr.responseJSON.value.result,
                        gravity: "top",
                        position: "right",
                        stopOnFocus: true,
                        close: false,
                        backgroundColor: "#ff6600",
                        duration: 3000
                    }).showToast();
                }
            },
            data: JSON.stringify({
                "Email": $('#Input_Email').val(),
                "StreetAddress": $('#Input_StreetAddress').val(),
                "CardNumber": parseInt($('#Input_CardNumber').val()),
                "ExpMonthDate": parseInt($('#Input_ExpMonthDate').val()),
                "ExpYearDate": parseInt($('#Input_ExpYearDate').val())
            })
        });
    });

    // Event to Search a Product
    let searchBox = $("#search-box");
    searchBox.keyup(function (key) {
        // Escape Key
        if (key.keyCode !== 27 && ($(this).val() !== "" || $(this).val().length !== 0)) {
            productSearch($(this));
        } else {
            hideSuggestionBox();
        }
    }).focusout(function () {
        hideSuggestionBox();
    });
});

// Function to Search a Product
function productSearch(ths) {
    let suggestionBox = $("#suggestion-box");
    let searchBox = $("#search-box");
    $.ajax({
        type: "GET",
        url: "/Search/Product",
        data: "product=" + ths.val(),
        beforeSend: function () {
            searchBox.css("background", "#fff0e5");
        },
        success: function (data) {
            if (data.length > 0) {
                showSuggestionBox();
                let list = "";
                data.forEach(function (product) {
                    list += "<div class=\"autocomplete-suggestion\" onClick='selectProduct(\"" +
                        product.name +
                        "\", \"" +
                        product.id +
                        "\");'>" +
                        product.name +
                        "</div>";
                });
                suggestionBox.html(list);
            }
            searchBox.css("background", "#FFF");
        }
    });
}

// To select Product Name
function selectProduct(name, id) {
    $("#search-box").val(name);
    window.location = "/Product/" + id + "/Show";
}

// To Hide/Show Suggestion Box
function hideSuggestionBox() {
    let suggestionBox = $("#suggestion-box");
    suggestionBox.fadeOut();
    suggestionBox.css("border", "none");
}

// To Hide/Show Suggestion Box
function showSuggestionBox() {
    let suggestionBox = $("#suggestion-box");
    suggestionBox.fadeIn();
    suggestionBox.css("border", "1px solid #999");
}
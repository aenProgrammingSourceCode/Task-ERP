function showpage(PageNumber, totalPages) {
    const pagesToShow = 6; // Number of pages to show at once
    $('.paging .page-link').addClass("hidden");

    if (PageNumber === 'previous') {
        // Show every 4 previous pages when "Previous" is clicked
        const currentPage = $('.paging .page-link:not(.hidden)').first().index();
        const startPage = Math.max(0, currentPage - (pagesToShow * 4)); // Adjust the multiplier to show 4 pages
        const endPage = Math.min(totalPages, startPage + pagesToShow);
        for (let i = startPage; i < endPage; i++) {
            $('.paging .page-link').eq(i).removeClass("hidden");
        }
    } else if (PageNumber === 'next') {
        // Show every 4 next pages when "Next" is clicked
        const currentPage = $('.paging .page-link:not(.hidden)').last().index();
        const startPage = Math.min(totalPages, currentPage + 1);
        const endPage = Math.min(totalPages, startPage + (pagesToShow * 4)); // Adjust the multiplier to show 4 pages
        for (let i = startPage; i < endPage; i++) {
            $('.paging .page-link').eq(i).removeClass("hidden");
        }
    } else {
        // Show the appropriate range around the selected page
        const pageNumber = PageNumber;
        const startPage = Math.max(0, Math.min(pageNumber - Math.floor(pagesToShow / 2), totalPages - pagesToShow));
        const endPage = Math.min(totalPages, startPage + pagesToShow);
        for (let i = startPage; i < endPage; i++) {
            $('.paging .page-link').eq(i).removeClass("hidden");
        }
    }
}

function paginationCallBack(totalPages, currentPage) {
    const pageSize = 6;

    const pageLinksContainer = $('.paging');
    pageLinksContainer.empty();

    for (let i = 1; i <= totalPages; i++) {
        (function (pageNumber) {
            const link = $('<a>').attr({ 'href': '#', 'class': 'page-link' }).text(pageNumber);

            if (pageNumber === currentPage) {
                link.addClass("active");
            }

            link.on('click', function () {
                $('.page-link').removeClass('active'); // Remove active class from all links
                $(this).addClass('active'); // Set active class on the clicked link

                getSalesOrders(pageNumber, pageSize); // Trigger your function with the page number
            });

            pageLinksContainer.append(link);
        })(i);
    }
}

function paginationCallBackForInvoiceList(totalPages, currentPage) {
    const pageSize = 6;

    const pageLinksContainer = $('.paging');
    pageLinksContainer.empty();

    for (let i = 1; i <= totalPages; i++) {
        (function (pageNumber) {
            const link = $('<a>').attr({ 'href': '#', 'class': 'page-link' }).text(pageNumber);

            if (pageNumber === currentPage) {
                link.addClass("active");
            }

            link.on('click', function () {
                $('.page-link').removeClass('active'); // Remove active class from all links
                $(this).addClass('active'); // Set active class on the clicked link

               getInvoices(pageNumber, pageSize); // Trigger your function with the page number
            });

            pageLinksContainer.append(link);
        })(i);
    }
}
function paginationCallBackForDeliveryList(totalPages, currentPage) {
    const pageSize = 6;

    const pageLinksContainer = $('.paging');
    pageLinksContainer.empty();

    for (let i = 1; i <= totalPages; i++) {
        (function (pageNumber) {
            const link = $('<a>').attr({ 'href': '#', 'class': 'page-link' }).text(pageNumber);

            if (pageNumber === currentPage) {
                link.addClass("active");
            }

            link.on('click', function () {
                $('.page-link').removeClass('active'); // Remove active class from all links
                $(this).addClass('active'); // Set active class on the clicked link

                getInvoices(pageNumber, pageSize); // Trigger your function with the page number
            });

            pageLinksContainer.append(link);
        })(i);
    }
}
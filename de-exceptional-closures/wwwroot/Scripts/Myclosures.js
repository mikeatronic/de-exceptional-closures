$(document).ready(function () {
    $('#MyClosuresTable').DataTable(
        {
            "ordering": true,
            "columnDefs": [{
                "targets": 7,
                "orderable": false,
            }],
            aaSorting: [],
            bInfo: true,
            paging: true,
            pageLength: 10,
            bLengthChange: true,
            bPaginate: true,
            bFilter: true,
            bSortable: true,
            order: [[2, "desc"]],
            language: { search: "", searchPlaceholder: "Search" },
            drawCallback: function () {

            }
        }
    );

    $('#MyClosuresTable input').addClass('form-control');
    $('#MyClosuresTable_wrapper input').unwrap();
    $('#MyClosuresTable_wrapper input').attr("aria-label", "search");
    $('#MyClosuresTable_filter').css("padding-bottom", "5px");
    $('#MyClosuresTable_filter label').addClass('hide-text');
    $('.disabled').attr('aria-disabled', true);
    $('.dataTables_filter input').attr('type', 'text');
});
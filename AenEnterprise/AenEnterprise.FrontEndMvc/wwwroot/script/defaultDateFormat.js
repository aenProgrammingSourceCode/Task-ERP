
// set default date for startup date
function setDefaultDates() {
    const dateFrom = new Date('2000-01-01').toISOString().split('T')[0];
    const dateTo = new Date('2030-12-31').toISOString().split('T')[0];
    $('#dateFromInput').val(dateFrom);
    $('#dateToInput').val(dateTo);
}
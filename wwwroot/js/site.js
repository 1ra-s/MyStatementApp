$(document).ready(function () {
    $('#generateForm').submit(function (event) {
        event.preventDefault();
        
        var accountId = $('#accountId').val();

        $.ajax({
            url: '/Statements/Generate', // Adjust URL based on your backend routing
            method: 'POST',
            data: { accountId: accountId },
            success: function (data) {
                $('#statementContent .card-body').html('<p>' + data.statementContent + '</p>');
                $('#statementContent').slideDown();
                $('#downloadLink').attr('href', data.downloadUrl).removeClass('d-none').fadeIn();
            },
            error: function (error) {
                console.error('Error generating statement:', error);
                $('#statementContent .card-body').html('<p>Error generating statement. Please try again.</p>');
                $('#statementContent').slideDown();
                $('#downloadLink').addClass('d-none').fadeOut();
            }
        });
    });
});

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function showLoader() {
    $('.loader-container').addClass('loader-show').removeClass('loader-hide');
}

function hideLoader() {
    $('.loader-container').addClass('loader-hide').removeClass('loader-show');
}
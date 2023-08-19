// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    console.log("Site conneted to js");

    loadVillas();
    $('#villa-form').submit(function (event) {
        event.preventDefault(); // Prevent the default form submission behavior
        console.log("The submit was hit");

        // Create a JSON object with form data
        var villaData = {
            Name: $('#add-name').val(),
            Sqft: $('#add-sqft').val(),
            Occupancy: $('#add-occupancy').val()
        };

        // Send a POST request to CreateVilla action using AJAX
        $.ajax({
            type: 'POST',
            url: createVillaUrl,
            contentType: 'application/json',
            data: JSON.stringify(villaData),
            success: function (data) {
                if (data.success) {
                    // If successful, refresh the villa list
                    $('#villasTableBody').load('/VIlla/VillaListPartial');
                    // Clear form fields
                    $('#add-name').val('');
                    $('#add-sqft').val('');
                    $('#add-occupancy').val('');
                } else {
                    alert('An error occurred while adding the villa.');
                }
            },
            error: function () {
                alert('An error occurred while adding the villa.');
            }
        });
    });

    //select all and attaches the click event on the .delete-btn
    $(document).on('click', '.delete-btn', function () {
        console.log('U click the delete button')
        var villaId = $(this).data('id');
        deleteVilla(villaId);
    });

    function deleteVilla(id) {
        $.ajax({
            type: 'DELETE',
            url: '/Villa/DeleteVilla/' + id,
            success: function (data) {
                if (!data.success) {
                    alert('An error cause by data.');
                }
                loadVillas();
            },
            error: function () {
                alert("Error loading villa data.")
            }
        });
    };

    //Next for Edit form

    $(document).on('click', '.edit-btn', function () {
        console.log("You triggered the edit button");
        var villaId = $(this).data('id');
        displayEditForm(villaId);
        document.getElementById('editForm').style.display = 'block';
    });
    function displayEditForm(id) {
        var editForm = $('#editForm');

        $.ajax({
            type: 'GET',
            url: '/Villa/GetVillaById/' + id,
            dataType: 'json',
            success: function (villa) {
                console.log('Received villa data:', villa);
                if (villa) {
                    $('#edit-id').val(villa.id);
                    $('#edit-name').val(villa.name);
                    $('#edit-sqft').val(villa.sqft);
                    $('#edit-occupancy').val(villa.occupancy);

                    editForm.show();
                }
            },
            error: function () {
                alert('Error fetching villa data.');
            }
        });
    }


    //Next for save form button
    $('#editForm form').submit(function (event) {
        event.preventDefault(); // Prevent the default form submission behavior
        var villaId = $('#edit-id').val(); // Get the villa ID from the hidden input
        saveVilla(villaId);
    });
    $(document).on('click', '.save-btn', function () {
        var villaId = $(this).data('id');
        saveVilla(villaId);
    });

    function saveVilla(id) {
        var villaData = {
            Id: id,
            Name: $('#edit-name').val(),
            Sqft: $('#edit-sqft').val(),
            Occupancy: $('#edit-occupancy').val()
        };

        $.ajax({
            type: 'PUT',
            url: '/Villa/UpdateVilla/' + id,
            contentType: 'application/json',
            data: JSON.stringify(villaData),
            success: function (data) {
                if (!data.success) {
                    // Update the UI accordingly (e.g., hide the edit form)
                    alert(data.message);
                    
                    ; // Refresh the villa list
                }
                console.log('You got new data', data);
                $('#editForm').hide();
                loadVillas()
            },
            error: function () {
                alert("Error updating villa data.");
            }
        });
    }

});

function loadVillas() {
    $.ajax({
        type: "GET",
        url: getVillasUrl,
        success: function (data) {
            var tbody = $("#villasTableBody");
            tbody.empty();

            $.each(data, function (index, villa) {
                var row = "<tr>" +
                    "<td>" + villa.id + "</td>" +
                    "<td>" + villa.name + "</td>" +
                    "<td>" + villa.sqft + "</td>" +
                    "<td>" + villa.occupancy + "</td>" +
                    "<td><button class='btn btn-primary edit-btn' data-id='" + villa.id + "'>Edit</button></td>" +
                    "<td><button class='btn btn-danger delete-btn' data-id='" + villa.id + "'>Delete</button></td>" +
                    "</tr>";
                tbody.append(row);
            });
        },
        error: function () {
            alert("Error loading villa data.")
        }
    });
}



const CreateCategory = () => {
    $.get("/Categories/Create/", (result) => {
        $("#myModal").modal();
        $("#myModalLabel").html("");
        $("#myModalBody").html(result);
    })
}

const DeleteCategory = (id) => {
    $.ajax({
        url: "/Categories/Delete/",
        data: { id }
    }).done((res) => {
        $("#myModal").modal();
        $("#myModalLabel").html("حذف دسته‌بندی");
        $("#myModalBody").html(res);
    });
}

const DeleteBook = (id) => {
    $.ajax({
        url: "/Books/DeleteBook/",
        data: { id }
    }).done((res) => {
        $("#myModal").modal();
        $("#myModalLabel").html("حذف کتاب");
        $("#myModalBody").html(res);
    })
}

const AddRole = () => {
    $.get("/ManageRoles/AddRole/", (result) => {
        $("#myModal").modal();
        $("#myModalLabel").html("");
        $("#myModalBody").html(result);
    })
}

const EditRole = (id) => {
    $.get("/ManageRoles/EditRole/" + id, (result) => {
        $("#myModal").modal();
        $("#myModalLabel").html("");
        $("#myModalBody").html(result);
    })
}

const AddUserToRole = (id) => {
    $.get("/ManageUsers/AddUserToRole/" + id, (result) => {
        $("#myModal").modal();
        $("#myModalLabel").html("");
        $("#myModalBody").html(result);
    })
}

const RemoveUserFromRole = (id) => {
    $.get("/ManageUsers/RemoveUserFromRole/" + id, (result) => {
        $("#myModal").modal();
        $("#myModalLabel").html("");
        $("#myModalBody").html(result);
    })
}
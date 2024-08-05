
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
    console.log(id)
    $.ajax({
        url: "/Books/DeleteBook/",
        data: { id }
    }).done((res) => {
        $("#myModal").modal();
        $("#myModalLabel").html("حذف کتاب");
        $("#myModalBody").html(res);
    })
}

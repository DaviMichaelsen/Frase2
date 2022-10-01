function ChangeMessageFormValue(valueId)
{
    alert("Mensagem Excluida, Atualize a página!");
    document.getElementById("MessageToDelete").value = valueId;
    let sbmtForm = document.getElementById("MessageForm");
    sbmtForm.submit();
}

function OpenUserProfile(id) {
    document.getElementById("ProfileToGo").value = id;
    if (document.getElementById("ProfileToGo").value != null && document.getElementById("ProfileToGo").value != "") {
        var frm = document.getElementById("UserProfile");
        frm.submit();
    }
}
function ChangeMessageFormValue(valueId)
{
    alert("Mensagem Excluida, Atualize a página!");
    document.getElementById("MessageToDelete").value = valueId;
    let sbmtForm = document.getElementById("MessageForm");
    sbmtForm.submit();
}
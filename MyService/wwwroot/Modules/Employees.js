$(document).ready(function () {
    $('#employeesId').DataTable({
        "autoWidth": false,
        "responsive": true,
        "language": {
            "emptyTable": "لايوجد موظفين متاحين"
        }
    });
});

EditEmployee = (id, name, email, phone, date,active, statue, resume) => {
    // تحديث الحقول في النموذج
    document.getElementById("EmployeeId").value = id;
    document.getElementById("Name").value = name;
    document.getElementById("Email").value = email;
    document.getElementById("Phone").value = phone;

    // التأكد من تنسيق التاريخ
    let formattedDate = new Date(date).toISOString().split("T")[0];
    document.getElementById("DateCreate").value = formattedDate;

    // تحديث الحقول الأخرى
    // تحديث الحقول الأخرى
    document.getElementById("Statue").value = statue;
    var Active = document.getElementById("EmployeeActive")=true;
    if (active == "True")
        Active.checked = true;
    else
        Active.checked = false;
    // التعامل مع السيرة الذاتية
    if (resume && resume !== "null") {
        document.getElementById("Resume").src = PathResum + resume;
    } else {
        document.getElementById("Resume").src = "";
    }

    document.getElementById("employeeModalLabel").innerHTML = lbTitleEdit;
};


function Rest() {
    document.getElementById("EmployeeId").value = "";

    document.getElementById("Name").value = "";
    document.getElementById("Email").value = "";
    document.getElementById("Phone").value = "";
    document.getElementById("date").value = "";
    document.getElementById("EmployeeActive").checked=flase;
    document.getElementById("Statue").value = "";
    document.getElementById("Resume").value = "";
    document.getElementById("employeeModalLabel").innerHTML = lbTitleAdd;

}
function ViewResume (resume){
    document.getElementById("ResumeLink").href = PathResum + resume;
}




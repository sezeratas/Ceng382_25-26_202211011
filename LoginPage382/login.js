const loginCredentials = [];

function storeCredentials(event) {
    event.preventDefault(); // Formun varsayılan submit davranışını engeller

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    loginCredentials.push({ username, password });

    console.log(loginCredentials);

}

// Function to update the clock
function updateClock() {
    const clockElement = document.getElementById('clock');
    const now = new Date();
    clockElement.innerText = now.toLocaleTimeString();
}

// Update the clock every second
setInterval(updateClock, 1000);

document.addEventListener('keydown', function(event) {
    if(event.key === 'h' || event.key === 'H') {
        var loginContainer = document.querySelector('.login-container');
        if (loginContainer.style.display === 'none') {
            loginContainer.style.display = 'flex';
        } else {
            loginContainer.style.display = 'none';
        }
    }
});

function checkLogin() {
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;
    
    if (username === "admin" && password === "admin") {
        window.location.href = "index2.html"; // Başarılı girişte yönlendirme
    } else {
        document.getElementById("message").textContent = "Hatalı kullanıcı adı veya şifre!";
    }
}

function validateInput(input) {
    var errorSpan = document.getElementById(input.id + "Error");
    if (input.value.trim() === "") {
        input.classList.add("error");
        errorSpan.style.display = "block";
    } else {
        input.classList.remove("error");
        errorSpan.style.display = "none";
    }
}

function addToTable() {
    var input1 = document.getElementById("className").value;
    var input2 = document.getElementById("NumberOfPeople").value;
    var input3 = document.getElementById("Description").value;

    if (input1.trim() === "" || input2.trim() === "" || input3.trim() === "") {
        alert("Lütfen tüm alanları doldurun!");
        return;
    }

    var tableBody = document.getElementById("tableBody");

    var newRow = document.createElement("tr"); // Yeni satır oluştur

    var cell1 = document.createElement("td");
    var cell2 = document.createElement("td");
    var cell3 = document.createElement("td");

    cell1.textContent = input1;
    cell2.textContent = input2;
    cell3.textContent = input3;

    newRow.appendChild(cell1);
    newRow.appendChild(cell2);
    newRow.appendChild(cell3);
    tableBody.appendChild(newRow);

    // Tabloyu görünür yap
    document.getElementById("tableContainer").style.display = "block";

    // Inputları temizle
    document.getElementById("className").value = "";
    document.getElementById("NumberOfPeople").value = "";
    document.getElementById("Description").value = "";
}

// third js event
document.addEventListener("DOMContentLoaded", function () {
    let inputs = document.querySelectorAll("input"); // Tüm input alanlarını seç

    inputs.forEach(input => {
        input.addEventListener("blur", function () {
            if (this.value.trim() === "") { // Eğer boşsa
                this.style.border = "2px solid red";  // Kırmızı çerçeve ekle
                this.style.backgroundColor = "#ffcccc"; // Açık kırmızı arkaplan
            } else {
                this.style.border = "2px solid green"; // Yeşil çerçeve ekle (doğru giriş)
                this.style.backgroundColor = "rgb(118, 74, 20)"; // Normal hale döndür
            }
        });
    });
});

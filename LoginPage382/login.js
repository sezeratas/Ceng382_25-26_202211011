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
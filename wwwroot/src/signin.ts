import { showToast } from "./animation.js";

const usernameTextbox = <HTMLInputElement>document.getElementById("username");
const passwordTextbox = <HTMLInputElement>document.getElementById("password");
const signinBtn = <HTMLInputElement>document.getElementById("signinBtn");

signinBtn.addEventListener("click", async () => {
    const username: string = usernameTextbox.value;
    const password: string = passwordTextbox.value;

    const response = await fetch("/signin/admin", {
        method: "POST",
        headers: {"Content-Type" : "application/json"},
        body: JSON.stringify({
            username : username,
            password : password
        })
    });

    if (response.ok){
        const data = await response.json();
        localStorage.setItem("token", data.token);
        window.location.href = "admin.html";
    }

    if (response.status === 401){
        showToast("Incorrect Admin Credentials!");
    }

    const message: string = await response.text();
    showToast(message);
});
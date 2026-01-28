import { showToast } from "./animation.js";

const usernameTextbox = <HTMLInputElement>document.getElementById("username");
const passwordTextbox = <HTMLInputElement>document.getElementById("password");
const signinBtn = <HTMLInputElement>document.getElementById("signinBtn");

signinBtn.addEventListener("click", async () => {
    const username: string = usernameTextbox.value;
    const password: string = passwordTextbox.value;

    const response = await fetch("", {
        method: "POST",
        headers: {"Content-Type" : "application/json"},
        body: JSON.stringify({
            username : username,
            password : password
        })
    });

    const message: string = await response.text();

    if (response.ok){
        window.location.href = "admin.ts";
    }

    showToast(message);
});
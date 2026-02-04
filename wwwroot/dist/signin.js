import { showToast } from "./animation.js";
const usernameTextbox = document.getElementById("username");
const passwordTextbox = document.getElementById("password");
const signinBtn = document.getElementById("signinBtn");
signinBtn.addEventListener("click", async () => {
    const username = usernameTextbox.value;
    const password = passwordTextbox.value;
    const response = await fetch("/signin/admin", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            username: username,
            password: password
        })
    });
    if (response.ok) {
        const data = await response.json();
        localStorage.setItem("token", data.token);
        window.location.href = "admin.html";
        return;
    }
    if (response.status === 401) {
        showToast("Incorrect Admin Credentials!");
        return;
    }
    const message = await response.text();
    showToast(message);
});
//# sourceMappingURL=signin.js.map
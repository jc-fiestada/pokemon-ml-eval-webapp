import { showToast } from "./animation.js";

addEventListener("DOMContentLoaded", async () => {
    const response = await fetch("/verify/page-access", {
        headers: {"Authorization" : `Bearer ${localStorage.getItem("token")}`}
    });

    if (response.status === 401){
        window.location.href = "unauthorized.html";
        return;
    }

    if (!response.ok){
        window.location.href = "error.html";
        return;
    }

    showToast("Welcome Admin!");
});